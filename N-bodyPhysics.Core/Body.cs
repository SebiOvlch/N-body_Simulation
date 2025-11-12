using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_bodyPhysics.Core
{
    public class Body
    {
        // State Variables
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration {  get; set; }

        // Properties
        public float Mass { get; set; }
        public float Radius { get; set; }

        // For  the drawing
        public Color color { get; set; }

        #region Constructors
        public Body(Vector2 pos, Vector2 vel, float mass, float radius, Color col)
        {
            Position = pos;
            Velocity = vel;
            Mass = mass;
            Radius = radius;
            color = col;
            Acceleration = Vector2.Zero;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates the gravitational force exerted by the input body to the current body
        /// </summary>
        /// <param name="b">Body b</param>
        /// <param name="G">Gravitational constant</param>
        /// <returns></returns>
        public Vector2 calculate_force_from(Body b, float G)
        {
            Body a = this;
            Vector2 r_ij_vector = b.Position - a.Position;
            float distanceSquared = r_ij_vector.LengthSquared;
            if (distanceSquared < 1e-6)
                return new Vector2(0, 0);

            float distance = r_ij_vector.Length;
            float ForceMagnitude = G * (a.Mass * b.Mass) / distanceSquared;

            Vector2 unitVector = r_ij_vector / distance;
            Vector2 forceVector = unitVector * ForceMagnitude;

            return forceVector;
        }

        public void Update(float deltaTime)
        {
            if(Mass == 0) return;

            Velocity += Acceleration * deltaTime;

            Position += Velocity * deltaTime;
        }

        public void ResetAcceleration()
        {
            Acceleration = Vector2.Zero;
        }
        #endregion
    }
}
