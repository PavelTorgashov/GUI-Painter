#region

using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

#endregion

namespace System.Windows.Forms
{
    public class FileManager : Component, ISupportInitialize
    {
        private bool m_DocumentChanged;

        private bool m_Initialized;

        public FileManager()
        {
            DocOpenedOrCreated += delegate { };

            SaveFilter = OpenFilter = "Any file|*.*";
            DocumentType = "document";
        }

        public FileManager(IContainer container)
            : this()
        {
            DocOpenedOrCreated += delegate { };
            container.Add(this);
        }

        public ToolStripItem OpenButton { get; set; }
        public ToolStripItem SaveButton { get; set; }
        public ToolStripItem SaveAsButton { get; set; }
        public ToolStripItem NewButton { get; set; }
        public Form MainForm { get; set; }

        [DefaultValue("document")]
        public string DocumentType { get; set; }

        public string OpenFilter { get; set; }
        public string SaveFilter { get; set; }
        public string DefaultSaveExtension { get; set; }

        public string MainFormTitle { get; set; }

        [Browsable(false)]
        public string CurrentFileName { get; set; }

        [Browsable(false)]
        public bool IsDocumentChanged
        {
            get { return m_DocumentChanged; }
            set
            {
                m_DocumentChanged = value;
                UpdateInterface();
            }
        }

        [Browsable(false)]
        public object Document { get; set; }

        [Browsable(false)]
        public string Text { get; set; }

        [Browsable(false)]
        public string DefaultFolder { get; set; }

        public void BeginInit()
        {
        }

        public void EndInit()
        {
            Init();
        }

        //вызываем этот метод при закрытии документа
        public DialogResult OnDocumentClosing()
        {
            var res = DialogResult.OK;

            if (IsDocumentChanged)
                while (
                    (res =
                        MessageBox.Show("Do you want to save current " + DocumentType + "?", "Save",
                            MessageBoxButtons.YesNoCancel)) == DialogResult.Yes)
                {
                    if (Save(CurrentFileName) != DialogResult.No) //if no exceptions
                        break;
                }

            return res;
        }

        public event EventHandler DocOpenedOrCreated = delegate { };

        public event EventHandler<DocEventArgs> NewDocNeeded = delegate { };
        public event EventHandler<DocEventArgs> SaveDocNeeded;
        public event EventHandler<DocEventArgs> OpenDocNeeded;

        private void Init()
        {
            if (SaveButton != null)
                SaveButton.Click += delegate { Save(CurrentFileName); };
            if (SaveAsButton != null)
                SaveAsButton.Click += delegate { Save(null); };
            if (OpenButton != null)
                OpenButton.Click += MiOpenClick;

            if (MainForm != null)
                MainForm.FormClosing += (o, e) => { e.Cancel = OnDocumentClosing() == DialogResult.Cancel; };

            if (NewButton != null)
                NewButton.Click += MiNewClick;

            m_Initialized = true;

            OnNewDoc(true);
        }

        private void MiNewClick(object sender, EventArgs e)
        {
            var res = OnDocumentClosing(); //спрашиваем пользователя, не хочет ли он сохранить текущий документ
            if (res == DialogResult.Cancel)
                return;

            OnNewDoc(false);
        }

        private void OnNewDoc(bool firstDocument)
        {
            if (DesignMode)
                return;

            var ea = new DocEventArgs(firstDocument);
            NewDocNeeded(this, ea);
            Document = ea.Document;
            CurrentFileName = null;
            IsDocumentChanged = false;

            DocOpenedOrCreated(this, EventArgs.Empty);
        }

        private void MiOpenClick(object sender, EventArgs e)
        {
            var res = OnDocumentClosing(); //спрашиваем пользователя, не хочет ли он сохранить текущий документ
            if (res == DialogResult.Cancel)
                return;

            var ofd = new OpenFileDialog { Filter = OpenFilter };
            if (!string.IsNullOrEmpty(DefaultFolder))
                ofd.InitialDirectory = DefaultFolder;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //читаем данные
                OnOpen(ofd.FileName);
                //запоминаем имя файла, откуда загрузили
                CurrentFileName = ofd.FileName;
                IsDocumentChanged = false; //сбрасываем флаг измененных данных
            }
        }

        private void OnOpen(string filePath)
        {
            if (OpenDocNeeded != null)
            {
                var ea = new DocEventArgs(false) { FileName = filePath };
                OpenDocNeeded(this, ea);
                Document = ea.Document;
            }
            else
            {
                using (var stream = File.OpenRead(filePath))
                using (var zip = new GZipStream(stream, CompressionMode.Decompress))
                    Document = new BinaryFormatter().Deserialize(zip);
            }

            DocOpenedOrCreated(this, EventArgs.Empty);
        }

        //save or saveAs
        public DialogResult Save(string fileName)
        {
            try
            {
                if (fileName == null)
                {
                    //save as...
                    var sfd = new SaveFileDialog { Filter = SaveFilter, DefaultExt = DefaultSaveExtension };
                    if (!string.IsNullOrEmpty(DefaultFolder))
                        sfd.InitialDirectory = DefaultFolder;

                    if (sfd.ShowDialog() == DialogResult.OK)
                        fileName = sfd.FileName;
                    else
                        return DialogResult.Cancel;
                }
                //save
                OnSave(fileName);
                //
                CurrentFileName = fileName;
                IsDocumentChanged = false;
                //
                return DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return DialogResult.No;
            }
        }

        private void OnSave(string fileName)
        {
            if (SaveDocNeeded != null)
                SaveDocNeeded(this, new DocEventArgs(false) { Document = Document, FileName = fileName });
            else
                using (var stream = File.OpenWrite(fileName))
                using (var zip = new GZipStream(stream, CompressionMode.Compress))
                    new BinaryFormatter().Serialize(zip, Document);
        }

        private void UpdateInterface()
        {
            if (!m_Initialized)
                return;

            this.Text = string.Format("{0} - {1}", MainFormTitle, CurrentFileName == null ? "New " + DocumentType : Path.GetFileName(CurrentFileName));

            if (MainForm != null)
                MainForm.Text = Text;

            if (SaveButton != null)
                SaveButton.Enabled = IsDocumentChanged;
        }
    }

    public class DocEventArgs : EventArgs
    {
        public object Document { get; set; }
        public string FileName { get; set; }
        public bool FirstDocument { get; private set; }

        public DocEventArgs(bool firstDocument)
        {
            this.FirstDocument = firstDocument;
        }
    }
}