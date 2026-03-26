using Photon.Core.Scene;

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
