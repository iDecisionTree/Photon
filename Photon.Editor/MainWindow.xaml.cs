using Photon.Core;
using System.Numerics;
using System.Windows;
using Plane = Photon.Core.Plane;

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

            camera.sceneObject.transform.position = new Vector3(0f, 0.1f, 1);
            camera.sceneObject.transform.Rotate(0, Mathf.Deg2Rad * 180, 0);

            Scene scene = new Scene();
            scene.AddHitableObject(new Plane());
            Sphere sphere = new Sphere();
            sphere.transform.scale = new Vector3(0.5f, 0.5f, 0.5f);
            sphere.transform.position = new Vector3(0f, 0.1f, 0f);
            scene.AddHitableObject(sphere);

            camera.Render(scene);
            camera.film.Save("output.ppm");
        }
    }
}