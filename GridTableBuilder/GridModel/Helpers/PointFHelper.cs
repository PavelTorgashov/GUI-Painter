using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using PointF = System.Drawing.PointF;

namespace System
{
    /// <summary>
    /// Coordinate manipulations
    /// </summary>
    public static class PointFHelper
    {
        public const float PI = 3.14159f;

        public static PointF InitFromAngle(this PointF c, float r, float angle)
        {
            var res = new PointF((float)(r * Math.Cos(angle)), (float)(r * Math.Sin(angle)));
            return res;
        }

        public static PointF Inverse(this PointF c)
        {
            var res = new PointF(-c.X, -c.Y);
            return res;
        }

        /// <summary>
        /// Complex conjugate (комплексно-сопряженный вектор)
        /// </summary>
        public static PointF Conjugate(this PointF c)
        {
            var res = new PointF(c.X, -c.Y);
            return res;
        }

        /// <summary>
        /// Determine which side of the line the 2D point is at
        /// </summary>
        /// <returns>
        /// 1 if on the right hand side;
        /// 0 if on the line;
        /// -1 if on the left hand side;
        /// </returns>
        public static float Side(this PointF c, PointF point)
        {
            var res = c.X * point.Y - point.X * c.Y;
            return res > float.Epsilon ? 1 :
                   res < -float.Epsilon ? -1 : 0;
        }

        /// <summary>
        /// Normal from the line to the point (the line is defined by normalized direction)
        /// </summary>
        public static PointF Normal2(this PointF normalizedDirection, PointF point)
        {
            var cosAngle = point.X * normalizedDirection.X + point.Y * normalizedDirection.Y;
            var x = normalizedDirection.X * cosAngle;
            var y = normalizedDirection.Y * cosAngle;
            return new PointF(point.X - x, point.Y - y);
        }

        /// <summary>
        /// Normal from the line to the point
        /// </summary>
        public static PointF Normal(this PointF direction, PointF point)
        {
            var cosAngle = (point.X * direction.X + point.Y * direction.Y) / direction.LengthSquare();
            var x = direction.X * cosAngle;
            var y = direction.Y * cosAngle;
            return new PointF(point.X - x, point.Y - y);
        }

