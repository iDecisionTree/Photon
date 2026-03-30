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
        public Dictionary<string, FragmentAttribute[]> attributes { get; set; }

        public GeometryObject()
        {
            attributes = new Dictionary<string, FragmentAttribute[]>();
        }

        public void Initialize()
        {
            if (mesh == null)
            {
                return;
            }

            attributes["positionOS"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["positionWS"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["positionCS"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["positionNDC"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["positionSS"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["normalOS"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["normalWS"] = new FragmentAttribute[mesh.vertices.Count];
            attributes["depth"] = new FragmentAttribute[mesh.vertices.Count];
        }
    }
}
