using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public struct Ray
    {
        public Vector3 origin
        {
            get => _origin;
            set => _origin = value;
        }

        public Vector3 direction
        {
            get => _direction;
            set => _direction = value;
        }

        private Vector3 _origin;
        private Vector3 _direction;
    
        /// <summary>
        /// 传入的方向默认已经归一化
        /// </summary>
        public Ray(Vector3 origin, Vector3 direction)
        {
            _origin = origin;
            _direction = direction;
        }

        public Vector3 At(float t)
        {
            return origin + t * _direction;
        }
    }
}
