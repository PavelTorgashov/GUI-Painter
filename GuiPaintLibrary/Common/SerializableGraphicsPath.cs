using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace GuiPaintLibrary.Common
{
    /// <summary>
    /// Сериализуемая обертка над GraphicsPath
    /// Взято отсюда:
    ///   Diagrams library
    ///   Copyright (c) 2010 Pavel Torgashov.
    /// </summary>
    [Serializable]
    public sealed class SerializableGraphicsPath : ISerializable, IDisposable
    {
        public GraphicsPath Path = new GraphicsPath();

        public SerializableGraphicsPath() { }

        private SerializableGraphicsPath(SerializationInfo info, StreamingContext context)
        {
            if (info.MemberCount > 0)
            {
                var points = (PointF[])info.GetValue("p", typeof(PointF[]));
                var types = (byte[])info.GetValue("t", typeof(byte[]));
                Path = new GraphicsPath(points, types);
            }
            else
                Path = new GraphicsPath();
        }

        public void Dispose()
        {
            Path?.Dispose();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (Path.PointCount <= 0) return;
            info.AddValue("p", Path.PathPoints);
            info.AddValue("t", Path.PathTypes);
        }

        public static implicit operator GraphicsPath(SerializableGraphicsPath path)
        {
            return path.Path;
        }

        public static implicit operator SerializableGraphicsPath(GraphicsPath path)
        {
            return new SerializableGraphicsPath { Path = path };
        }
    }

}
