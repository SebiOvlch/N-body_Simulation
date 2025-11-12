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
            => new Vector2(a.X - b.Y, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, float scalar)
        => new Vector2(a.X * scalar, a.Y * scalar);
        public static Vector2 operator /(Vector2 a, float scalar)
            => new Vector2(a.X / scalar, a.Y / scalar);
        #endregion

        #region Methods
        // Properties
        public float Length(Vector2 V)
        {
            return (float)Math.Sqrt(V.X *V.X + V.Y *V.Y);
        }

        public float LengthSquared(Vector2 V)
        {
            return (float)(V.X * V.X + V.Y * V.Y);
        }

        // Geometrical methods
        public float Dot(Vector2 a, Vector2 b)
        {
            return (float)(a.X * b.X + a.Y * b.Y);
        }

        public float Distance(Vector2 a, Vector2 b)
        {
            return Length(b - a);
        }
        #endregion

    }
}
