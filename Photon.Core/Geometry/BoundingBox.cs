using Photon.Math.Vector;

namespace Photon.Core.Geometry
{
    public readonly struct BoundingBox
    {
        public readonly Vector3 min;
        public readonly Vector3 max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }

        public Vector3 center => (min + max) / 2f;
        public Vector3 size => max - min;
    }
}
