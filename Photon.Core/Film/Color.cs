using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photon.Core
{
    public struct Color
    {
        public byte r
        {
            get => _r;
            set => _r = value;
        }

        public byte g
        {
            get => _g;
            set => _g = value;
        }

        public byte b
        {
            get => _b;
            set => _b = value;
        }

        private byte _r;
        private byte _g;
        private byte _b;

        public Color(byte r, byte g, byte b)
        {
            _r = r;
            _g = g;
            _b = b;
        }
    }
}
