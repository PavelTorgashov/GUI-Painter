using GuiPaintLibrary.Common;
using GuiPaintLibrary.Figures;
using GuiPaintLibrary.Geometries;
using GuiPaintLibrary.Renderers;
using GuiPaintLibrary.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace GuiPaintLibrary.Selections
{
    /// <summary>
    /// Набор выделенных фигур и операции над ними
    /// </summary>
    [Serializable]
    public class Selection : Figure, IEnumerable<Figure>
    {
        // внутренний набор для хранения списка выделенных фигур
        private readonly HashSet<Figure> _selected = new HashSet<Figure>();
        private bool _isFrameVisible = true;

        /// <summary>
        /// Очистка списка выделенных фигур
        /// </summary>
        public void Clear()
        {
            _selected.Clear();
            GrabGeometry();
        }

        /// <summary>
        /// Добавление фигуры к списку выделенных фигур
        /// </summary>
        /// <param name="fig">Добавляемая фигура</param>
        public void Add(Figure fig)
        {
            _selected.Add(fig);
            GrabGeometry();
        }

        /// <summary>
        /// Исключение фигуры из списка выделенных фигур
        /// </summary>
        /// <param name="fig">Исключаемая фигура</param>
        public void Remove(Figure fig)
        {
            _selected.Remove(fig);
            GrabGeometry();
        }

        /// <summary>
        /// Запрет рисовать рамку вокруг выбранных фигур
        /// </summary>
        public bool IsFrameVisible
        {
            private get { return _isFrameVisible; }
            set
            {
                if (_isFrameVisible == value) return;
                _isFrameVisible = value;
                GrabGeometry();
            }
        }

        /// <summary>
        /// Ищем попадание в контур фигуры
        /// </summary>
        /// <param name="point">Положение курсора</param>
        /// <param name="layer">Ссылка на слой</param>
        /// <returns>Контур фигуры найден</returns>
        public bool IsHit(Layer layer, Point point)
        {
            var found = false;
            using (var pen = new Pen(Color.Black, 5))
            {
                // просмотр начинаем с конца списка - там самые "верхние" фигуры
                for (var i = layer.Figures.Count - 1; i >= 0; i--)
                {
                    var fig = layer.Figures[i];
                    var path = fig.GetTransformedPath();
                    // проверяем также попадание на контур фигуры
                    if (!path.Path.IsOutlineVisible(point, pen)) continue;
                    found = true;
                    break;
                }
            }
            return found;
        }

        /// <summary>
        /// Ищем фигуру в данной точке
        /// </summary>
        /// <param name="layer">Ссылка на слой</param>
        /// <param name="point">Положение курсора</param>
        /// <param name="figure">Найденная фигура или null</param>
        /// <returns>True - фигура найдена</returns>
        public bool FindFigureAt(Layer layer, Point point, out Figure figure)
        {
            figure = null;
            var found = false;
            using (var pen = new Pen(Color.Black, 5))
            {
                // просмотр начинаем с конца списка - там самые "верхние" фигуры
                for (var i = layer.Figures.Count - 1; i >= 0; i--)
                {
                    var fig = layer.Figures[i];
                    if (!layer.IsVisible(fig) || layer.IsLocked(fig)) continue;
                    var closedGeometry = fig.Geometry as IGeometry;
                    var isClosed = closedGeometry == null ||
                        closedGeometry != null && closedGeometry.IsClosed;
                    var path = fig.GetTransformedPath();
                    if (isClosed && path.Path.IsVisible(point))
                    {
                        figure = fig;
                        found = true;
                        break;
                    }
                    // проверяем также попадание на контур фигуры
                    if (!path.Path.IsOutlineVisible(point, pen)) continue;
                    figure = fig;
                    found = true;
                    break;
                }
            }
            return found;
        }

        /// <summary>
        /// Копирование геометрий выделенных фигур в свою геометрию
        /// </summary>
        private void GrabGeometry()
        {
            // захватываем геометрию выбранных фигур
            var path = new SerializableGraphicsPath();
            foreach (var fig in _selected)
            {
                path.Path.SetMarkers();
                path.Path.AddPath(fig.GetTransformedPath(), false);
            }

            // нарисовать рамку вокруг выбранных фигур 
            if (IsFrameVisible)
            {
                var bounds = path.Path.GetBounds();
                path.Path.AddRectangle(bounds);
            }
            // выбираем разрешённые операции
            // если выбрана только одна фигура - просто используем её AllowedOperations
            // иначе - разрешаем все операции
            var allowedOperations = _selected.Count == 1
                ? _selected.First().Geometry.AllowedOperations
                : AllowedOperations.All;

            // присваиваем геометрию
            Geometry = new PrimitiveGeometry(path, allowedOperations);

            // сбрасываем преобразование в единичную матрицу
            Transform = new SerializableGraphicsMatrix();
        }

        /// <summary>
        /// Применение своего Transform к Transform выделенных фигур
        /// </summary>
        public void PushTransformToSelectedFigures()
        {
            foreach (var fig in _selected)
                fig.PushTransform(Transform);
            GrabGeometry();
        }

        /// <summary>
        /// Перенос фигур из списка
        /// </summary>
        /// <param name="offsetX">Смещение по горизонтали</param>
        /// <param name="offsetY">Смещение по вертикали</param>
        public void Translate(float offsetX, float offsetY)
        {
            var m = new SerializableGraphicsMatrix();
            m.Matrix.Translate(offsetX, offsetY);
            // делаем на "чистой" матрице
            Transform = m;
        }

        /// <summary>
        /// Масштабирование
        /// </summary>
        /// <param name="scaleX">Коэффициент масштабирования по горизонтали</param>
        /// <param name="scaleY">Коэффициент масштабирования по вертикали</param>
        /// <param name="anchor">Координаты "якоря" (в мировых координатах)</param>
        public void Scale(float scaleX, float scaleY, PointF anchor)
        {
            //можем менять размер?
            var allowSize = Geometry.AllowedOperations.HasFlag(AllowedOperations.Size);
            var allowScale = Geometry.AllowedOperations.HasFlag(AllowedOperations.Scale);

            if (!allowScale && !allowSize)
                return; //не можем менять размеры

            if (allowScale && !allowSize)
                //можем шкалировать, но с сохранением аспекта
                //сохраняем аспект
                scaleX = scaleY = Math.Max(scaleX, scaleY);

            //шкалируем относительно якоря
            var m = new SerializableGraphicsMatrix();
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            m.Matrix.Scale(scaleX, scaleY);            //масштабируем
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат

            //
            Transform = m;
        }

        /// <summary>
        /// Искривление
        /// </summary>
        /// <param name="skewX">Коэффициент сдвига по горизонтали</param>
        /// <param name="skewY">Коэффициент сдвига по вертикали</param>
        /// <param name="anchor">Координаты "якоря" (в мировых координатах)</param>
        public void Skew(float skewX, float skewY, PointF anchor)
        {
            //можем искажать?
            var allowSkew = Geometry.AllowedOperations.HasFlag(AllowedOperations.Skew);
            if (!allowSkew)
                return; //не можем искажать

            //сдвигаем относительно якоря
            var m = new SerializableGraphicsMatrix();
            m.Matrix.Translate(anchor.X, anchor.Y);    //переводим центр координат в якорь
            m.Matrix.Shear(skewX, skewY);              //сдвигаем
            m.Matrix.Translate(-anchor.X, -anchor.Y);  //возвращаем центр координат

            //
            Transform = m;
        }

        /// <summary>
        /// Поворот
        /// </summary>
        /// <param name="angle">Угол поворота, в градусах</param>
        /// <param name="center">Координаты центра вращения (в мировых координатах)</param>
        public void Rotate(float angle, PointF center)
        {
            //можем вращать?
            var allowRotate = Geometry.AllowedOperations.HasFlag(AllowedOperations.Rotate);

            if (!allowRotate)
                return; //не можем вращать

            //вращаем относительно якоря
            var m = new SerializableGraphicsMatrix();
            m.Matrix.RotateAt(angle, center);      //вращаем

            //
            Transform = m;
        }

        /// <summary>
        /// Наличие фигуры в списке выбранных
        /// </summary>
        /// <param name="figure">Проверяемая фигура</param>
        /// <returns>True - фигура в списке</returns>
        public bool Contains(Figure figure)
        {
            return _selected.Contains(figure);
        }

        /// <summary>
        /// Конструктор с инициализацией по умолчанию
        /// </summary>
        public Selection()
        {
            // настройки вида рамки выбора
            Style.BorderStyle.DashStyle = DashStyle.Dash;
            Style.BorderStyle.Color = Color.Magenta;
            Style.FillStyle.IsVisible = false;
            // в списке ничего нет, но объект Geometry инициализируется
            GrabGeometry();
        }

        /// <summary>
        /// Количество фигур в списке
        /// </summary>
        public int Count { get { return _selected.Count; } }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return _selected.GetEnumerator();
        }

        /// <summary>
        /// Перемещение точки градиента
        /// </summary>
        /// <param name="owner">Фигура</param>
        /// <param name="index">Индекс точки градиента</param>
        /// <param name="newPosition">Смещение</param>
        public void MoveGradient(Figure owner, int index, PointF newPosition)
        {
            var gradient = owner.Style.FillStyle as IGradientFill;
            if (gradient == null)
                return; // работаем только с градиентами

            //get points in world coordinates
            var points = gradient.GetGradientPoints(owner);

            //move point
            points[index] = newPosition;

            //push
            gradient.SetGradientPoints(owner, points);
        }

        /// <summary>
        /// Перемещение точки искажения
        /// </summary>
        /// <param name="owner">Фигура</param>
        /// <param name="index">Индекс точки искажения</param>
        /// <param name="newPosition">Смещение</param>
        public void MoveWarpNode(Figure owner, int index, PointF newPosition)
        {
            var warped = (WarpRendererDecorator)RendererDecorator.GetDecorator(owner.Renderer, typeof(WarpRendererDecorator));
            if (warped == null)
                return; // работаем только с градиентами

            //get points in world coordinates
            var points = warped.GetWarpPoints(owner);

            //move point
            points[index] = newPosition;

            //push
            warped.SetWarpPoints(owner, points);
        }

        /// <summary>
        /// Перемещение вершины
        /// </summary>
        /// <param name="owner">Фигура</param>
        /// <param name="index">Индекс вершины</param>
        /// <param name="newPosition">Смещение</param>
        public void MoveVertex(Figure owner, int index, PointF newPosition)
        {
            //можем менять положение вершин?
            var allowVertex = owner.Geometry.AllowedOperations.HasFlag(AllowedOperations.Vertex);

            if (!allowVertex)
                return; //не можем менять положение вершин

            var transformed = owner.Geometry as ITransformedGeometry;
            if (transformed == null)
                return; //работаем только с полигонами

            //get points in world coordinates
            var points = transformed.GetTransformedPoints(owner);

            //move point
            if (index >= 0 && index < points.Length)
                points[index] = newPosition;

            //push
            transformed.SetTransformedPoints(owner, points);
            //
            GrabGeometry();
        }

        /// <summary>
        /// Удаление выбранной вершины фигуры
        /// </summary>
        /// <param name="owner">Фигура</param>
        /// <param name="index">Индекс вершины</param>
        public void RemoveVertex(Figure owner, int index)
        {
            //можем менять положение вершин?
            var allowVertex = Geometry.AllowedOperations.HasFlag(AllowedOperations.Vertex);

            if (!allowVertex)
                return; //не можем менять положение вершин
            List<PointF> points;
            var polygone = owner.Geometry as PolygoneGeometry;
            if (polygone != null)
            {
                // работаем только с полигонами
                points = new List<PointF>(polygone.Points);

                if (polygone.IsClosed && points.Count < 4) return;
                if (!polygone.IsClosed && points.Count < 3) return;

                //удаляем вершину
                points.RemoveAt(index);
                //push
                polygone.Points = points.ToArray();
                //
                GrabGeometry();
                return;
            }
            var bezier = owner.Geometry as BezierGeometry;
            if (bezier == null) return;
            points = new List<PointF>(bezier.Points);
            // если точек минимум для кривой, выходим
            if (points.Count < 5) return;
            var types = new List<byte>(bezier.Types);
            // если вершина угловая, то удаляем только её
            if ((types[index] & 0x27) == (byte)PathPointType.Line)
            {
                //удаляем вершину
                points.RemoveAt(index);
                // и её тип
                types.RemoveAt(index);
            }
            // если это начальная точка пути
            else if ((types[index] & 0x07) == (byte)PathPointType.Start)
            {
                // запоминаем её тип, понятно, что это ноль, а вдруг?
                var typ = types[index];
                if ((types[index + 1] & 0x07) == (byte)PathPointType.Line)
                {
                    // удаляем вершину и тип
                    points.RemoveAt(index);
                    types.RemoveAt(index);
                    // присваиваем теперь начальной точке её тип
                    types[index] = typ;
                }
                else
                {
                    // иначе предполагается, удаляется гладкая вершина
                    // поэтому удаляем сразу три точки
                    for (var i = 0; i < 3; i++)
                    {
                        points.RemoveAt(index);
                        types.RemoveAt(index);
                    }
                    // и присваиваем теперь оставшейся начальной точке её тип
                    types[index] = typ;
                }
            }
            else
            {
                // удаляем две предыдущие контрольные точки и вершину
                index -= 2;
                for (var i = 0; i < 3; i++)
                {
                    points.RemoveAt(index);
                    types.RemoveAt(index);
                }
            }
            //push
            bezier.Points = points.ToArray();
            bezier.Types = types.ToArray();
            //
            GrabGeometry();
        }

        /// <summary>
        /// Вставка новой вершины в контур фигуры
        /// </summary>
        /// <param name="owner">Фигура</param>
        /// <param name="point">Положение указателя мышки</param>
        public void InsertVertex(Figure owner, PointF point)
        {
            //можем менять положение вершин?
            var allowVertex = Geometry.AllowedOperations.HasFlag(AllowedOperations.Vertex);

            if (!allowVertex)
                return; //не можем менять положение вершин

            var transformed = owner.Geometry as ITransformedGeometry;
            if (transformed == null)
                return; //работаем только с полигонами

            //get points in world coordinates
            var points = transformed.GetTransformedPoints(owner).ToList();

            using (var pen = new Pen(Color.Black, 5))
            {
                // поищем, на какой стороне фигуры добавлять новую вершину
                var k = transformed.IsClosed ? 0 : 1;
                for (var i = k; i < points.Count; i++)
                {
                    // замыкаем контур отрезком, соединяющим начало и конец фигуры
                    var pt1 = i == 0 ? points[points.Count - 1] : points[i - 1];
                    var pt2 = points[i];
                    using (var path = new GraphicsPath())
                    {
                        path.AddLine(pt1, pt2);
                        if (!path.IsOutlineVisible(point, pen)) continue;
                        if (owner.Geometry is PolygoneGeometry)
                        {
                            // вставляем новую точку
                            points.Insert(i, point);
                            //push
                            transformed.SetTransformedPoints(owner, points.ToArray());
                        }
                        else
                        {
                            var bezier = owner.Geometry as BezierGeometry;
                            if (bezier == null) break;
                            var types = bezier.Types.ToList();
                            // вставляем две контрольные точки и вершину
                            for (var j = 0; j < 3; j++)
                            {
                                points.Insert(i, point);
                                types.Insert(i, (byte)PathPointType.Bezier);
                            }
                            transformed.SetTransformedPoints(owner, points.ToArray());
                            bezier.Types = types.ToArray();
                        }
                        break;
                    }
                }
            }
            //
            GrabGeometry();
        }
    }
}
