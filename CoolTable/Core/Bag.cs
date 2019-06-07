using CoolTable.Control;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CoolTable.Core
{
    public class Bag
    {
        private ScrollBarCorner corner = ScrollBarCorner._3_Right_Bottom;

        private Font font = new Font("Arial", 10f, FontStyle.Regular);

        private object data = null;
        private float x = 0;
        private float y = 0;
        private float width = 150;
        private float lineHeight = 20;

        private float lineWeight = 1;
        private Color lineColor = Color.Black;

        private Color background = Color.White;
        private Color foreground = Color.Black;
        private Color backgroundWhenSelected = Color.AliceBlue;
        private Color foregroundWhenSelected = Color.Black;

        private string headerText = "Column ##";

        private Color headerBackground = Color.Gray;
        private Color headerForeground = Color.White;
        private Color headerBackgroundWhenSelected = Color.LightGray;
        private Color headerForegroundWhenSelected = Color.Black;

        private Color lineBackgroundColor = Color.Purple;
        private Color lineForegroundColor = Color.White;
        private Color lineBackgroundColorWhenSelected = Color.Pink;
        private Color lineForegroundColorWhenSelected = Color.Black;

        private bool isRowSelected = false;
        private bool isColumnSelected = false;
        private bool isLineNumberColumn = false;

        private int columnIndex = -1;
        private int rowIndex = -1;

        public Bag()
        {
        }

        public ScrollBarCorner ScrollBarCorner { get => corner; set => corner = value; }

        public Font Font { get => font; set => font = value; }

        public object Data { get => data; set => data = value; }
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Width { get => width; set => width = value; }
        public float LineHeight { get => lineHeight; set => lineHeight = value; }

        public float LineWeight { get => lineWeight; set => lineWeight = value; }
        public Color LineColor { get => lineColor; set => lineColor = value; }

        public Color Background { get => background; set => background = value; }
        public Color Foreground { get => foreground; set => foreground = value; }
        public Color BackgroundWhenSelected { get => backgroundWhenSelected; set => backgroundWhenSelected = value; }
        public Color ForegroundWhenSelected { get => foregroundWhenSelected; set => foregroundWhenSelected = value; }

        public string HeaderText { get => headerText; set => headerText = value; }

        public Color HeaderBackground { get => headerBackground; set => headerBackground = value; }
        public Color HeaderForeground { get => headerForeground; set => headerForeground = value; }
        public Color HeaderBackgroundWhenSelected { get => headerBackgroundWhenSelected; set => headerBackgroundWhenSelected = value; }
        public Color HeaderForegroundWhenSelected { get => headerForegroundWhenSelected; set => headerForegroundWhenSelected = value; }

        public Color LineBackgroundColor { get => lineBackgroundColor; set => lineBackgroundColor = value; }
        public Color LineForegroundColor { get => lineForegroundColor; set => lineForegroundColor = value; }
        public Color LineBackgroundColorWhenSelected { get => lineBackgroundColorWhenSelected; set => lineBackgroundColorWhenSelected = value; }
        public Color LineForegroundColorWhenSelected { get => lineForegroundColorWhenSelected; set => lineForegroundColorWhenSelected = value; }
        
        public bool IsRowSelected { get => isRowSelected; set => isRowSelected = value; }
        public bool IsColumnSelected { get => isColumnSelected; set => isColumnSelected = value; }
        public bool IsLineNumberColumn { get => isLineNumberColumn; set => isLineNumberColumn = value; }

        public int ColumnIndex { get => columnIndex; set => columnIndex = value; }
        public int RowIndex { get => rowIndex; set => rowIndex = value; }

        public Color GetRowForegroundColor()
        {
            return isRowSelected ? foregroundWhenSelected : foreground;
        }

        public Color GetRowBackgroundColor()
        {
            return isRowSelected ? backgroundWhenSelected : background;
        }

        public Color GetHeaderForegroundColor()
        {
            return isColumnSelected ? headerForegroundWhenSelected : headerForeground;
        }

        public Color GetHeaderBackgroundColor()
        {
            return isColumnSelected ? headerBackgroundWhenSelected : headerBackground;
        }

        public Color GetLineNumberForegroundColor()
        {
            return isRowSelected ? lineForegroundColorWhenSelected : lineForegroundColor;
        }

        public Color GetLineNumberBackgroundColor()
        {
            return isRowSelected ? lineBackgroundColorWhenSelected : lineBackgroundColor;
        }
    }
}
