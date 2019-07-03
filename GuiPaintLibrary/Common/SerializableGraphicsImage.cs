using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;

namespace GuiPaintLibrary.Common
{
    [Serializable]
    public sealed class SerializableGraphicsImage : ISerializable, IDisposable
    {
        public Bitmap Bitmap { get; set; }

        private SerializableGraphicsImage() { }

        private SerializableGraphicsImage(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount == 1)
            {
                var bytes = (byte[])info.GetValue("m", typeof(byte[]));
                using (var m = new MemoryStream(bytes))
                {
                    Bitmap = new Bitmap(m);
                }
            }
            else
                Bitmap = null;
        }

        public void Dispose()
        {
            Bitmap?.Dispose();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Bitmap == null) return;
            using (var m = new MemoryStream())
            {
                Bitmap.Save(m, ImageFormat.Png);
                info.AddValue("m", m.ToArray());
            }
        }

        public static implicit operator Image(SerializableGraphicsImage image)
        {
            return image != null ? image.Bitmap : null;
        }

        public static implicit operator SerializableGraphicsImage(Bitmap image)
        {
            return new SerializableGraphicsImage { Bitmap = image };
        }
    }
}
