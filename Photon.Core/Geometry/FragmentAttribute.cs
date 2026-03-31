using Photon.Core.Geometry;
using Photon.Math.Vector;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
public readonly struct FragmentAttribute
{
    [FieldOffset(0)] public readonly FragmentAttributeType type;
    [FieldOffset(4)] public readonly float floatValue;
    [FieldOffset(4)] public readonly Vector2 vector2Value;
    [FieldOffset(4)] public readonly Vector3 vector3Value;
    [FieldOffset(4)] public readonly Vector4 vector4Value;

    public FragmentAttribute(float value)
    {
        type = FragmentAttributeType.Float;
        floatValue = value;
    }

    public FragmentAttribute(Vector2 value)
    {
        type = FragmentAttributeType.Vector2;
        vector2Value = value;
    }

    public FragmentAttribute(Vector3 value)
    {
        type = FragmentAttributeType.Vector3;
        vector3Value = value;
    }

    public FragmentAttribute(Vector4 value)
    {
        type = FragmentAttributeType.Vector4;
        vector4Value = value;
    }
}