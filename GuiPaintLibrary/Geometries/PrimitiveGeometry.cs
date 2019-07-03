using GuiPaintLibrary.Common;
using System;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Содержит геометрию фиксированной формы
    /// </summary>
    [Serializable]
    public class PrimitiveGeometry : Geometry, IDisposable
    {
        readonly SerializableGraphicsPath _path = new SerializableGraphicsPath();
        readonly AllowedOperations _allowedOperations;
        public override SerializableGraphicsPath Path { get { return _path; } }
        public override AllowedOperations AllowedOperations { get { return _allowedOperations; } }

        internal PrimitiveGeometry(SerializableGraphicsPath path, AllowedOperations allowed)
        {
            Name = "Primitive";
            _path = path;
            _allowedOperations = allowed;
        }

        public void Dispose()
        {
            _path?.Dispose();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
