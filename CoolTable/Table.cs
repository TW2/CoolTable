using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CoolTable.Core;
using CoolTable.Control;

namespace CoolTable
{
    public partial class Table : UserControl
    {
        Bag bag = new Bag();
        List<Column> columns = new List<Column>();
        ScrollBarManager manager = null;
        bool alwaysDisplayHeader = true;

        float curY, oldCurY = 0f;
        float grip = 25;

        public Table()
        {
            InitializeComponent();
            Console.WriteLine("Hello World");
            DoubleBuffered = true;
            manager = ScrollBarManager.Create(ScrollBarCorner._1_Left_Bottom, this);
            Resize += Table_Resize;
            Paint += Table_Paint;
            MouseWheel += Table_MouseWheel;
        }

        private void Table_MouseWheel(object sender, MouseEventArgs e)
        {
            oldCurY = oldCurY + (e.Delta > 0 ? 3 : -3);

            if(oldCurY < -GetLinesCount())
            {
                oldCurY = -GetLinesCount();
            }

            if(oldCurY > 0)
            {
                oldCurY = 0;
            }


            Event.ScrollEventArgs evt = new Event.ScrollEventArgs(
                oldCurY,
                GetLinesCountHeight(),
                bag);
            
            curY = -evt.Position;
            grip = GetLinesCountHeight() / 4f;
            if (grip < 20) { grip = 20; }

            _Scrolling(evt);
        }

        public event EventHandler<Event.ScrollEventArgs> OnScrolling;

        protected virtual void _Scrolling(Event.ScrollEventArgs e)
        {
            EventHandler<Event.ScrollEventArgs> handler = OnScrolling;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void Table_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Table_Paint(object sender, PaintEventArgs e)
        {
            //Initialization for table
            float xOffset = 0;
            float yOffset = 0;
            float xColumn, yRow;
            int lineIndex = 0;

            grip = GetLinesCountHeight() / 4f;
            if (grip < 20) { grip = 20; }

            switch (manager.Corner)
            {
                case ScrollBarCorner._1_Left_Bottom:
                    xOffset = manager.ScrollbarWidth; // Avoid scrollbar
                    yOffset = 0;
                    break;
                case ScrollBarCorner._3_Right_Bottom:
                    xOffset = 0;
                    yOffset = 0;
                    break;
                case ScrollBarCorner._7_Left_Top:
                    xOffset = manager.ScrollbarWidth; // Avoid scrollbar
                    yOffset = manager.ScrollbarWidth; // Avoid scrollbar
                    break;
                case ScrollBarCorner._9_Right_Top:
                    xOffset = 0;
                    yOffset = manager.ScrollbarWidth; // Avoid scrollbar
                    break;
            }

            // La valeur de l'offset doit être inférieur à 0 et supérieur à -(total de lignes * lineheight)
            // Mise à jour par une fonction appelée avant.
            // D'où :
            yOffset -= curY;

            //==============================================================================
            // Fill the background with default back color
            //------------------------------------------------------------------------------
            e.Graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, Width, Height);
            
            #region Lines of table

            //==============================================================================
            // LINES
            //------------------------------------------------------------------------------
            xColumn = xOffset;
            yRow = yOffset + manager.ScrollbarWidth; // Height of header = scrollbarWidth (default: 20px)
            lineIndex = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                Column c = columns[i];

                //Partially fill the bag
                bag.X = xColumn;
                bag.Width = c.ColumnWidth;
                bag.ColumnIndex = i;
                bag.Y = yRow;
                bag.IsLineNumberColumn = c.IsLineNumberColumn;

                //Modify the bag for header
                bag.LineHeight = 20;

                for (int j = lineIndex; j < c.DataCount; j++)
                {
                    //Partially fill the bag
                    bag.Y = yRow;
                    bag.RowIndex = j;

                    c.Renderer.BorderDesign(e.Graphics, bag);

                    yRow += 20;
                }

                //Initialization for next loop
                xColumn += c.ColumnWidth;
                yRow = yOffset + manager.ScrollbarWidth;
            }

            #endregion

            #region Data to fill the table

