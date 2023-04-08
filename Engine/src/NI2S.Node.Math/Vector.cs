using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Mathematics
{
    public struct Vector2
    {
        public static readonly Vector2 Zero = new(0.0, 0.0);
        public static readonly Vector2 Unit = new(1.0, 1.0);

        public double X;
        public double Y;

        public Vector2(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public Vector2(double size) : this() { X = Y = size; }
    }

    public struct Vector
    {
        public static readonly Vector Zero = new(0.0, 0.0, 0.0);
        public static readonly Vector Unit = new(1.0, 1.0, 1.0);

        public double X;
        public double Y;
        public double Z;

        public Vector(double x, double y, double z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(double size) : this() { X = Y = Z = size; }
    }

    public struct Vector4
    {
        public static readonly Vector4 Zero = new(0.0, 0.0, 0.0, 0.0);
        public static readonly Vector4 Unit = new(1.0, 1.0, 1.0, 1.0);

        public double X;
        public double Y;
        public double Z;
        public double W;

        public Vector4(double x, double y, double z, double w = 1.0) : this()
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(double size) : this() { X = Y = Z = W = size; }
    }
}
