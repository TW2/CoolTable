using CoolTable.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoolTable.Event
{
    public delegate void ScrollEventHandler(object sender, ScrollEventArgs e);

    public class ScrollEventArgs : EventArgs
    {
        private float _position;

        public ScrollEventArgs(float position, float height, Bag bag)
        {
            _position = position * bag.LineHeight;

            if(position * bag.LineHeight < -height)
            {
                _position = -height;
            }

            if(position * bag.LineHeight > 0)
            {
                _position = 0;
            }

        }

        public float Position { get => _position; }
    }
}
