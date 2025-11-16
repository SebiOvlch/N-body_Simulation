using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media;
using N_bodyPhysics.Core;

namespace N_bodySimulation.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private N_bodyPhysics.Core.N_bodySimulation _simulationEngine;
        private List<VisualBody> _visualBodies;

        private const float PHYSICS_SPEED_FACTOR = 2f;
        private const float FIXED_DELTA_TIME = 0.005f;
        private float _timeAccumulator = 0f;
        private DateTime _lastUpdateTime;

        public MainWindow()
        {
            InitializeComponent();

            _simulationEngine = new N_bodyPhysics.Core.N_bodySimulation(1e-4f);
            _visualBodies = new List<VisualBody>();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeBodies();
            _lastUpdateTime = DateTime.Now;
            CompositionTarget.Rendering += SimulationLoop;
        }

        private void SimulationLoop(object? sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float deltaTimeActual = (float)(now - _lastUpdateTime).TotalSeconds;
            _lastUpdateTime = now;

            _timeAccumulator += deltaTimeActual;

            while (_timeAccumulator >= FIXED_DELTA_TIME)
            {
                _simulationEngine.Step(FIXED_DELTA_TIME * PHYSICS_SPEED_FACTOR);
                _timeAccumulator -= FIXED_DELTA_TIME;

                foreach (var body in _simulationEngine.GetBodies)
                {
                    body.AddCurrentPositionToTrajectory();
                }
            }

            foreach (var vBody in _visualBodies)
            {
                vBody.UpdateVisualPosition();
            }
        }

        private void InitializeBodies()
        {
            float G_scaled = _simulationEngine.G; // G_scaled = 1e-4f
            float canvasWidth = (float)SimulationCanvas.ActualWidth;
            float canvasHeight = (float)SimulationCanvas.ActualHeight;
            Vector2 centerPos = new Vector2(canvasWidth / 2, canvasHeight / 2);

            // --- Központi Csillag (Sun) ---
            // Extrém nagy tömeg a stabilitáshoz
            const float sunMass = 60.0e8f; // Csökkentettük a korábbi 50e6-hoz képest!
            const float sunRadius = 25f;
            Body sun = new Body(centerPos, Vector2.Zero, sunMass, sunRadius, System.Drawing.Color.Yellow);
            AddBodyToSimulation(sun);

            // -------------------------------------------------------------
            // Segédmetódus a testek gyors beállításához
            // -------------------------------------------------------------
            Action<float, float, float, float, System.Drawing.Color, bool> AddPlanet =
                (mass, radius, orbitalDistance, speedFactor, color, orbitClockwise) =>
                {
                    float requiredSpeed = (float)Math.Sqrt((G_scaled * sunMass) / orbitalDistance);

                    Vector2 pos;
                    if (orbitClockwise)
                        pos = new Vector2(centerPos.X + orbitalDistance, centerPos.Y);
                    else
                        pos = new Vector2(centerPos.X - orbitalDistance, centerPos.Y);

                    // Stabil szorzó: 0.90f - 0.95f között
                    float finalSpeed = requiredSpeed * speedFactor;

                    Vector2 vel = orbitClockwise ? new Vector2(0, finalSpeed) : new Vector2(0, -finalSpeed);

                    Body planet = new Body(pos, vel, mass, radius, color);
                    AddBodyToSimulation(planet);
                };

            // --- 1. Belső Bolygó (Mercury) ---
            const float mercuryMass = 100f;
            const float mercuryRadius = 4f;
            const float mercuryOrbitalDistance = 100f;
            // Belső bolygók gyorsabban mozognak, alacsonyabb sebességfaktorral stabilabbak.
            AddPlanet(mercuryMass, mercuryRadius, mercuryOrbitalDistance, 0.90f, System.Drawing.Color.Gray, true);

            // --- 2. Belső Bolygó (Venus) ---
            const float venusMass = 250f;
            const float venusRadius = 6f;
            const float venusOrbitalDistance = 180f;
            // Pálya ellentétes irányban
            AddPlanet(venusMass, venusRadius, venusOrbitalDistance, 0.90f, System.Drawing.Color.Gold, false);

            // --- 3. Föld (Earth) ---
            const float earthMass = 500f;
            const float earthRadius = 8f;
            const float earthOrbitalDistance = 280f;
            // Vissza az eredeti irányba
            AddPlanet(earthMass, earthRadius, earthOrbitalDistance, 0.92f, System.Drawing.Color.DodgerBlue, true);

            // --- 4. Külső Bolygó (Mars) ---
            const float marsMass = 300f;
            const float marsRadius = 6f;
            const float marsOrbitalDistance = 400f;
            // Pálya ellentétes irányban
            AddPlanet(marsMass, marsRadius, marsOrbitalDistance, 0.92f, System.Drawing.Color.Red, false);
        }
        /// <summary>
        /// Creates the visual element, links it to the Body, and adds both to the simulation.
        /// </summary>
        private void AddBodyToSimulation(Body body)
        {
            Polyline trajectory = new Polyline
            {
                StrokeThickness = 1, // Vonal vastagsága
                                     // Áttetszővé tesszük a színt, hogy szebb legyen.
                Stroke = new SolidColorBrush(Color.FromArgb(150, body.color.R, body.color.G, body.color.B)),
                // Fontos: a vonalat a bolygó színe alapján hozzuk létre
            };

            Ellipse ellipse = new Ellipse
            {
                Width = body.Radius * 2,
                Height = body.Radius * 2,
                // Convert System.Drawing.Color to System.Windows.Media.Color
                Fill = new SolidColorBrush(Color.FromArgb(body.color.A, body.color.R, body.color.G, body.color.B))
            };

            // Create the linker object
            VisualBody vBody = new VisualBody(body)
            {
                VisualElement = ellipse,
                TrajectoryElement = trajectory
            };

            // Add to the WPF Canvas
            SimulationCanvas.Children.Add(trajectory);
            SimulationCanvas.Children.Add(ellipse);

            // Add to the WPF management list and the Core engine
            _visualBodies.Add(vBody);
            _simulationEngine.AddBody(body);
        }
    }
}