namespace Photon.Core.Scene
{
    public class SceneManager
    {
        public List<SceneObject> sceneObjects { get; set; }

        public SceneManager()
        {
            sceneObjects = new List<SceneObject>();
        }
    }
}
