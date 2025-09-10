using Photon.Core;
using System.Numerics;
using System.Windows;

namespace Photon.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SceneObject obj = new SceneObject("Camera");
            Camera camera = obj.AddComponent<Camera>();

            camera.sceneObject.transform.position = new Vector3(0, 0, 1);
            camera.sceneObject.transform.Rotate(Mathf.Deg2Rad * 180, 0, 0);

            Sphere sphere = new Sphere(Vector3.Zero, 0.5f);
            camera.Render(sphere);
            camera.film.Save("output.ppm");
        }
    }
}