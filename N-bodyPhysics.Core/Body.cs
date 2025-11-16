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
        public Vector2 PreviousPosition { get; set; }
        private float min_distance_squared = .1f;
        // State Variables
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration {  get; set; }

        // Properties
        public float Mass { get; set; }
        public float Radius { get; set; }

        // For  the drawing
        public Color color { get; set; }
        private Queue<Vector2> _trajectoryPoints;
        private const int MAX_TRAJECTORY_POINTS = 700;

        #region Constructors
        public Body(Vector2 pos, Vector2 vel, float mass, float radius, Color col)
        {
            Position = pos;
            Velocity = vel;
            Mass = mass;
            Radius = radius;
            color = col;
            Acceleration = Vector2.Zero;
            PreviousPosition = pos;
            _trajectoryPoints = new Queue<Vector2>();

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
            if (distanceSquared < min_distance_squared)
            {
                distanceSquared = min_distance_squared;
            }

            float distance = r_ij_vector.Length;
            float ForceMagnitude = G * (a.Mass * b.Mass) / distanceSquared;

            Vector2 unitVector = r_ij_vector.Normalize();
            Vector2 forceVector = unitVector * ForceMagnitude;

            return forceVector;
        }

        public void Update(float deltaTime)
        {
            if(Mass == 0) return;

            Vector2 currentPosition = Position;

            Vector2 accelerationTerm = Acceleration * (deltaTime * deltaTime);

            Vector2 newPosition = (Position * 2f) - PreviousPosition + accelerationTerm;

            PreviousPosition = currentPosition;

            Position = newPosition;

            Velocity = (newPosition - PreviousPosition) / (2f * deltaTime);

            Acceleration = Vector2.Zero;
        }

        public void ResetAcceleration()
        {
            Acceleration = Vector2.Zero;
        }

        public void AddCurrentPositionToTrajectory()
        {
            _trajectoryPoints.Enqueue(Position);

            if (_trajectoryPoints.Count > MAX_TRAJECTORY_POINTS)
            {
                _trajectoryPoints.Dequeue();
            }
        }

        public IEnumerable<Vector2> GetTrajectoryPoints()
        {
            return _trajectoryPoints;
        }
        #endregion
    }
}
