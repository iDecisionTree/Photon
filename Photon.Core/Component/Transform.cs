using Photon.Math;
using Photon.Math.Matrix;
using Photon.Math.Vector;

namespace Photon.Core.Component
{
    public class Transform : ComponentBase
    {
        public Vector3 position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                _isDirty = true;
            }
        }

        public Quaternion rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                _isDirty = true;
            }
        }

        public Vector3 scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                _isDirty = true;
            }
        }

        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;
        private Matrix4x4 _worldMatrix;
        private bool _isDirty;

        public Transform()
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.one;
        }

        public Matrix4x4 worldMatrix => CalculateWorldMatrix();
        public Vector3 forward => rotation * Vector3.unitZ;
        public Vector3 up => rotation * Vector3.unitY;
        public Vector3 right => rotation * Vector3.unitX;

        public Matrix4x4 CalculateWorldMatrix()
        {
            if (_isDirty)
            {
                _worldMatrix = Matrix4x4.CreateTRS(position, rotation, scale);
                _isDirty = false;
            }

            return _worldMatrix;
        }
    }
}
