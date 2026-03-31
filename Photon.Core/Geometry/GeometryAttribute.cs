using Photon.Core.Geometry;
using Photon.Math.Vector;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GeometryAttribute
{
    [FieldOffset(0)] public readonly GeometryAttributeType type;
    [FieldOffset(4)] public readonly float floatValue;
    [FieldOffset(4)] public readonly Vector2 vector2Value;
    [FieldOffset(4)] public readonly Vector3 vector3Value;
    [FieldOffset(4)] public readonly Vector4 vector4Value;

    public GeometryAttribute(float value)
    {
        type = GeometryAttributeType.Float;
        floatValue = value;
    }

    public GeometryAttribute(Vector2 value)
    {
        type = GeometryAttributeType.Vector2;
        vector2Value = value;
    }

    public GeometryAttribute(Vector3 value)
    {
        type = GeometryAttributeType.Vector3;
        vector3Value = value;
    }

    public GeometryAttribute(Vector4 value)
    {
        type = GeometryAttributeType.Vector4;
        vector4Value = value;
    }
}