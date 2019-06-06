using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CoolTable.Core;

namespace CoolTable.Renderer
{
    public class ARenderer : IRenderer
    {
        public ARenderer()
        {

        } 

        public void BorderDesign(Graphics g, Bag bag)
        {
            Color back = bag.IsLineNumberColumn == true ?
                    bag.GetLineNumberBackgroundColor() : 
                    bag.GetRowBackgroundColor();

            g.FillRectangle(new SolidBrush(back), bag.X, bag.Y, bag.Width, bag.LineHeight);
            g.DrawRectangle(new Pen(bag.LineColor, bag.LineWeight), bag.X, bag.Y, bag.Width, bag.LineHeight);
        }

        public void HeaderDesign(Graphics g, Bag bag)
        {
            g.FillRectangle(new SolidBrush(bag.GetHeaderBackgroundColor()), bag.X, bag.Y, bag.Width, bag.LineHeight);
            g.DrawRectangle(new Pen(bag.LineColor, bag.LineWeight), bag.X, bag.Y, bag.Width, bag.LineHeight);
            RectangleF rectf = new RectangleF(bag.X + 2f, bag.Y + 2f, bag.Width - 4f, bag.LineHeight - 4f);
            g.DrawString(bag.HeaderText, bag.Font, new SolidBrush(bag.GetHeaderForegroundColor()), rectf);
        }

        public void InnerDesign(Graphics g, Bag bag)
        {
            Color fore = bag.IsLineNumberColumn == true ?
                    bag.GetLineNumberForegroundColor() :
                    bag.GetRowForegroundColor();

            RectangleF rectf = new RectangleF(bag.X + 2f, bag.Y + 2f, bag.Width - 4f, bag.LineHeight - 4f);
            g.DrawString(bag.Data.ToString(), bag.Font, new SolidBrush(fore), rectf);
        }
    }
}
