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
        public Film film
        {
            get => _film;
            set
            {
                _film = value;
                UpdateProjectionMatrix();
            }
        }

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

        public float aspect => _film.width / (float)_film.height;

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

        public Matrix4x4 viewMatrix => _viewMatrix;
        public Matrix4x4 projectionMatrix => _projectionMatrix;

        private Film _film;
        private float _fov;
        private float _near;
        private float _far;
        private Matrix4x4 _viewMatrix;
        private Matrix4x4 _projectionMatrix;

        public Camera()
        {
            // 这里不从属性赋值是避免多次计算矩阵, 后面所有方法要从属性赋值
            _film = new Film(1920, 1080);
            _fov = 90f;
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

        public void SetResolution(int width, int height)
        {
            film = new Film(width, height);
        }

        public Ray GenerateRay(float u, float v)
        {
            Matrix4x4 invViewMatrix;
            Matrix4x4.Invert(_viewMatrix, out invViewMatrix);
            Matrix4x4 invProjectionMatrix;
            Matrix4x4.Invert(_projectionMatrix, out invProjectionMatrix);

            float ndcX = 2f * u - 1f;
            float ndcY = 1f - 2f * v;

            Vector4 ndc = new Vector4(ndcX, ndcY, 1f, 1f);
            Vector4 view = Vector4.Transform(ndc, invProjectionMatrix);
            view /= view.W;
            Vector4 world = Vector4.Transform(view, invViewMatrix);

            Vector3 direction = new Vector3(world.X, world.Y, world.Z) - sceneObject.transform.position;

            return new Ray(sceneObject.transform.position, Vector3.Normalize(direction));
        }

        public void Render(Sphere s)
        {
            Vector3 light = new Vector3(1f, 1f, 1f);

            Parallel.For(0, _film.height, y =>
            {
                for (int x = 0; x < _film.width; x++)
                {
                    float u = (x + 0.5f) / _film.width;
                    float v = (y + 0.5f) / _film.height;

                    Ray ray = GenerateRay(u, v);
                    HitInfo hitInfo;
                    if (s.Intersect(ray, out hitInfo))
                    {
                        Vector3 l = Vector3.Normalize(light - hitInfo.point);
                        float cos = Mathf.Max(0f, Vector3.Dot(hitInfo.normal, l));
                        _film.SetPixel(x, y, new Color(cos));
                    }
                }
            });
        }

        private void UpdateViewMatrix()
        {
            _viewMatrix = Matrix4x4.CreateLookToLeftHanded(sceneObject.transform.position, sceneObject.transform.forward, sceneObject.transform.up);
        }

        private void UpdateProjectionMatrix()
        {
            _projectionMatrix = Matrix4x4.CreatePerspectiveFieldOfViewLeftHanded(fovRadians, aspect, _near, _far);
        }
    }
}
