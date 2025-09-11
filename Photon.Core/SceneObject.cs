namespace Photon.Core
{
    public class SceneObject
    {
        public string name
        {
            get => _name;
            set => _name = value;
        }

        public Transform transform => _transform;

        private string _name;
        private Transform _transform;
        private List<Component> _components = new List<Component>();

        public SceneObject(string name = "New SceneObject")
        {
            _name = name;
            _transform = AddComponent<Transform>();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T? existingComponent = GetComponent<T>();
            if (existingComponent != null)
            {
                return existingComponent;
            }

            T component = new T();
            component.Init(this);
            _components.Add(component);

            return component;
        }

        public T? GetComponent<T>() where T : Component
        {
            return _components.OfType<T>().FirstOrDefault();
        }

        public void RemoveComponent<T>() where T : Component
        {
            T? component = GetComponent<T>();
            if (component != null)
            {
                _components.Remove(component);
            }
        }
    }
}
