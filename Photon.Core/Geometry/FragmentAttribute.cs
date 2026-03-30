using Photon.Core.Geometry;
using Photon.Math.Vector;

public readonly struct FragmentAttribute
{
    public readonly FragmentAttributeType type;
    public readonly object value;

    public FragmentAttribute(object value)
    {
        if (value is float)
        {
            type = FragmentAttributeType.Float;
        }
        else if (value is Vector2)
        {
            type = FragmentAttributeType.Vector2;
        }
        else if (value is Vector3)
        {
            type = FragmentAttributeType.Vector3;
        }
        else if (value is Vector4)
        {
            type = FragmentAttributeType.Vector4;
        }
        else
        {
            throw new ArgumentException($"不支持的片元属性类型{value.GetType()}");
        }

        this.value = value;
    }

    public FragmentAttribute(FragmentAttributeType type, object value)
    {
        this.type = type;
        this.value = value;
    }
}