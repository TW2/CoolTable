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
        Scrollbar sbVertical = Scrollbar.Create(ScrollBarType.Right); // Right (default)
        Scrollbar sbHorizontal = Scrollbar.Create(ScrollBarType.Bottom); // Bottom

        public Table()
        {
            InitializeComponent();
            Console.WriteLine("Hello World");
            DoubleBuffered = true;
            Paint += Table_Paint;
        }

        private void Table_Paint(object sender, PaintEventArgs e)
        {
            //Initialization for table
            float x = 0;// - horScrollBar.Value;
            float xColumn, yRow;
            float xa, ya;
            int lineIndex = 0;
            PointF p1, p2, p3;

            //==============================================================================
            // Fill the background with default back color
            //------------------------------------------------------------------------------
            e.Graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, Width, Height);

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

            // Fill area and corner
            //-----------
            // The area is defined by :
            // - Table.Width -> Need it for auto-positionning
            // - Table.Height -> Need it for auto-positionning
            // The corner is the place where two scrollbars enconter.

            if (sbVertical.ScrollBarType == ScrollBarType.Right && sbHorizontal.ScrollBarType == ScrollBarType.Bottom)
            {
                // Corner (Right-Bottom) :
                // +-------------------+
                // |                   |
                // |                   |
                // |                   |
                // +------------------** <-- this corner 
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    Width - sbVertical.ScrollbarWidth,
                    Height - sbHorizontal.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                // Background (Right-Bottom and vertical)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    Width - sbVertical.ScrollbarWidth,
                    0,
                    sbVertical.ScrollbarWidth,
                    Height - sbVertical.ScrollbarWidth);

                // Background (Right-Bottom and horizontal)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbHorizontal.BackgroundColor),
                    0,
                    Height - sbHorizontal.ScrollbarWidth,
                    Width - sbHorizontal.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                //-------- Arrows --------
                // Vertical - TOP
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.ElementsColor),
                    Width - sbVertical.ScrollbarWidth,
                    0,
                    sbVertical.ScrollbarWidth,
                    sbVertical.ScrollbarWidth);
                xa = Width - sbVertical.ScrollbarWidth;
                ya = 0;
                p1 = new PointF(xa + sbVertical.ScrollbarWidth / 2f, ya + sbVertical.ScrollbarWidth / 3f);
                p2 = new PointF(xa + sbVertical.ScrollbarWidth / 3f, ya + sbVertical.ScrollbarWidth / 3f);
                p3 = new PointF(xa + sbVertical.ScrollbarWidth * 2f / 3f, ya + sbVertical.ScrollbarWidth / 3f);
                e.Graphics.FillPolygon(new SolidBrush(Color.Yellow), new PointF[] { p1, p2, p3 });
                // Vertical - BOTTOM
                // Horizontal - LEFT
                // Horizontal - RIGHT
                //------------------------

                // Corner
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    Width - sbVertical.ScrollbarWidth,
                    Height - sbHorizontal.ScrollbarWidth,
                    sbVertical.ScrollbarWidth - 1,
                    sbHorizontal.ScrollbarWidth - 1);

                // Foreground lines (Right-Bottom and vertical)
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    Width - sbVertical.ScrollbarWidth,
                    0,
                    sbVertical.ScrollbarWidth - 1,
                    Height - sbVertical.ScrollbarWidth);

                // Foreground lines (Right-Bottom and horizontal)
                e.Graphics.DrawRectangle(
                    new Pen(sbHorizontal.LineColor, sbHorizontal.LineWeight),
                    0,
                    Height - sbHorizontal.ScrollbarWidth,
                    Width - sbHorizontal.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth - 1);
            }
            else if (sbVertical.ScrollBarType == ScrollBarType.Right && sbHorizontal.ScrollBarType == ScrollBarType.Top)
            {
                // Corner (Right-Top) :
                // +------------------** <-- this corner
                // |                   |
                // |                   |
                // |                   |
                // +-------------------+
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    Width - sbVertical.ScrollbarWidth,
                    0,
                    sbVertical.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                // Background (Right-Top and vertical)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    Width - sbVertical.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    Height);

                // Background (Right-Top and horizontal)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbHorizontal.BackgroundColor),
                    0,
                    0,
                    Width - sbHorizontal.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                // Corner
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    Width - sbVertical.ScrollbarWidth,
                    0,
                    sbVertical.ScrollbarWidth - 1,
                    sbHorizontal.ScrollbarWidth);

                // Foreground lines (Right-Top and vertical)
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    Width - sbVertical.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    sbVertical.ScrollbarWidth - 1,
                    Height);

                // Foreground lines (Right-Top and horizontal)
                e.Graphics.DrawRectangle(
                    new Pen(sbHorizontal.LineColor, sbHorizontal.LineWeight),
                    0,
                    0,
                    Width - sbHorizontal.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);
            }
            else if (sbVertical.ScrollBarType == ScrollBarType.Left && sbHorizontal.ScrollBarType == ScrollBarType.Bottom)
            {
                // Corner
                //                 +-------------------+
                //                 |                   |
                //                 |                   |
                //                 |                   |
                // this corner --> **------------------+ 
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    0,
                    Height - sbHorizontal.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                // Background (Left-Bottom and vertical)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    0,
                    0,
                    sbVertical.ScrollbarWidth,
                    Height - sbVertical.ScrollbarWidth);

                // Background (Left-Bottom and horizontal)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbHorizontal.BackgroundColor),
                    sbHorizontal.ScrollbarWidth,
                    Height - sbHorizontal.ScrollbarWidth,
                    Width,
                    sbHorizontal.ScrollbarWidth);

                // Corner
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    0,
                    Height - sbHorizontal.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth - 1);

                // Foreground lines (Left-Bottom and vertical)
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    0,
                    0,
                    sbVertical.ScrollbarWidth,
                    Height - sbVertical.ScrollbarWidth);

                // Foreground lines (Left-Bottom and horizontal)
                e.Graphics.DrawRectangle(
                    new Pen(sbHorizontal.LineColor, sbHorizontal.LineWeight),
                    sbHorizontal.ScrollbarWidth,
                    Height - sbHorizontal.ScrollbarWidth,
                    Width,
                    sbHorizontal.ScrollbarWidth - 1);
            }
            else if (sbVertical.ScrollBarType == ScrollBarType.Left && sbHorizontal.ScrollBarType == ScrollBarType.Top)
            {
                // Corner
                // this corner --> **------------------+
                //                 |                   |
                //                 |                   |
                //                 |                   |
                //                 +-------------------+ 
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    0,
                    0,
                    sbVertical.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                // Background (Left-Top and vertical)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbVertical.BackgroundColor),
                    0,
                    sbHorizontal.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    Height - sbVertical.ScrollbarWidth);

                // Background (Left-Top and horizontal)
                e.Graphics.FillRectangle(
                    new SolidBrush(sbHorizontal.BackgroundColor),
                    sbHorizontal.ScrollbarWidth,
                    0,
                    Width,
                    sbHorizontal.ScrollbarWidth);

                // Corner
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    0,
                    0,
                    sbVertical.ScrollbarWidth,
                    sbHorizontal.ScrollbarWidth);

                // Foreground lines (Left-Top and vertical)
                e.Graphics.DrawRectangle(
                    new Pen(sbVertical.LineColor, sbVertical.LineWeight),
                    0,
                    sbHorizontal.ScrollbarWidth,
                    sbVertical.ScrollbarWidth,
                    Height - sbVertical.ScrollbarWidth);

                // Foreground lines (Left-Top and horizontal)
                e.Graphics.DrawRectangle(
                    new Pen(sbHorizontal.LineColor, sbHorizontal.LineWeight),
                    sbHorizontal.ScrollbarWidth,
                    0,
                    Width,
                    sbHorizontal.ScrollbarWidth);
            }

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
