using Photon.Math.Vector;

namespace Photon.Core.Shader
{
    public struct LightData
    {
        public Vector3 direction;
        public float attenuation;
        public Vector4 color;

        public LightData(Vector3 direction, float attenuation, Vector4 color)
        {
            this.direction = direction;
            this.attenuation = attenuation;
            this.color = color;
        }
    }
}
