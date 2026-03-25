using Photon.Core.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.Scene
{
    public class SceneObject
    {
        public string name { get; set; }
        public Transform transform { get; private set; }

        private readonly Dictionary<Type, ComponentBase> _components;

        public SceneObject(string name)
        {
            this.name = name;
            _components = new Dictionary<Type, ComponentBase>();
            transform = AddComponent<Transform>();
        }

        public T AddComponent<T>() where T : ComponentBase, new()
        {
            Type type = typeof(T);
            if (!_components.ContainsKey(type))
            {
                T component = new T();
                component.sceneObject = this;
                _components.Add(type, component);
                component.Initialize();
                
                return component;
            }

            return (T)_components[type];
        }

        public T? GetComponent<T>() where T : ComponentBase
        {
            Type type = typeof(T);
            _components.TryGetValue(type, out ComponentBase? component);
            return (T?)component;
        }

        public void RemoveComponent<T>() where T : ComponentBase
        {
            Type type = typeof(T);
            if (_components.ContainsKey(type))
            {
                _components[type].Stop();
                _components.Remove(type);
            }
        }

        public void Start()
        {
            foreach (ComponentBase component in _components.Values.ToList())
            {
                component.Start();
            }
        }

        public void Update()
        {
            foreach (ComponentBase component in _components.Values.ToList())
            {
                component.Update();
            }
        }

        public void Stop()
        {
            foreach (ComponentBase component in _components.Values.ToList())
            {
                component.Stop();
            }
        }
    }
}
