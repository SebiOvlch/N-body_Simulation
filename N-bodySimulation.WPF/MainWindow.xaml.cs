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

        private const float FIXED_DELTA_TIME = 0.0001f; // 60 fps
        private float _timeAccumulator = 0f;
        private DateTime _lastUpdateTime;

        public MainWindow()
        {
            InitializeComponent();

            _simulationEngine = new N_bodyPhysics.Core.N_bodySimulation();
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
                _simulationEngine.Step(FIXED_DELTA_TIME);
                _timeAccumulator -= FIXED_DELTA_TIME;
            }

            foreach (var vBody in _visualBodies)
            {
                vBody.UpdateVisualPosition();
            }
        }

        private void InitializeBodies()
        {
            float G_scaled = 6.674f;

            float canvasWidth = (float)SimulationCanvas.ActualWidth;
            float canvasHeight = (float)SimulationCanvas.ActualHeight;
            Vector2 centerPos = new Vector2(canvasWidth / 2, canvasHeight / 2);

            // --- 1. Csillag (Központi Test) ---
            float starMass = 55e5f; // Még nagyobb tömeg a stabilabb pályákhoz
            float starRadius = 40f; ;

            Body star = new Body(
                centerPos,
                Vector2.Zero,
                starMass,
                starRadius,
                System.Drawing.Color.Yellow);

            AddBodyToSimulation(star);

            // --- 2. Bolygók (P1 - P8) ---
            Random rand = new Random();

            // Kezdő paraméterek a ciklushoz
            float currentDistance = 60f; // Első bolygó távolsága
            float distanceIncrement = 30f; // Távolság növekedés a bolygók között
            float baseMass = 100f;

            for (int i = 0; i < 15; i++)
            {
                // 1. Tömeg és sugar skálázása
                float mass = baseMass * (1 + (i * 0.5f)); // Növekvő tömeg
                float radius = 5f + (i * 1.5f); // Növekvő sugar

                // 2. Távolság beállítása
                float orbitalDistance = currentDistance + (i * distanceIncrement);

                // 3. Szükséges sebesség számítása a körpályához (v = sqrt(G*M/r))
                float requiredSpeed = (float)Math.Sqrt((G_scaled * starMass) / orbitalDistance);

                // 4. Pozíció beállítása (X-tengelyen, jobbra)
                Vector2 planetPos = new Vector2(centerPos.X + orbitalDistance, centerPos.Y);

                // 5. Sebesség beállítása (Y-tengelyen, a pályához merőlegesen)
                // A szorzót (pl. 1.2f) használjuk, hogy elliptikus pályát hozzunk létre (ne körpálya legyen).
                Vector2 planetVel = new Vector2(0, requiredSpeed * 1.2f);

                // 6. Szín beállítása (random)
                System.Drawing.Color planetColor = System.Drawing.Color.FromArgb(
                    255, rand.Next(50, 255), rand.Next(50, 255), rand.Next(50, 255));

                Body planet = new Body(
                    planetPos,
                    planetVel,
                    mass,
                    radius,
                    planetColor);

                AddBodyToSimulation(planet);
            }
        }
        /// <summary>
        /// Creates the visual element, links it to the Body, and adds both to the simulation.
        /// </summary>
        private void AddBodyToSimulation(Body body)
        {
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
                VisualElement = ellipse
            };

            // Add to the WPF Canvas
            SimulationCanvas.Children.Add(ellipse);

            // Add to the WPF management list and the Core engine
            _visualBodies.Add(vBody);
            _simulationEngine.AddBody(body);
        }
    }
}