            //==============================================================================
            // DATA
            //------------------------------------------------------------------------------
            xColumn = xOffset;
            yRow = yOffset + manager.ScrollbarWidth; // Height of header = scrollbarWidth (default: 20px)
            lineIndex = 0;
            for (int i = 0; i < columns.Count; i++)
            {
                Column c = columns[i];

                //Partially fill the bag
                bag.X = xColumn;
                bag.Width = c.ColumnWidth;
                bag.LineHeight = 20;
                bag.ColumnIndex = i;
                bag.IsLineNumberColumn = c.IsLineNumberColumn;

                //TODO - range showFrom showTo


                for (int j = lineIndex; j < c.DataCount; j++)
                {
                    object data = c.ToArray()[j];

                    //Partially fill the bag
                    bag.Y = yRow;
                    bag.RowIndex = j;                    
                    bag.Data = c.IsLineNumberColumn == false ? data : Convert.ToString(j + 1);

                    c.Renderer.InnerDesign(e.Graphics, bag);

                    yRow += manager.ScrollbarWidth;
                }

                //Initialization for next loop
                xColumn += c.ColumnWidth;
                yRow = yOffset + manager.ScrollbarWidth;
            }

            #endregion

            #region Header of a table

            //==============================================================================
            // HEADER
            //------------------------------------------------------------------------------
            xColumn = xOffset;
            yRow = yOffset;
            for (int i = 0; i < columns.Count; i++)
            {
                Column c = columns[i];

                //Fill the bag
                bag.X = xColumn;
                bag.Y = alwaysDisplayHeader == false ? yRow : 0;
                bag.Width = c.ColumnWidth;
                bag.LineHeight = 20;
                bag.ColumnIndex = c.ColumnIndex;
                bag.RowIndex = 0;
                bag.HeaderText = c.Title;

                c.Renderer.HeaderDesign(e.Graphics, bag);

                //Initialization for next loop
                xColumn += c.ColumnWidth;
            }

            #endregion

            #region Scrollbars and corners

            //==============================================================================
            // CUSTOM SCROLLBARs
            //------------------------------------------------------------------------------
            //
            // Corner (Right-Bottom) (NumPad 3):
            // +-------------------+
            // |                   |
            // |                   |
            // |                   |
            // +------------------** <-- this corner
            //
            // Corner (Right-Top)  (NumPad 9):
            // +------------------** <-- this corner
            // |                   |
            // |                   |
            // |                   |
            // +-------------------+
            //
            // Corner (Left-Bottom) (NumPad 1)
            //                 +-------------------+
            //                 |                   |
            //                 |                   |
            //                 |                   |
            // this corner --> **------------------+ 
            //
            // Corner (Left-Top) (NumPad 7)
            // this corner --> **------------------+
            //                 |                   |
            //                 |                   |
            //                 |                   |
            //                 +-------------------+ 
            manager.DrawScrollbars(e);

            #endregion

        }

        public Bag GetBag()
        {
            return bag;
        }

        public int GetColumnsCount()
        {
            return columns.Count;
        }

        public int GetLinesCount()
        {
            return columns.Count > 0 ? columns[0].DataCount : 0;
        }

        public int GetLinesCountHeight()
        {
            int lines = columns.Count > 0 ? columns[0].DataCount : 0;
            return Convert.ToInt32(lines * bag.LineHeight);
        }

        public float GetColumnsWidth()
        {
            float output = 0f;

            foreach(Column c in columns)
            {
                output += c.ColumnWidth;
            }

            return output;
        }

        public float GetGrip()
        {
            return grip;
        }

        public void AddLineNumberColumn(string title = "Line")
        {
            Column c = Column.Create(typeof(string), title, 50);
            c.IsLineNumberColumn = true;
            c.ColumnIndex = columns.Count;
            columns.Add(c);

            // Update columns size
            float wc = 0f;
            foreach(Column cx in columns)
            {
                wc += cx.ColumnWidth;
            }
            manager.ColumnWrap = wc;
        }

        public void AddColumn(Column c)
        {
            c.ColumnIndex = columns.Count;
            columns.Add(c);

            // Update columns size
            float wc = 0f;
            foreach (Column cx in columns)
            {
                wc += cx.ColumnWidth;
            }
            manager.ColumnWrap = wc;
        }

        public void AddRow(object[] values)
        {
            bool lineNumberHasBeenFound = false;

            for (int i = 0; i < values.Length; i++)
            {
                if(columns[i].IsLineNumberColumn == true | lineNumberHasBeenFound == true)
                {
                    if(lineNumberHasBeenFound == false)
                    {
                        columns[i].AddValue("");
                        lineNumberHasBeenFound = true;
                    }
                    else
                    {
                        columns[i].AddValue(values[i]);
                    }
                }
                else
                {
                    columns[i].AddValue(values[i]);
                }                
            }

            // Update maximum value
            manager.MaximumValue = GetLinesCountHeight();

            Refresh();
        }

    }
}
