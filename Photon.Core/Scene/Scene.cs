using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public class Scene
    {
        public HitableObject[] hitableObjects => _hitableObjects.ToArray();

        private List<HitableObject> _hitableObjects;

        public Scene()
        {
            _hitableObjects = new List<HitableObject>();
        }

        public void AddHitableObject(HitableObject hitableObject)
        {
            if (!_hitableObjects.Contains(hitableObject))
            {
                _hitableObjects.Add(hitableObject);
            }
        }

        public void RemoveHitableObject(HitableObject hitableObject)
        {
            if (_hitableObjects.Contains(hitableObject))
            {
                _hitableObjects.Remove(hitableObject);
            }
        }

        public bool Intersect(Ray ray, out HitInfo hitInfo)
        {
            hitInfo = new HitInfo();
            hitInfo.t = float.MaxValue;
            HitableObject closestObject = null;

            bool isHit = false;

            for (int i = 0; i < _hitableObjects.Count; i++)
            {
                Matrix4x4 worldToObjectMatrix;
                if (!Matrix4x4.Invert(_hitableObjects[i].transform.modelMatrix, out worldToObjectMatrix))
                {
                    continue;
                }
                Ray rayObject = ray.WorldToObject(worldToObjectMatrix);

                if (_hitableObjects[i].Intersect(rayObject, out HitInfo tempInfo))
                {
                    if (tempInfo.t < hitInfo.t)
                    {
                        hitInfo = tempInfo;
                        isHit = true;
                        closestObject = _hitableObjects[i];
                    }
                }
            }

            if (isHit)
            {
                hitInfo.point = Vector3.Transform(hitInfo.point, closestObject.transform.modelMatrix);
                hitInfo.normal = Vector3.Normalize(Vector3.TransformNormal(hitInfo.normal, closestObject.transform.modelMatrix));
            }

            return isHit;
        }
    }
}
