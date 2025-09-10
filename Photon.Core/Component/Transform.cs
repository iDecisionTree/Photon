using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class Transform : Component
    {
        public Vector3 position
        {
            get => _position;
            set
            {
                _position = value;
                UpdateTranslationMatrix();
                UpdateModelMatrix();
                onTransformChanged?.Invoke();
            }
        }

        public Quaternion rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                UpdateRotationMatrix();
                UpdateModelMatrix();
                onTransformChanged?.Invoke();
            }
        }

        public Vector3 scale
        {
            get => _scale;
            set
            {
                _scale = value;
                UpdateScaleMatrix();
                UpdateModelMatrix();
                onTransformChanged?.Invoke();
            }
        }

        public Action onTransformChanged;

        public Vector3 forward => Vector3.Transform(Vector3.UnitZ, _rotation);
        public Vector3 back => Vector3.Transform(-Vector3.UnitZ, _rotation); 
        public Vector3 right => Vector3.Transform(Vector3.UnitX, _rotation);
        public Vector3 left => Vector3.Transform(-Vector3.UnitX, _rotation);
        public Vector3 up => Vector3.Transform(Vector3.UnitY, _rotation);
        public Vector3 down => Vector3.Transform(-Vector3.UnitY, _rotation);

        public Matrix4x4 translationMatrix => _translationMatrix;
        public Matrix4x4 rotationMatrix => _rotationMatrix;
        public Matrix4x4 scaleMatrix => _scaleMatrix;
        public Matrix4x4 modelMatrix => _modelMatrix;

        private Vector3 _position;
        private Quaternion _rotation;
        private Vector3 _scale;
        private Matrix4x4 _translationMatrix;
        private Matrix4x4 _rotationMatrix;
        private Matrix4x4 _scaleMatrix;
        private Matrix4x4 _modelMatrix;

        public Transform()
        {
            // 这里不从属性赋值是避免多次计算矩阵, 后面所有方法要从属性赋值
            _position = Vector3.Zero;
            _rotation = Quaternion.Identity;
            _scale = Vector3.One;
        }

        internal override void Init(SceneObject sceneObject)
        {
            base.Init(sceneObject);

            UpdateTranslationMatrix();
            UpdateRotationMatrix();
            UpdateScaleMatrix();
            UpdateModelMatrix();
        }

        public void Translate(Vector3 translation)
        {
            position += translation;
        }

        public void Rotate(Quaternion rotation)
        {
            this.rotation = Quaternion.Normalize(Quaternion.Concatenate(this.rotation, rotation));
        }

        public void Rotate(float pitch, float yaw, float rool)
        {
            Quaternion rotation = Quaternion.CreateFromYawPitchRoll(yaw, pitch, rool);
            Rotate(rotation);
        }

        private void UpdateTranslationMatrix()
        {
            _translationMatrix = Matrix4x4.CreateTranslation(_position);
        }

        private void UpdateRotationMatrix()
        {
            _rotationMatrix = Matrix4x4.CreateFromQuaternion(_rotation);
        }

        private void UpdateScaleMatrix()
        {
            _scaleMatrix = Matrix4x4.CreateScale(_scale);
        }

        private void UpdateModelMatrix()
        {
            _modelMatrix = _scaleMatrix * _rotationMatrix * _translationMatrix;
        }
    }
}
