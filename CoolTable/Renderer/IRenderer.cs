using CoolTable.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CoolTable.Renderer
{
    public interface IRenderer
    {
        void HeaderDesign(Graphics g, Bag bag);
        void BorderDesign(Graphics g, Bag bag);
        void InnerDesign(Graphics g, Bag bag);
    }
}
