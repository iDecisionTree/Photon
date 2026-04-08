using Photon.Core.Geometry;
using Photon.Math.Vector;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public readonly struct GeometryProperty
{
    [FieldOffset(0)] public readonly GeometryPropertyType type;
    [FieldOffset(4)] public readonly float floatValue;
    [FieldOffset(4)] public readonly Vector2 vector2Value;
    [FieldOffset(4)] public readonly Vector3 vector3Value;
    [FieldOffset(4)] public readonly Vector4 vector4Value;

    public GeometryProperty(float value)
    {
        type = GeometryPropertyType.Float;
        floatValue = value;
    }

    public GeometryProperty(Vector2 value)
    {
        type = GeometryPropertyType.Vector2;
        vector2Value = value;
    }

    public GeometryProperty(Vector3 value)
    {
        type = GeometryPropertyType.Vector3;
        vector3Value = value;
    }

    public GeometryProperty(Vector4 value)
    {
        type = GeometryPropertyType.Vector4;
        vector4Value = value;
    }
}