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

        public Table()
        {
            InitializeComponent();
            Console.WriteLine("Hello World");
            DoubleBuffered = true;
            manager = ScrollBarManager.Create(ScrollBarCorner._3_Right_Bottom, this);
            Resize += Table_Resize;
            Paint += Table_Paint;
        }

        private void Table_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Table_Paint(object sender, PaintEventArgs e)
        {
            //Initialization for table
            float x = 0;// - horScrollBar.Value;
            float xColumn, yRow;
            int lineIndex = 0;

            //==============================================================================
            // Fill the background with default back color
            //------------------------------------------------------------------------------
            e.Graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, Width, Height);

            //==============================================================================
            // Update scrollbars
            //------------------------------------------------------------------------------
            manager.DrawScrollbars(e);

            #region Header of a table

            //==============================================================================
            // HEADER
            //------------------------------------------------------------------------------
            xColumn = x;
            for (int i = 0; i < columns.Count; i++)
            {
                Column c = columns[i];

                //Fill the bag
                bag.X = xColumn;
                bag.Y = 0;
                bag.Width = c.ColumnWidth;
                bag.LineHeight = 20;
                bag.ColumnIndex = c.ColumnIndex;
                bag.RowIndex = 0;
                bag.HeaderText = c.Title;

                //Drawing
                //if (c.PageLayout != null)
                //{
                //    c.PageLayout.GetHeader(e.Graphics, bag);
                //}
                //else
                //{
                //    c.BoxRenderer.GetHeader(e.Graphics, bag);
                //}
                c.Renderer.HeaderDesign(e.Graphics, bag);

                //Initialization for next loop
                xColumn += c.ColumnWidth;
            }

            #endregion

            #region Lines of table

            //==============================================================================
            // LINES
            //------------------------------------------------------------------------------
            xColumn = x;
            yRow = 20; //Height of header
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

                    //if (c.PageLayout != null)
                    //{
                    //    c.PageLayout.GetLines(e.Graphics, bag);
                    //}
                    //else
                    //{
                    //    c.BoxRenderer.GetLines(e.Graphics, bag);
                    //}

                    c.Renderer.BorderDesign(e.Graphics, bag);

                    yRow += 20;
                }

                //Initialization for next loop
                xColumn += c.ColumnWidth;
                yRow = 20;
            }

            #endregion

            #region Data to fill the table

            //==============================================================================
            // DATA
            //------------------------------------------------------------------------------
            xColumn = x;
            yRow = 20;
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

                    //if (c.Rules != null)
                    //{
                    //    c.BoxRenderer.GetRender(e.Graphics, bag);// Default value

                    //    foreach (IRule rule in c.Rules)
                    //    {
                    //        rule.GetColumnRule(e.Graphics, bag);// If rule = override
                    //    }
                    //}
                    //else if (c.PageLayout != null)
                    //{
                    //    c.PageLayout.GetRender(e.Graphics, bag);
                    //}
                    //else
                    //{
                    //    c.BoxRenderer.GetRender(e.Graphics, bag);
                    //}

                    c.Renderer.InnerDesign(e.Graphics, bag);

                    yRow += 20;
                }

                //Initialization for next loop
                xColumn += c.ColumnWidth;
                yRow = 20;
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
            // Nothing to do because handled by ScrollManager class itself.

            #endregion

        }

        public void AddLineNumberColumn()
        {
            Column c = Column.Create(typeof(string), "Line", 50);
            c.IsLineNumberColumn = true;
            c.ColumnIndex = columns.Count;
            columns.Add(c);
        }

        public void AddColumn(Column c)
        {
            c.ColumnIndex = columns.Count;
            columns.Add(c);
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

            Refresh();
        }

    }
}
