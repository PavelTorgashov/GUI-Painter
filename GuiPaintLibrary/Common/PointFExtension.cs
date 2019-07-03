using System;
using System.Drawing;

namespace GuiPaintLibrary.Common
{
    public static class PointFExtension
    {
        public const float PI = (float)Math.PI;
        public const float PI2 = 2 * PI;
        public const float TO_DEGREES = 180 / (float)Math.PI;
        public const float TO_RADIANS = (float)Math.PI / 180;

        /// <summary>
        /// Длина (модуль) вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>число</returns>
        public static float Length(this PointF vector)
        {
            return (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        /// <summary>
        /// Квадрат длины вектора
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>число</returns>
        public static float LengthSqr(this PointF vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        /// <summary>
        /// Скалярное произведение векторов
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>число - длина вектора, соединяющего концы умножаемых векторов</returns>
        public static float DotScalar(this PointF vector1, PointF vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y;
        }

        /// <summary>
        /// Сложение двух векторов
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>Вектор</returns>
        public static PointF Add(this PointF vector1, PointF vector2)
        {
            return new PointF(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        /// <summary>
        /// Вычитание двух векторов
        /// </summary>
        /// <param name="vector2"></param>
        /// <param name="vector1"></param>
        /// <returns>Вектор</returns>
        public static PointF Sub(this PointF vector1, PointF vector2)
        {
            return new PointF(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        /// <summary>
        /// Минимальный угол между векторами, в рад
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns>число - угол</returns>
        public static float Angle(this PointF vector1, PointF vector2)
        {
            var a = vector1.Angle() - vector2.Angle();
            a += (a > PI) ? -2 * PI : (a < -PI) ? 2 * PI : 0;

            return a;
        }

        /// <summary>
        /// Angle (-PI ; PI]
        /// </summary>
        public static float Angle(this PointF c)
        {
            return (float)Math.Atan2(c.Y, c.X);
        }

        /* not used
                /// <summary>
                /// Поворот вектора на угол
                /// </summary>
                /// <param name="vector"></param>
                /// <param name="angle">угол поворота</param>
                /// <returns></returns>
                public static PointF Rotate(this PointF vector, float angle)
                {
                    return new PointF((float) (vector.X*Math.Cos(angle) - vector.Y*Math.Sin(angle)),
                                      (float) (vector.X*Math.Sin(angle) + vector.Y*Math.Cos(angle)));
                }

                /// <summary>
                /// Строим вектор на координатах двух точек
                /// </summary>
                /// <param name="point2"></param>
                /// <param name="point1"></param>
                /// <returns>Вектор</returns>
                public static PointF Vector(this PointF point2, PointF point1)
                {
                    return new PointF(point2.X - point1.X, point2.Y - point1.Y);
                }
         * 
                /// <summary>
                /// Ортогональная проекция Вектора 1 на Вектор 2
                /// </summary>
                /// <param name="vector1">Вектор 1</param>
                /// <param name="vector2">Вектор 2</param>
                /// <returns>число - длина проекции</returns>
                public static float OrtoProjection(this PointF vector1, PointF vector2)
                {
                    return vector1.DotScalar(vector2)/vector2.Length();
                }

                /// <summary>
                /// Умножение вектора на скаляр
                /// </summary>
                /// <param name="vector"></param>
                /// <param name="value">множитель</param>
                /// <returns>Вектор</returns>
                public static PointF MulTo(this PointF vector, float value)
                {
                    return new PointF(vector.X * value, vector.Y * value);
                }

                /// <summary>
                /// Деление вектора на скаляр
                /// </summary>
                /// <param name="vector"></param>
                /// <param name="value">делитель</param>
                /// <returns>Вектор</returns>
                public static PointF DivTo(this PointF vector, float value)
                {
                    if (Math.Abs(value) < float.Epsilon) throw new DivideByZeroException();
                    return new PointF(vector.X / value, vector.Y / value);
                }

                /// <summary>
                /// Нахождение коеффициента масштабирования
                /// </summary>
                /// <param name="vector1">Вектор 1</param>
                /// <param name="vector2">Вектор 2</param>
                /// <returns>Вектор</returns>
                public static PointF ScaleKoeff(this PointF vector1, PointF vector2)
                {
                    if (vector2.Length() < float.Epsilon) return vector2.Clone().MulTo(float.Epsilon);
                    return new PointF(Math.Abs(vector1.X / vector2.X), Math.Abs(vector1.Y / vector2.Y));
                }

                /// <summary>
                /// Нормализация вектора
                /// </summary>
                /// <param name="vector"></param>
                /// <returns>Нормализованный вектор</returns>
                public static PointF Norm(this PointF vector)
                {
                    var length = vector.Length();
                    if (Math.Abs(length - 0f) < float.Epsilon) throw new DivideByZeroException();
                    return new PointF(vector.X / length, vector.Y / length);
                }

                public static PointF Clone(this PointF vector)
                {
                    return new PointF(vector.X, vector.Y);
                }
         */
    }
}
