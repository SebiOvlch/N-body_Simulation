using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_bodyPhysics.Core
{
    public class N_bodySimulation
    {
        private List<Body> bodies;
        private const float Gravitational_constant = 6.674e-11f;

        public N_bodySimulation()
        {
            bodies = new List<Body>();
        }

        public void AddBody(Body body)
        {
            bodies.Add(body);
        }

        public void RemoveBody(Body body)
        {
            bodies.Remove(body);
        }

        public void Step(float fixedDeltaTime)
        {
            foreach (var body in bodies)
                body.Acceleration = Vector2.Zero;

            CalculateNetForces();

            foreach (var body in bodies)
                body.Update(fixedDeltaTime);
        }

        private void CalculateNetForces()
        {
            for(int i = 0; i < bodies.Count; i++)
            {
                Body bodyA = bodies[i];
                for(int j = i + 1; j < bodies.Count; j++)
                {
                    Body bodyB = bodies[j];

                    Vector2 forceVector = bodyA.calculate_force_from(bodyB, Gravitational_constant);

                    Vector2 AccelerationContribution_A = forceVector / bodyA.Mass;
                    bodyA.Acceleration += AccelerationContribution_A;

                    Vector2 AccelerationContribution_B = forceVector / bodyB.Mass;
                    bodyB.Acceleration -= AccelerationContribution_B;
                }
            }
        }
    }
}
