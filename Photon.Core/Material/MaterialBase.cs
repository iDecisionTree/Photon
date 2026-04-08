using Photon.Core.Shader;

namespace Photon.Core.Material
{
    public abstract class MaterialBase
    {
        public ShaderBase? shader { get; set; } = null;
        public Dictionary<string, int> propertyIndexMap { get; set; }
        public ShaderUniform[]? shaderUniforms { get; set; } = null;

        protected MaterialBase()
        {
            propertyIndexMap = new Dictionary<string, int>();
        }

        public abstract void InitializeUniform();
        public abstract void BindUniform();
    }
}
