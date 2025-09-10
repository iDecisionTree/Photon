using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class Camera : Component
    {
        public float fov
        {
            get => _fov;
            set
            {
                _fov = value;
                UpdateProjectionMatrix();
            }
        }

        public float fovRadians => _fov * Mathf.Deg2Rad;

        public float aspect
        {
            get => _aspect;
            set
            {
                _aspect = value;
                UpdateProjectionMatrix();
            }
        }

        public float near
        {
            get => _near;
            set
            {
                _near = value;
                UpdateProjectionMatrix();
            }
        }

        public float far
        {
            get => _far;
            set
            {
                _far = value;
                UpdateProjectionMatrix();
            }
        }

        private float _fov;
        private float _aspect;
        private float _near;
        private float _far;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;

        public Camera()
        {
            // 这里不从属性赋值是避免多次计算矩阵, 后面所有方法要从属性赋值
            _fov = 90f;
            _aspect = 16f / 9f;
            _near = 0.3f;
            _far = 1000f;
        }

        internal override void Init(SceneObject sceneObject)
        {
            base.Init(sceneObject);

            sceneObject.transform.onTransformChanged += UpdateViewMatrix;
            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }

        private void UpdateViewMatrix()
        {
            _viewMatrix = Matrix4x4.CreateLookAtLeftHanded(sceneObject.transform.position, sceneObject.transform.forward, sceneObject.transform.up);
        }

        private void UpdateProjectionMatrix()
        {
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfViewLeftHanded(fovRadians, _aspect, _near, _far);
        }
    }
}
