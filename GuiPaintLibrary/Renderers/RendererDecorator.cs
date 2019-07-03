using GuiPaintLibrary.Figures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiPaintLibrary.Renderers
{
    /// <summary>
    /// Класс декоратора для рисовальщика фигуры
    /// </summary>
    [Serializable]
    public abstract class RendererDecorator : Renderer
    {
        private readonly Renderer _renderer;

        protected RendererDecorator(Renderer renderer)
        {
            _renderer = renderer;
        }

        public static List<RendererDecorator> GetDecorators(Renderer renderer, List<RendererDecorator> list = null)
        {
            if (list == null) list = new List<RendererDecorator>();
            var rendererDecorator = renderer as RendererDecorator;
            if (rendererDecorator == null) return list;
            list.Add(rendererDecorator);
            return GetDecorators(rendererDecorator._renderer, list);
        }

        /// <summary>
        /// Ищет тип декоратора в цепочке подключенных декораторов
        /// </summary>
        /// <param name="renderer">Ссылка на рендерер фигуры</param>
        /// <param name="decoratorType">Тип декоратора для поиска</param>
        /// <returns>True - тип в цепочке найден</returns>
        public static bool ContainsType(Renderer renderer, Type decoratorType)
        {
            var rendererDecorator = renderer as RendererDecorator;
            if (rendererDecorator == null) return false;
            return rendererDecorator.GetType() == decoratorType ||
                ContainsType(rendererDecorator._renderer, decoratorType);
        }

        public static Renderer GetDecorator(Renderer renderer, Type decoratorType)
        {
            var rendererDecorator = renderer as RendererDecorator;
            if (rendererDecorator == null) return null;
            if (rendererDecorator.GetType() == decoratorType) return renderer;
            return GetDecorator(rendererDecorator._renderer, decoratorType);
        }

        /// <summary>
        /// Возвращает первичный рендерер, на который накладываются декораторы
        /// </summary>
        /// <param name="renderer">Рендерер для исследования</param>
        /// <returns>Первичный рендерер</returns>
        public static Renderer GetBaseRenderer(Renderer renderer)
        {
            var rendererDecorator = renderer as RendererDecorator;
            return rendererDecorator == null ? renderer : GetBaseRenderer(rendererDecorator._renderer);
        }

        public static bool ContainsAnyDecorator(Renderer renderer)
        {
            return renderer as RendererDecorator != null;
        }

        public static bool ExistsWithoutThisDecorator(IEnumerable<Figure> figures, Type type)
        {
            return figures.Count(figure => !ContainsType(figure.Renderer, type)) > 0;
        }

        public static IEnumerable<Figure> WhereNotContainsDecorator(IEnumerable<Figure> figures, Type type)
        {
            return figures.Where(figure => !ContainsType(figure.Renderer, type));
        }

        public static IEnumerable<Figure> WhereContainsDecorator(IEnumerable<Figure> figures, Type type)
        {
            return figures.Where(figure => ContainsType(figure.Renderer, type));
        }
    }
}
