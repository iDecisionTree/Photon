using Photon.Core.Lighting;
using Photon.Math.Vector;

namespace Photon.Core.Component
{
    public class Light : ComponentBase
    {
        public LightType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        public Vector4 color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        private LightType _type;
        private Vector4 _color;

        public Vector3 GetLightDirection(Vector3 positionWS)
        {
            switch (type)
            {
                case LightType.Directional:
                    return -sceneObject!.transform.forward;
                case LightType.Point:
                    return Vector3.Normalize(sceneObject!.transform.position - positionWS);
                default:
                    throw new NotImplementedException("不支持的光源类型");
            }
        }

        public float GetAttenuation(Vector3 positionWS)
        {
            switch (type)
            {
                case LightType.Directional:
                    return 1f;
                case LightType.Point:
                    float distance = Vector3.Distance(sceneObject!.transform.position, positionWS);
                    return 1f / (1f + 0.1f * distance + 0.01f * distance * distance);
                default:
                    throw new NotImplementedException("不支持的光源类型");
            }
        }
    }
}
