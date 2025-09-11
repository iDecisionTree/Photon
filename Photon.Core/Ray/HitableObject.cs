using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public interface HitableObject
    {
        public abstract bool Intersect(Ray ray, out HitInfo hitInfo);
    }
}
