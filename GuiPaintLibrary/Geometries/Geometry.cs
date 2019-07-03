using System;
using GuiPaintLibrary.Common;

namespace GuiPaintLibrary.Geometries
{
    /// <summary>
    /// Допустимые операции над геометрией
    /// </summary>
    [Serializable]
    [Flags]
    public enum AllowedOperations : uint
    {
        None = 0x0,         // ничего нельзя
        Scale = 0x1,        // может быть растянут
        Size = 0x2,         // может изменять размер по горизонтали и вертикали
        Rotate = 0x4,       // может быть повёрнут
        Select = 0x8,       // может быть выбран
        Skew = 0x10,        // может быть искажён
        Vertex = 0x20,      // может изменять внутренние вершины
        Pathed = 0x40,      // может быть конвертирован в путь
        Warp = 0x80,        // может быть искажён произвольно
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }

    [Serializable]
    public abstract class Geometry
    {
        public string Name { get; set; }
        public abstract SerializableGraphicsPath Path { get; }
        public abstract AllowedOperations AllowedOperations { get; }

    }
}
