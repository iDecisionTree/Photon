using Photon.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class SceneObject
    {
        private string _name;
        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;

        public string name
        {
            get => _name;
            set => _name = value;
        }

        public Vector3 position
        {
            get => _position;
            set => _position = value;
        }

        public Quaternion rotation
        {
            get => _rotation;
            set => _rotation = value;
        }

        public Vector3 scale
        {
            get => _scale;
            set => _scale = value;
        }

        public Vector3 eulerAngle => _rotation.eulerAngle;

        public SceneObject()
        {
            _name = "Scene Object";
            _position = Vector3.zero;
            _rotation = Quaternion.identity;
            _scale = Vector3.one;
        }

        public SceneObject(string name, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            _name = name;
            _position = position;
            _rotation = rotation;
            _scale = scale; 
        }
    }
}
