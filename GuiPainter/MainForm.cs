using GuiPainter.Common;
using GuiPainter.Controllers;
using GuiPainter.Controls;
using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using GuiPaintLibrary.Geometries;
using GuiPaintLibrary.Renderers;
using GuiPaintLibrary.Selections;
using GuiPaintLibrary.Styles;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GuiPainter
{
    public partial class MainForm : Form
    {
        readonly string _caption;
        readonly VersionInfo _versionInfo;

        Layer _layer;
        SelectionController _selectionController;
        UndoRedoController _undoRedoController;

        private bool FileChanged { get; set; }

        public MainForm()
        {
            InitializeComponent();

            // вариант для отладки
            pbCanvas.Size = new Size(1000, 1000);

            _versionInfo = new VersionInfo();
            _caption = string.Format("GUI Painter (Ver {0:0.0})",
                (decimal)_versionInfo.Version / 10);
            _layer = new Layer();
            ConnectMethods();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BuildInterface();
        }

        private void ConnectMethods()
        {
            _undoRedoController = new UndoRedoController(_layer);
            _selectionController = new SelectionController(_layer);

            // подключение обработчиков событий для контроллера выбора
            _selectionController.SelectedFigureChanged += BuildInterface;
            _selectionController.SelectedTransformChanging += UpdateInterface;
            _selectionController.SelectedTransformChanged += UpdateInterface;
            _selectionController.SelectedRangeChanging += _selectionController_SelectedRangeChanging;
            _selectionController.EditorModeChanged += _ => UpdateInterface();
            _selectionController.LayerStartChanging += () =>
                                      OnLayerStartChanging(_selectionController.EditorMode.ToString());
            _selectionController.LayerChanged += OnLayerChanged;
            _selectionController.ResetFigureCreator += _selectionController_ResetCreateFigureSelector;
        }

        private void _selectionController_ResetCreateFigureSelector()
        {
            tsbPointer.Enabled = false;
        }

        void OnLayerStartChanging(string opName)
        {
            _undoRedoController.OnStartOperation(opName);
        }

        void OnLayerChanged()
        {
            _undoRedoController.OnFinishOperation();
            UpdateCanvasSize();
            UpdateFiguresTree();
        }

        /// <summary>
        /// Действия по корректировке размера холста по размеру содержимого слоя
        /// </summary>
        private void UpdateCanvasSize()
        {
            if (_layer == null) return;
            var size = new Size();
            foreach (var figrect in _layer.Figures.Select(figure =>
                figure.GetTransformedPath().Path.GetBounds()))
            {
                if (size.Width < figrect.Right) size.Width = (int)figrect.Right;
                if (size.Height < figrect.Bottom) size.Height = (int)figrect.Bottom;
            }

            // запас для скролирования. Что бы можно было тянуть маркеры.
            size.Width += 50;
            size.Height += 50;

            var sf = _selectionController.ScaleFactor;
            size = new Size((int)(size.Width * sf), (int)(size.Height * sf));

            var pnlSize = pnlScroll.ClientSize;
            pbCanvas.Size = new Size(size.Width < pnlSize.Width ? pnlSize.Width : size.Width,
                                     size.Height < pnlSize.Height ? pnlSize.Height : size.Height);
        }

        void BuildInterface()
        {
            //build tools
            foreach (var editor in pnTools.Controls.OfType<IEditor<LayerSelectionInfo>>()) //get editors of layer
                editor.Build(new LayerSelectionInfo { Layer = _layer, Selection = _selectionController.Selection });

            foreach (var editor in pnTools.Controls.OfType<IEditor<Selection>>()) //get editors of figure
                editor.Build(_selectionController.Selection);

            //
            UpdateFiguresTree();
            UpdateInterface();
        }

        private void _selectionController_SelectedRangeChanging(Rectangle rect, float angle)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Area: {0}", rect);
            if (Math.Abs(angle - 0) > float.Epsilon)
                sb.AppendFormat(" Rotate at: {0:0.0}°", angle);
            if (_selectionController.EditorMode == EditorMode.Drag &&
                _selectionController.Selection.Count > 0)
            {
                var figure = _selectionController.Selection.FirstOrDefault();
                if (figure != null)
                {
                    angle = Helper.GetAngle(figure.Transform);
                    var size = Helper.GetSize(figure.Transform);
                    sb.AppendFormat(" Figure: size {0}, rotated at {1:0.0}°", size, angle);
                }
            }

            //tsslRibbonRect.Text = sb.ToString();
            UpdateInterface();
        }

        private void timerCheckClipboard_Tick(object sender, EventArgs e)
        {
            tsbPaste.Enabled = tsmiPaste.Enabled = _selectionController.CanPasteFromClipboard;
        }

        private void UpdateInterface()
        {
            //tsbPolyline.Enabled = tsbRomb.Enabled =
            //    tsbText.Enabled = tsbPicture.Enabled = 
            tsbRect.Enabled = _selectionController.EditorMode == EditorMode.Select;

            //tsbSelectMode.Checked = _selectionController.EditorMode == EditorMode.Select;
            //tsbSkewMode.Checked = _selectionController.EditorMode == EditorMode.Skew;
            //tsbWarpMode.Checked = _selectionController.EditorMode == EditorMode.Warp;
            //tsbVertexMode.Checked = _selectionController.EditorMode == EditorMode.Verticies;

            var changedUndo = FileChanged != UndoRedoManager.Instance.CanUndo;
            tsmiUndo.Enabled = tsbUndo.Enabled = FileChanged = UndoRedoManager.Instance.CanUndo;
            var changedRedo = tsbRedo.Enabled != UndoRedoManager.Instance.CanRedo;
            tsmiRedo.Enabled = tsbRedo.Enabled = UndoRedoManager.Instance.CanRedo;
            tsmiSave.Enabled = tsbSave.Enabled = FileChanged;
            //tsslEditorMode.Text = string.Format("Mode: {0}", _selectionController.EditorMode);
            var exists = _selectionController.Selection.All(f =>
                                 f.Geometry is PrimitiveGeometry && f.Renderer is DefaultRenderer);
            //tsddbGeometySwitcher.Enabled = exists;
            //tsddbFillBrushSwitcher.Enabled = tsddbEffectSwitcher.Enabled = tsmDuplicateFigure.Enabled = tsmiDuplicateFigure.Enabled =
            //     tsbFlipX.Enabled = tsbFlipY.Enabled = tsbRotate90Ccw.Enabled = tsbRotate90Cw.Enabled = tsbRotate180.Enabled =
            //     tsmDelete.Enabled = tsmiDelete.Enabled =
            //        tsbBringToFront.Enabled = tsbSendToBack.Enabled = tsbUpToFront.Enabled = tsbSendToDown.Enabled =
            //        tsmiAssignedToLayer.Enabled = tsbDublicate.Enabled = 
            tsbCopy.Enabled = tsmiCopy.Enabled = tsbCut.Enabled = tsmiCut.Enabled = _selectionController.Selection.Count > 0; ;

            //tsbAlignLeft.Enabled = tsbAlignCenter.Enabled = tsbAlignRight.Enabled =
            //     tsbAlignTop.Enabled = tsbAlignMiddle.Enabled = tsbAlignBottom.Enabled = _selectionController.Selection.Count > 1;
            //tsbEvenHorizontalSpaces.Enabled = tsbEvenVerticalSpaces.Enabled = _selectionController.Selection.Count > 2;

            //tsbSameWidth.Enabled = tsbSameHeight.Enabled = tsbSameBothSizes.Enabled =
            //             _selectionController.Selection.Count(SelectionHelper.IsNotSkewAndRotated) > 1;

            //tsbConvertToPath.Enabled = _selectionController.Selection.Count(fig =>
            //            fig.Geometry.AllowedOperations.HasFlag(AllowedOperations.Pathed)) > 0;

            if (changedUndo || changedRedo)
                UpdateFiguresTree();

            //try
            //{
            //    cbScaleFactor.SelectedIndexChanged -= cbScaleFactor_SelectedIndexChanged;
            //    cbScaleFactor.Text = string.Format("{0}%", _selectionController.ScaleFactor * 100);
            //}
            //finally
            //{
            //    cbScaleFactor.SelectedIndexChanged += cbScaleFactor_SelectedIndexChanged;
            //}

            pbCanvas.Invalidate();
        }

        private void UpdateFiguresTree()
        {
            var first = _selectionController.Selection.FirstOrDefault();
            try
            {
                tvFigures.AfterSelect -= tvFigures_AfterSelect;
                tvFigures.BeginUpdate();
                try
                {
                    tvFigures.Nodes.Clear();
                    var noLayers = new TreeNode("(no layers)");
                    tvFigures.Nodes.Add(noLayers);
                    AddToTree(noLayers.Nodes, null, first);
                    foreach (var layer in _layer.Layers.OrderBy(layer => layer.Name)
                                                       .Where(layer => layer.Figures.Count > 0))
                    {
                        var nodeLayer = new TreeNode(layer.Name);
                        tvFigures.Nodes.Add(nodeLayer);
                        AddToTree(nodeLayer.Nodes, layer, first);
                    }
                }
                finally
                {
                    tvFigures.EndUpdate();
                }
            }
            finally
            {
                if (tvFigures.SelectedNode != null)
                    tvFigures.SelectedNode.EnsureVisible();
                tvFigures.AfterSelect += tvFigures_AfterSelect;
            }
        }

        private void tvFigures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            _selectionController.Selection.Clear();
            if (e.Node != null)
            {
                var fignode = e.Node as FigureTreeNode;
                if (fignode == null) return;
                if (!_layer.IsVisible(fignode.Figure) ||
                    _layer.IsLocked(fignode.Figure)) return;
                _selectionController.Selection.Add(fignode.Figure);
                _selectionController.UpdateMarkers();
                BuildInterface();
            }
        }

        private void AddToTree(TreeNodeCollection nodes, LayerItem layer, Figure first)
        {
            foreach (var fig in _layer.Figures.ToList().AsEnumerable().Reverse())
            {
                if (layer == null && !_layer.AssignedToLayer(fig) ||
                    layer != null && layer.Figures.Contains(fig))
                {
                    var state = !_layer.IsVisible(fig)
                        ? " (hidden)"
                        : _layer.IsLocked(fig) ? " (locked)" : "";
                    var fignode = new FigureTreeNode(
                        string.Format("{0}{1}", fig.Geometry, state))
                    { Figure = fig };
                    nodes.Add(fignode);
                    AddDecorators(fig, fignode);
                    if (fig == first)
                    {
                        tvFigures.SelectedNode = fignode;
                        fignode.ExpandAll();
                    }
                }
            }
        }

        private static void AddDecorators(Figure fig, FigureTreeNode fignode)
        {
            if (Helper.ContainsAnyDecorator(fig))
            {
                if (FillDecorator.ContainsAnyDecorator(fig.Style.FillStyle))
                {
                    var list = FillDecorator.GetDecorators(fig.Style.FillStyle);
                    foreach (var item in list)
                        fignode.Nodes.Add(string.Format("{0}FillStyleDecorator", item));
                }
                if (RendererDecorator.ContainsAnyDecorator(fig.Renderer))
                {
                    var list = RendererDecorator.GetDecorators(fig.Renderer);
                    foreach (var item in list)
                        fignode.Nodes.Add(string.Format("{0}RendererDecorator", item));
                }
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var scaleFactor = _selectionController.ScaleFactor;
            graphics.ScaleTransform(scaleFactor, scaleFactor);

            if (_layer.FillStyle.IsVisible)
                using (var brush = _layer.FillStyle.GetBrush(null))
                    graphics.FillRectangle(brush, pbCanvas.ClientRectangle);

            // отрисовка созданных фигур
            foreach (var fig in _layer.Figures.Where(fig => _layer.IsVisible(fig)))
            {
                fig.Renderer.Render(graphics, fig);
            }

            // отрисовка выделения
            _selectionController.Selection.Renderer.Render(graphics,
                                                           _selectionController.Selection);
            // отрисовка маркеров
            foreach (var marker in _selectionController.Markers)
            {
                marker.Transform.Matrix.Scale(1 / scaleFactor, 1 / scaleFactor);
                marker.Renderer.Render(graphics, marker);
                marker.Transform.Matrix.Scale(scaleFactor, scaleFactor);
            }
        }

        private void pbCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (e.Clicks == 2)
                _selectionController.OnDblClick(e.Location, ModifierKeys);
            else
                _selectionController.OnMouseDown(e.Location, ModifierKeys);
        }

        private void pbCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // состояние курсора обрабатывается вне зависимости от нажатых клавиш мышки
            Cursor = _selectionController.GetCursor(e.Location, ModifierKeys, e.Button);
            _selectionController.OnMouseMove(e.Location, ModifierKeys);
        }

        private void pbCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _selectionController.OnMouseUp(e.Location, ModifierKeys);
        }

        /// <summary>
        /// Выбор фигур для последующего создания
        /// </summary>
        private void btnCreateFigure_Click(object sender, EventArgs e)
        {
            _selectionController.Clear();
            Func<Figure> figureCreator = null;
            Cursor figureCreatorCursor = CursorFactory.GetCursor(UserCursor.SelectByRibbonRect);
            if (sender == tsbPointer)
            {
                figureCreatorCursor = Cursors.Default;
                Cursor = Cursors.Default;
                tsbPointer.Enabled = false;

                _selectionController.CreateFigureCursor = figureCreatorCursor;
                _selectionController.CreateFigureRequest = figureCreator;
                return;
            }
            tsbPointer.Enabled = true;
            //
            if (sender == btnRectangle || sender == tsbRect)
            {
                figureCreatorCursor = Cursor = CursorFactory.GetCursor(UserCursor.CreateRect);
                figureCreator = () =>
                {
                    var fig = new Figure();
                    FigureBuilder.BuildRectangleGeometry(fig);
                    return fig;
                };
            }
            else if (sender == btnRoundedRectangle)
            {
                figureCreatorCursor = Cursor = CursorFactory.GetCursor(UserCursor.CreateRect);
                figureCreator = () =>
                {
                    var fig = new Figure();
                    FigureBuilder.BuildRoundedRectangleGeometry(fig, 0.25f);
                    return fig;
                };
            }
            //
            _selectionController.CreateFigureCursor = figureCreatorCursor;
            _selectionController.CreateFigureRequest = figureCreator;
        }

        private void tsmiUndo_Click(object sender, EventArgs e)
        {
            _selectionController.Clear();
            UndoRedoManager.Instance.Undo();
            UpdateInterface();
        }

        private void tsmiRedo_Click(object sender, EventArgs e)
        {
            _selectionController.Clear();
            UndoRedoManager.Instance.Redo();
            UpdateInterface();
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            _selectionController.CutSelectedToClipboard();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            _selectionController.CopySelectedToClipboard();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            if (!_selectionController.CanPasteFromClipboard) return;
            _selectionController.PasteFromClipboardAndSelected();
        }
    }
}
