using Photon.Core.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.Component
{
    public abstract class ComponentBase
    {
        public SceneObject? sceneObject { get; internal set; } = null;

        public virtual void Initialize()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update() 
        {
        }

        public virtual void Stop()
        {
        }
    }
}
