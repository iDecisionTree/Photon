using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class MeshRenderer : Component
    {
        public Mesh mesh
        {
            get => mesh;
            set => mesh = value;
        }

        private Mesh _mesh;

        public MeshRenderer()
        {

        }


    }
}
