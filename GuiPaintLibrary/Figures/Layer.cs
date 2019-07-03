using GuiPaintLibrary.Styles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiPaintLibrary.Figures
{
    /// <summary>
    /// Допустимые операции над слоем
    /// </summary>
    [Serializable]
    [Flags]
    public enum LayerAllowedOperations : uint
    {
        None = 0x0,         // ничего нельзя
        Visible = 0x1,      // элементы этого слоя могут отображаться, быть выбраны, изменены и удалены
        Print = 0x2,        // могут быть распечатаны, экспортированы
        Actived = 0x4,      // слой делается активным и вновь добавляемые элементы автоматически добавляются к нему
        Locking = 0x8,      // только отображение элементов (как фон)
        // новые режимы добавлять здесь

        All = 0xffffffff,   // всё можно
    }

    [Serializable]
    public class LayerItem
    {
        private readonly List<Figure> _figures = new List<Figure>();

        public List<Figure> Figures
        {
            get { return _figures; }
            set
            {
                _figures.Clear();
                _figures.AddRange(value);
            }
        }

        public LayerItem()
        {
            AllowedOperations = LayerAllowedOperations.All ^
                (LayerAllowedOperations.Actived | LayerAllowedOperations.Locking);
        }

        public string Name { get; set; }

        /// <summary>
        /// Допустимые операции в слое
        /// </summary>
        public LayerAllowedOperations AllowedOperations { get; private set; }

        public void AddAllowedOperation(LayerAllowedOperations op)
        {
            AllowedOperations |= op;
        }

        public void RemoveAllowedOperation(LayerAllowedOperations op)
        {
            AllowedOperations &= LayerAllowedOperations.All ^ op;
        }

        public override string ToString()
        {
            return Name;
        }

    }

    /// <summary>
    /// Слой, содержащий все фигуры
    /// </summary>
    [Serializable]
    public class Layer
    {
        private readonly List<Figure> _figures = new List<Figure>();
        private List<LayerItem> _layers = new List<LayerItem>();

        public List<Figure> Figures
        {
            get { return _figures; }
            set
            {
                _figures.Clear();
                _figures.AddRange(value);
            }
        }

        public List<LayerItem> Layers
        {
            get
            {
                return _layers;
            }
            set
            {
                // поддержка обратной совместимости
                if (_layers == null) _layers = new List<LayerItem>();
                _layers.Clear();
                _layers.AddRange(value);
            }
        }

        public Fill FillStyle { get; set; }

        public Layer()
        {
            FillStyle = new DefaultFill { IsVisible = false };
            _figures = new List<Figure>();
            _layers = new List<LayerItem>();
        }

        public bool AssignedToLayer(Figure fig)
        {
            foreach (var layer in Layers)
            {
                if (layer.Figures.Contains(fig))
                    return true;
            }
            return false;
        }

        public bool IsVisible(Figure fig)
        {
            foreach (var layer in Layers.Where(layer =>
                                 layer.AllowedOperations.HasFlag(LayerAllowedOperations.Visible)))
            {
                if (layer.Figures.Contains(fig))
                    return true;
            }
            foreach (var layer in Layers.Where(layer =>
                                 !layer.AllowedOperations.HasFlag(LayerAllowedOperations.Visible)))
            {
                if (layer.Figures.Contains(fig))
                    return false;
            }
            return true;
        }

        public bool IsLocked(Figure fig)
        {
            foreach (var layer in Layers.Where(layer =>
                                 layer.AllowedOperations.HasFlag(LayerAllowedOperations.Locking)))
            {
                if (layer.Figures.Contains(fig))
                    return true;
            }
            return false;
        }
    }
}
