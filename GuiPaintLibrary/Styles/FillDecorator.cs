using GuiPaintLibrary.Figures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiPaintLibrary.Styles
{
    /// <summary>
    /// Класс декоратора для заполнения фигуры
    /// </summary>
    [Serializable]
    public abstract class FillDecorator : Fill
    {
        private readonly Fill _fill;

        protected FillDecorator(Fill fill)
        {
            _fill = fill;
        }

        public static List<FillDecorator> GetDecorators(Fill fill, List<FillDecorator> list = null)
        {
            if (list == null) list = new List<FillDecorator>();
            var fillDecorator = fill as FillDecorator;
            if (fillDecorator == null) return list;
            list.Add(fillDecorator);
            return GetDecorators(fillDecorator._fill, list);
        }

        public static bool ContainsType(Fill fill, Type decoratorType)
        {
            var fillDecorator = fill as FillDecorator;
            if (fillDecorator == null) return false;
            return fillDecorator.GetType() == decoratorType ||
                ContainsType(fillDecorator._fill, decoratorType);
        }

        public static Fill GetDecorator(Fill fill, Type decoratorType)
        {
            var fillDecorator = fill as FillDecorator;
            if (fillDecorator == null) return null;
            if (fillDecorator.GetType() == decoratorType) return fill;
            return GetDecorator(fillDecorator._fill, decoratorType);
        }

        public static Fill GetBaseFill(Fill fill)
        {
            var fillDecorator = fill as FillDecorator;
            return fillDecorator == null ? fill : GetBaseFill(fillDecorator._fill);
        }

        public static bool ContainsAnyDecorator(Fill fill)
        {
            return fill as FillDecorator != null;
        }

        public static bool ExistsWithoutThisDecorator(IEnumerable<Figure> figures, Type type)
        {
            return figures.Count(figure => !ContainsType(figure.Style.FillStyle, type)) > 0;
        }

        public static IEnumerable<Figure> WhereContainsDecorator(IEnumerable<Figure> figures, Type type)
        {
            return figures.Where(figure => !ContainsType(figure.Style.FillStyle, type));
        }

    }
}
