using Photon.Math;
using Photon.Math.Matrix;

namespace Photon.Core.Component
{
    public class Camera : Transform
    {
        public float fov
        {
            get
            {
                return _fov;
            }
            set
            {
                _fov = value;
                _isDirty = true;
            }
        }

        public float fovDegree
        {
            get
            {
                return Mathf.RadiansToDegree(_fov);
            }
            set
            {
                _fov = Mathf.DegreeToRadians(value);
                _isDirty = true;
            }
        }

        public float aspect
        {
            get
            {
                return _aspect;
            }
            set
            {
                _aspect = value;
            }
        }

        public float near
        {
            get
            {
                return _near;
            }
            set
            {
                _near = value;
                _isDirty = true;
            }
        }

        public float far
        {
            get
            {
                return _far;
            }
            set
            {
                _far = value;
                _isDirty = true;
            }
        }

        private float _fov;
        private float _aspect;
        private float _near;
        private float _far;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;
        private bool _isDirty;

        public Camera()
        {
            _fov = Mathf.PI / 3f;
            _near = 0.1f;
            _far = 1000f;
            _isDirty = true;
        }

        public Matrix4x4 viewMatrix => CalculateViewMatrix();
        public Matrix4x4 projectionMatrix => CalculateProjectionMatrix();

        public Matrix4x4 CalculateViewMatrix()
        {
            _viewMatrix = Matrix4x4.CreateLookAt(sceneObject!.transform.position, sceneObject.transform.forward, sceneObject.transform.right, sceneObject.transform.up);
            return _viewMatrix;
        }

        public Matrix4x4 CalculateProjectionMatrix()
        {
            if (_isDirty)
            {
                _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfView(fov, aspect, near, far);
                _isDirty = false;
            }

            return _projectionMatrix;
        }
    }
}
