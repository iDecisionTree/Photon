using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public abstract class Component
    {
        public SceneObject sceneObject => _sceneObject;

        private SceneObject _sceneObject;

        protected Component()
        {
        }

        internal virtual void Init(SceneObject sceneObject)
        {
            _sceneObject = sceneObject;
        }
    }
}
