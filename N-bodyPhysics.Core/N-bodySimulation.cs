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
        private bool _firstStep = true;
        public readonly float G;
        public N_bodySimulation(float gravitationalConstant = 1e-4f)
        {
            bodies = new List<Body>();
            this.G = gravitationalConstant;
        }

        public List<Body> GetBodies
        {
            get
            {
                return this.bodies;
            }
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

            CalculateNetForces();
            if (_firstStep)
            {
                foreach (var body in bodies)
                {
                    body.PreviousPosition = body.Position
                        - body.Velocity * fixedDeltaTime
                        + body.Acceleration * (0.5f * fixedDeltaTime * fixedDeltaTime);
                }
                _firstStep = false;
            }

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

                    Vector2 forceVector = bodyA.calculate_force_from(bodyB, this.G);

                    Vector2 AccelerationContribution_A = forceVector / bodyA.Mass;
                    bodyA.Acceleration += AccelerationContribution_A;

                    Vector2 AccelerationContribution_B = forceVector / bodyB.Mass;
                    bodyB.Acceleration -= AccelerationContribution_B;
                }
            }
        }
    }
}
