using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace System.Drawing
{
    public static partial class GraphicsHelper
    {
        static Pen pen = new System.Drawing.Pen(Color.Bisque);
        static SolidBrush brush = new System.Drawing.SolidBrush(Color.Bisque);

        public static Pen Pen(this Color color, float width = 1f)
        {
            pen.Color = color;
            pen.Width = width;
            return pen;
        }

        public static SolidBrush Brush(this Color color)
        {
            brush.Color = color;
            return brush;
        }

        public static Color Light(this Color color, int channelDelta = 20, float alphaMultiplier = 1)
        {
            return GetColor(color.A * alphaMultiplier, color.R + channelDelta, color.G + channelDelta, color.B + channelDelta);
        }

        public static Color Lerp(Color color1, Color color2, float k)
        {
            var kk = 1 - k;
            return GetColor(color1.A * kk + color2.A * k, color1.R * kk + color2.R * k, color1.G * kk + color2.G * k, color1.B * kk + color2.B * k);
        }

        public static int Saturate(this int val, int min = 0, int max = 255)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }

        public static Color GetColor(float a, float r, float g, float b)
        {
            var A = ((int)Math.Round(a)).Saturate();
            var R = ((int)Math.Round(r)).Saturate();
            var G = ((int)Math.Round(g)).Saturate();
            var B = ((int)Math.Round(b)).Saturate();
            return Color.FromArgb(A, R, G, B);
        }
    }
}
