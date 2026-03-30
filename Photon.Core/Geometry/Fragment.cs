using Photon.Math.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core.Geometry
{
    public struct Fragment
    {
        public readonly Vector2 positionSS;
        public Vector4 color;
        public readonly Dictionary<string, FragmentAttribute> attributes;

        public Fragment(Vector2 positionSS, Vector4 color, Dictionary<string, FragmentAttribute> attributes)
        {
            this.positionSS = positionSS;
            this.color = color;
            this.attributes = attributes;
        }
    }
}
