using Photon.Math.Matrix;
using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.Geometry
{
    public class GeometryObject
    {
        public Mesh? mesh { get; set; } = null;
        public Primitive primitive { get; set; }
        public Matrix4x4 worldMatrix { get; set; }
        public Vector3[]? positionWS { get; set; } = null;
        public Vector4[]? positionCS { get; set; } = null;
        public Vector3[]? positionNDC { get; set; } = null;
        public Vector2[]? positionSS { get; set; } = null;

        public void Initialize()
        {
            if (mesh == null)
            {
                return;
            }

            positionCS = new Vector4[mesh.vertices.Count];
            positionNDC = new Vector3[mesh.vertices.Count];
            positionSS = new Vector2[mesh.vertices.Count];
        }
    }
}
