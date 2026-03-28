using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
