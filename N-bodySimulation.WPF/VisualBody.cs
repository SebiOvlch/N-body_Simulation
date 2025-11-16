using N_bodyPhysics.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace N_bodySimulation.WPF
{
    public class VisualBody
    {
        public Body CoreBody { get; private set; }

        public Ellipse VisualElement { get; set; }
        public Polyline TrajectoryElement { get; set; }

        public VisualBody(Body coreBody)
        {
            CoreBody = coreBody;
        }

        /// <summary>
        /// Updates the visual position of the Ellipse based on the physics body's position.
        /// </summary>
        public void UpdateVisualPosition()
        {
            if (VisualElement != null)
            {
                Canvas.SetLeft(VisualElement, CoreBody.Position.X - CoreBody.Radius);
                Canvas.SetTop(VisualElement, CoreBody.Position.Y - CoreBody.Radius);
            }
            UpdateTrajectory();
        }

        private void UpdateTrajectory()
        {
            PointCollection points = new PointCollection();
            foreach (var vec in CoreBody.GetTrajectoryPoints()) 
            {
                points.Add(new System.Windows.Point(vec.X, vec.Y)); 
            }
            TrajectoryElement.Points = points;
        }
    }
}
