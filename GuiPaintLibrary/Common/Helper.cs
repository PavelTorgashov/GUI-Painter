using GuiPaintLibrary.Figures;
using GuiPaintLibrary.Renderers;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace GuiPaintLibrary.Common
{
    public static class Helper
    {
        public static float GetAngle(Matrix matrix)
        {
            var x = new Vector(1, 0);
            var winMatrix = CreateWindowsMatrix(matrix);
            var rotated = Vector.Multiply(x, winMatrix);
            var angleBetween = Vector.AngleBetween(x, rotated);
            return (float)angleBetween;
        }

        public static SizeF GetSize(Matrix matrix)
        {
            var x = new Vector(1, 0);
            var y = new Vector(0, 1);
            var winMatrix = CreateWindowsMatrix(matrix);
            var scaledX = Vector.Multiply(x, winMatrix);
            var scaledY = Vector.Multiply(y, winMatrix);
            return new SizeF((float)scaledX.Length, (float)scaledY.Length);
        }

        public static float GetSkewAngle(Matrix matrix)
        {
            var x = new Vector(1, 0);
            var y = new Vector(0, 1);
            var winMatrix = CreateWindowsMatrix(matrix);
            var skewX = Vector.Multiply(x, winMatrix);
            var skewY = Vector.Multiply(y, winMatrix);
            var angleBetween = Vector.AngleBetween(skewX, skewY);
            return (float)angleBetween;
        }

        /// <summary>
        /// Преобразование ContentAlignment в StringFormat.Alignment и StringFormat.LineAlignment
        /// </summary>
        /// <param name="stringFormat">Ссылка на объект StringFormat</param>
        /// <param name="alignment">Значения для установки</param>
        public static void UpdateStringFormat(StringFormat stringFormat, ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }
        }

        // ReSharper disable InconsistentNaming
        public const float EPSILON = 0.01f;

        public static object FillDecorator { get; private set; }

        // ReSharper restore InconsistentNaming

        /// <summary>
        /// Расчёт коэффициента масштабирования
        /// </summary>
        /// <param name="marker">Точка маркера</param>
        /// <param name="anchor">Точка якоря</param>
        /// <param name="mouse">Положение мышки</param>
        /// <returns></returns>
        public static float GetScale(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor); // строим вектор Anchor-Marker
            var m = mouse.Sub(anchor);  // строим вектор Anchor-Mouse(position)
            // считаем коэффициент
            var scale = m.DotScalar(a) / a.LengthSqr();
            // защита результата от "крайних" случаев расчёта
            if (System.Math.Abs(scale) < EPSILON) scale = EPSILON;
            return scale;
        }

        /// <summary>
        /// Расчет угла вращения (в градусах)
        /// </summary>
        /// <param name="marker">Точка маркера</param>
        /// <param name="anchor">Точка якоря</param>
        /// <param name="mouse">Положение мышки</param>
        /// <returns></returns>
        public static float GetAngle(PointF marker, PointF anchor, PointF mouse)
        {
            var a = marker.Sub(anchor);
            var m = mouse.Sub(anchor);
            return m.Angle(a) * PointFExtension.TO_DEGREES;
        }

        /// <summary>
        /// Сохранить все фигуры в поток
        /// </summary>
        /// <param name="stream">поток в памяти</param>
        /// <param name="obj"></param>
        public static void SaveToStream(Stream stream, object obj)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
        }

        /// <summary>
        /// Восстановить фигуры из потока в памяти
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static object LoadFromStream(Stream stream)
        {
            var formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        private static System.Windows.Media.Matrix CreateWindowsMatrix(Matrix matrix)
        {
            return new System.Windows.Media.Matrix(matrix.Elements[0], matrix.Elements[1], matrix.Elements[2],
                                                       matrix.Elements[3], matrix.Elements[4], matrix.Elements[5]);
        }

        public static void CutLastBezierPoints(ref PointF[] points, ref byte[] types)
        {
            if (points.Length == types.Length && types.Length >= 4)
            {
                points = new List<PointF>(points.Take(points.Length - 3)).ToArray();
                types = new List<byte>(types.Take(types.Length - 3)).ToArray();
            }
        }

        public static bool ContainsAnyDecorator(Figure figure)
        {
            var result = false;
            result |= RendererDecorator.ContainsAnyDecorator(figure.Renderer);
            result |= GuiPaintLibrary.Styles.FillDecorator.ContainsAnyDecorator(figure.Style.FillStyle);
            return result;
        }
    }
}
