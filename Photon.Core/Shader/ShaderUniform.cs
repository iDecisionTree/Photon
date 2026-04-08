using Photon.Math.Matrix;
using Photon.Math.Vector;
using System.Runtime.InteropServices;

namespace Photon.Core.Shader
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ShaderUniform
    {
        [FieldOffset(0)] public ShaderUniformType type;
        [FieldOffset(4)] public int intValue;
        [FieldOffset(4)] public bool boolValue;
        [FieldOffset(4)] public float floatValue;
        [FieldOffset(4)] public Vector2 vector2Value;
        [FieldOffset(4)] public Vector3 vector3Value;
        [FieldOffset(4)] public Vector4 vector4Value;
        [FieldOffset(4)] public Matrix4x4 matrix4x4Value;

        public ShaderUniform(int value)
        {
            type = ShaderUniformType.Int;
            intValue = value;
        }

        public ShaderUniform(bool value)
        {
            type = ShaderUniformType.Bool;
            boolValue = value;
        }

        public ShaderUniform(float value)
        {
            type = ShaderUniformType.Float;
            floatValue = value;
        }

        public ShaderUniform(Vector2 value)
        {
            type = ShaderUniformType.Vector2;
            vector2Value = value;
        }

        public ShaderUniform(Vector3 value)
        {
            type = ShaderUniformType.Vector3;
            vector3Value = value;
        }

        public ShaderUniform(Vector4 value)
        {
            type = ShaderUniformType.Vector4;
            vector4Value = value;
        }

        public ShaderUniform(Matrix4x4 value)
        {
            type = ShaderUniformType.Matrix4x4;
            matrix4x4Value = value;
        }
    }
}