        /// <summary>
        /// Distance to the point
        /// </summary>
        public static float DistanceTo(this PointF point1, PointF point2)
        {
            var dx = point2.X - point1.X;
            var dy = point2.Y - point1.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Angle from (-PI ; PI]
        /// </summary>
        public static float Angle(this PointF c)
        {
            return (float)Math.Atan2(c.Y, c.X);
        }

        /// <summary>
        /// Angle from [0 ; PI)
        /// </summary>
        public static float AngleBy2(this PointF c)
        {
            var a = Math.Atan2(c.Y, c.X);
            return (float)(a < 0 ? a + PI : (a >= PI ? PI - float.Epsilon : a));
        }

        /// <summary>
        /// Minimal degree between vectors
        /// </summary>
        public static float Angle(this PointF c1, PointF c2)
        {
            var a = c1.Angle() - c2.Angle();
            a += (float)((a > PI) ? -2 * PI : (a < -PI) ? 2 * PI : 0);

            return a;
        }

        public static float Length(this PointF c)
        {
            var x = c.X;
            var y = c.Y;
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static float LengthSquare(this PointF c)
        {
            var x = c.X;
            var y = c.Y;
            return x * x + y * y;
        }

        public static float LengthManhattan(this PointF c)
        {
            return Math.Abs(c.X) + Math.Abs(c.Y);
        }

        public static PointF Normalized(this PointF c)
        {
            var l = c.Length();
            return new PointF(c.X / l, c.Y / l);
        }

        public static PointF Add(this PointF c1, PointF c2)
        {
            return new PointF(c1.X + c2.X, c1.Y + c2.Y);
        }

        public static PointF Sub(this PointF c1, PointF c2)
        {
            return new PointF(c1.X - c2.X, c1.Y - c2.Y);
        }

        public static PointF Mul(this PointF c, float k)
        {
            return new PointF(c.X * k, c.Y * k);
        }

        /// <summary>
        /// Dot product (скалярное произведение)
        /// </summary>
        public static float DotScalar(this PointF x1, PointF x2)
        {
            return x1.X * x2.X + x1.Y * x2.Y;
        }

        /// <summary>
        /// Dot product for complex numbers (скалярное произведение комплексных чисел)
        /// </summary>
        public static PointF DotComplex(this PointF x1, PointF x2)
        {
            return new PointF(x1.X * x2.X - x1.Y * x2.Y, x1.Y * x2.X + x1.X * x2.Y);
        }

        /// <summary>
        /// Norma of cross product (норма векторного произведения, площадь натянутого паралелограмма)
        /// </summary>
        public static float DotVectorLength(this PointF x1, PointF x2)
        {
            return Math.Abs(x1.X * x2.Y - x1.Y * x2.X);
        }

        /// <summary>
        /// Projection on direction
        /// </summary>
        public static PointF Projection(this PointF x, PointF normalizedDirection)
        {
            var cosAngle = x.X * normalizedDirection.X + x.Y * normalizedDirection.Y;
            return new PointF(normalizedDirection.X * cosAngle, normalizedDirection.Y * cosAngle);
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        /// <param name="k">From 0 to 1</param>
        /// <returns></returns>
        public static PointF Lerp(this PointF x1, PointF x2, float k)
        {
            var m = 1.0f - k;
            return new PointF(x1.X * m + x2.X * k, x1.Y * m + x2.Y * k);
        }

        /// <summary>
        /// Linear interpolation
        /// </summary>
        public static PointF LerpTrim(this PointF x1, PointF x2, float k)
        {
            if (k < 0) k = 0f;
            if (k > 1) k = 1f;
            return Lerp(x1, x2, k);
        }

        public static float CosAngle(this PointF c)
        {
            return c.X / c.Length();
        }

        /// <summary>
        /// Cos of angle betwwen this and other vector
        /// </summary>
        public static float CosAngle(this PointF x1, PointF x2)
        {
            return (x1.X * x2.X + x1.Y * x2.Y) / x1.Length() / x2.Length();
        }

        public static PointF Rotate(this PointF c, float cosAngle, float sinAngle)
        {
            return new PointF(cosAngle * c.X - sinAngle * c.Y, sinAngle * c.X + cosAngle * c.Y);
        }

        public static PointF Rotate90(this PointF c)
        {
            return new PointF(-c.Y, c.X);
        }

        public static PointF Rotate(this PointF c, float angle)
        {
            var cosAngle = Math.Cos(angle);
            var sinAngle = Math.Sin(angle);
            return new PointF((float)(cosAngle * c.X - sinAngle * c.Y), (float)(sinAngle * c.X + cosAngle * c.Y));
        }

        public static PointF Rotate(this PointF c, float angle, PointF center)
        {
            var cosAngle = Math.Cos(angle);
            var sinAngle = Math.Sin(angle);

            var xx = c.X - center.X;
            var yy = c.Y - center.Y;

            return new PointF((float)(cosAngle * xx - sinAngle * yy + center.X), (float)(sinAngle * xx + cosAngle * yy + center.Y));
        }

        public static System.Drawing.Point ToPoint(this PointF c)
        {
            return new System.Drawing.Point((int)Math.Round(c.X), (int)Math.Round(c.Y));
        }

        public static string ToString(this PointF c)
        {
            return string.Format("{0:0.00};{1:0.00}", c.X, c.Y);
        }

        public static PointF ToPointF(this System.Drawing.Point p)
        {
            return new PointF(p.X, p.Y);
        }

        // Find the point of intersection between
        // the lines p1 --> p2 and p3 --> p4.
        // Находит пересечение отрезков, если точно известно, что они пересекаются
        public static PointF SegmentIntersection(PointF p1, PointF p2, PointF p3, PointF p4)
        {
            // Get the segments' parameters.
            float dx12 = p2.X - p1.X;
            float dy12 = p2.Y - p1.Y;
            float dx34 = p4.X - p3.X;
            float dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            float denominator = (dy12 * dx34 - dx12 * dy34);

            float t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
            float t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            // Find the point of intersection.
            return new PointF(p1.X + dx12 * t1, p1.Y + dy12 * t1);
        }

        /// <summary>
        /// Точка пересечения между двумя примыми, заднными направляющей и точкой
        /// </summary>
        public static PointF Intersection(this PointF dir1, PointF point1, PointF dir2, PointF point2)
        {
            var x2 = point1.X;
            var y2 = point1.Y;
            var x1 = point1.X + dir1.X;
            var y1 = point1.Y + dir1.Y;

            var a2 = point2.X;
            var b2 = point2.Y;
            var a1 = point2.X + dir2.X;
            var b1 = point2.Y + dir2.Y;

            var c = (b2 - b1) / (a2 - a1 + 0.00001f);
            var z = (y2 - y1) / (x2 - x1 + 0.00001f);

            var x = (c * x2 - y2 - z * a2 + b2) / (c - z);
            var y = c * x - c * x2 + y2;

            return new PointF(x, y);
        }

        public static PointF Intersection2(this PointF dir1, PointF point1, PointF dir2, PointF point2)
        {
            var x0 = point1.X;
            var p = dir1.X;
            var y0 = point1.Y;
            var q = dir1.Y;

            var x1 = point2.X;
            var p1 = dir2.X;
            var y1 = point2.Y;
            var q1 = dir2.Y;

            var x = (x0 * q * p1 - x1 * q1 * p - y0 * p * p1 + y1 * p * p1) / (q * p1 - q1 * p);
            var y = (y0 * p * q1 - y1 * p1 * q - x0 * q * q1 + x1 * q * q1) / (p * q1 - p1 * q);

            return new PointF(x, y);
        }

        /// <summary>
        /// This is essentially the same as Lerp but instead the function will ensure that the speed never exceeds maxDistanceDelta. 
        /// Negative values of maxDistanceDelta pushes the vector away from target.
        /// </summary>
        public static PointF MoveTowards(this PointF current, PointF target, float maxDistanceDelta)
        {
            var dir = target.Sub(current);
            var magnitude = dir.Length();
            if (magnitude <= maxDistanceDelta || magnitude <= float.Epsilon)
            {
                return target;
            }
            return current.Add(dir.Mul(maxDistanceDelta / magnitude));
        }


        /// <summary>
        /// Distance square to the point
        /// </summary>
        public static float DistanceSquareTo(this PointF point1, PointF point2)
        {
            var dx = point2.X - point1.X;
            var dy = point2.Y - point1.Y;
            return dx * dx + dy * dy;
        }
    }
}
