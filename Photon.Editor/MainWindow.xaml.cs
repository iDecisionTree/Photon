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

            Mesh mesh = ObjImporter.Import("bunny.obj", false);

            SceneObject obj = new SceneObject("Camera");
            Camera camera = obj.AddComponent<Camera>();

            camera.sceneObject.transform.position = new Vector3(0, 0.5f, 1);
            camera.sceneObject.transform.Rotate(0, Mathf.Deg2Rad * 180, 0);

            Sphere sphere = new Sphere(Vector3.Zero, 0.5f);
            camera.Render(mesh);
            camera.film.Save("output.ppm");
        }
    }
}