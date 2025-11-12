using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_bodyPhysics.Core
{
    public class Vector2
    {
        public float X {  get; set; }
        public float Y { get; set; }

        #region Constructors
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vector2()
        {
            X = 0;
            Y = 0;
        }
        #endregion

        #region Operators
        public static Vector2 Zero => new Vector2();
        public static Vector2 operator +(Vector2 a, Vector2 b)
            => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b)
            => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, float scalar)
        => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator /(Vector2 a, float scalar)
        {
            if (scalar == 0) throw new DivideByZeroException("Cannot divide a Vector2 by zero.");
            return new Vector2(a.X / scalar, a.Y / scalar);
        }
        #endregion

        #region Methods
        // Properties
        public float Length => (float)Math.Sqrt(X * X + Y * Y);
       

        public float LengthSquared => (float)(X * X + Y * Y);

        // Geometrical methods

        public Vector2 Normalize()
        {
            float length = Length;
            if (length == 0) return Zero;
            return this / length;
        }
        public static float Dot(Vector2 a, Vector2 b)
        {
            return (float)(a.X * b.X + a.Y * b.Y);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            Vector2 difference = a - b;
            return difference.Length;
        }

        public Vector2 Perpendicular() => new Vector2(-Y, X);


        #endregion

    }
}
