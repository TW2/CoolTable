using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CoolTable.Control
{
    public class Scrollbar
    {
        private ScrollBarType type = ScrollBarType.Right;

        private int minValue = 0;
        private int maxValue = 1000;
        private int curValue = 0;

        private float scrollbarWidth = 20;

        private Color backgroundColor = Color.LightGray;
        private Color foregroundColor = Color.White;
        private Color lineColor = Color.Black;
        private Color elementsColor = Color.BlueViolet;

        private int lineWeight = 1;

        public Scrollbar()
        {
        }

        public ScrollBarType ScrollBarType { get => type; set => type = value; }

        public int MinimumValue { get => minValue; set => minValue = value; }
        public int MaximumValue { get => maxValue; set => maxValue = value; }
        public int CurrentValue { get => curValue; set => curValue = value; }

        public float ScrollbarWidth { get => scrollbarWidth; set => scrollbarWidth = value; }

        public Color BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public Color ForegroundColor { get => foregroundColor; set => foregroundColor = value; }
        public Color LineColor { get => lineColor; set => lineColor = value; }
        public Color ElementsColor { get => elementsColor; set => elementsColor = value; }
        public int LineWeight { get => lineWeight; set => lineWeight = value; }

        public static Scrollbar Create(ScrollBarType type = ScrollBarType.Right)
        {
            Scrollbar sb = new Scrollbar();

            sb.type = type;

            return sb;
        }
    }
}
