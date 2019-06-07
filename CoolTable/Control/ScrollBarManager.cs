using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CoolTable.Control
{
    public class ScrollBarManager
    {
        private ScrollBarCorner corner = ScrollBarCorner._3_Right_Bottom;
        private System.Windows.Forms.Control parentCtrl = null;

        private int minValue = 0;
        private int maxValue = 1000;
        private int curValue = 0;

        private float scrollbarWidth = 20;

        private Color backgroundColor = Color.LightGray;
        private Color foregroundColor = Color.White;
        private Color lineColor = Color.Black;
        private Color elementsColor = Color.BlueViolet;

        private int lineWeight = 1;

        public ScrollBarManager()
        {
            
        }

        public ScrollBarCorner Corner { get => corner; set => corner = value; }

        public int MinimumValue { get => minValue; set => minValue = value; }
        public int MaximumValue { get => maxValue; set => maxValue = value; }
        public int CurrentValue { get => curValue; set => curValue = value; }

        public float ScrollbarWidth { get => scrollbarWidth; set => scrollbarWidth = value; }

        public Color BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public Color ForegroundColor { get => foregroundColor; set => foregroundColor = value; }
        public Color LineColor { get => lineColor; set => lineColor = value; }
        public Color ElementsColor { get => elementsColor; set => elementsColor = value; }
        public int LineWeight { get => lineWeight; set => lineWeight = value; }

        public System.Windows.Forms.Control ParentControl { get => parentCtrl; set => parentCtrl = value; }

        public static ScrollBarManager Create(ScrollBarCorner corner, System.Windows.Forms.Control ctrl)
        {
            ScrollBarManager sb = new ScrollBarManager();

            sb.corner = corner;
            sb.parentCtrl = ctrl;
            sb.parentCtrl.Refresh();

            return sb;
        }

        //================================================================

        public void DrawScrollbars(System.Windows.Forms.PaintEventArgs e)
        {
            DoVertical(e);
            DoHorizontal(e);
        }

        #region Vertical

        private void DoVertical(System.Windows.Forms.PaintEventArgs e)
        {
            float x = 0, y = 0, width = scrollbarWidth, height = parentCtrl.Height - scrollbarWidth;
            float xa = 0, ya = 0;
            PointF pTop1, pTop2, pTop3;
            PointF pBottom1, pBottom2, pBottom3;

            // Retrieve coordinates
            switch (corner)
            {
                case ScrollBarCorner._1_Left_Bottom:
                    x = 0;
                    y = 0;
                    break;
                case ScrollBarCorner._3_Right_Bottom:
                    x = parentCtrl.Width - scrollbarWidth;
                    y = 0;
                    break;
                case ScrollBarCorner._7_Left_Top:
                    x = 0;
                    y = scrollbarWidth;
                    break;
                case ScrollBarCorner._9_Right_Top:
                    x = parentCtrl.Width - scrollbarWidth;
                    y = scrollbarWidth;
                    break;
            }

            // Fill corner and vertical
            // 1. Corner
            if(corner == ScrollBarCorner._1_Left_Bottom | corner == ScrollBarCorner._3_Right_Bottom)
            {
                xa = corner == ScrollBarCorner._1_Left_Bottom ? 0 : parentCtrl.Width - scrollbarWidth;
                ya = height;
            }
            else
            {
                xa = corner == ScrollBarCorner._7_Left_Top ? 0 : parentCtrl.Width - scrollbarWidth;
                ya = 0;
            }
            e.Graphics.FillRectangle(new SolidBrush(backgroundColor), xa, ya, scrollbarWidth, scrollbarWidth);

            // 2. Vertical
            e.Graphics.FillRectangle(new SolidBrush(backgroundColor), x, y, width, height);

            // 3. Arrows
            if(corner == ScrollBarCorner._1_Left_Bottom)
            {
                //=============================================
                // At verticality from 7 to 1
                //=============================================

                // Represents 7
                float x7 = 0;
                float y7 = 0;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x7, y7, scrollbarWidth, scrollbarWidth);
                pTop1 = new PointF(x7 + scrollbarWidth / 2f, y7 + scrollbarWidth * 1f / 3f);
                pTop2 = new PointF(x7 + scrollbarWidth * 1f / 3f, y7 + scrollbarWidth * 2f / 3f);
                pTop3 = new PointF(x7 + scrollbarWidth * 2f / 3f, y7 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pTop1, pTop2, pTop3 });

                // Represents 1
                float x1 = 0;
                float y1 = parentCtrl.Height - 2 * scrollbarWidth;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x1, y1, scrollbarWidth, scrollbarWidth);
                pBottom1 = new PointF(x1 + scrollbarWidth / 2f, y1 + scrollbarWidth * 2f / 3f);
                pBottom2 = new PointF(x1 + scrollbarWidth * 1f / 3f, y1 + scrollbarWidth * 1f / 3f);
                pBottom3 = new PointF(x1 + scrollbarWidth * 2f / 3f, y1 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pBottom1, pBottom2, pBottom3 });
            }
            else if(corner == ScrollBarCorner._3_Right_Bottom)
            {
                //=============================================
                // At verticality from 9 to 3
                //=============================================

                // Represents 9
                float x9 = parentCtrl.Width - scrollbarWidth;
                float y9 = 0;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x9, y9, scrollbarWidth, scrollbarWidth);
                pTop1 = new PointF(x9 + scrollbarWidth / 2f, y9 + scrollbarWidth * 1f / 3f);
                pTop2 = new PointF(x9 + scrollbarWidth * 1f / 3f, y9 + scrollbarWidth * 2f / 3f);
                pTop3 = new PointF(x9 + scrollbarWidth * 2f / 3f, y9 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pTop1, pTop2, pTop3 });

                // Represents 3
                float x3 = parentCtrl.Width - scrollbarWidth;
                float y3 = parentCtrl.Height - 2 * scrollbarWidth;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x3, y3, scrollbarWidth, scrollbarWidth);
                pBottom1 = new PointF(x3 + scrollbarWidth / 2f, y3 + scrollbarWidth * 2f / 3f);
                pBottom2 = new PointF(x3 + scrollbarWidth * 1f / 3f, y3 + scrollbarWidth * 1f / 3f);
                pBottom3 = new PointF(x3 + scrollbarWidth * 2f / 3f, y3 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pBottom1, pBottom2, pBottom3 });
            }
            else if (corner == ScrollBarCorner._7_Left_Top)
            {
                //=============================================
                // At verticality from 7 to 1
                //=============================================

                // Represents 7
                float x7 = 0;
                float y7 = scrollbarWidth;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x7, y7, scrollbarWidth, scrollbarWidth);
                pTop1 = new PointF(x7 + scrollbarWidth / 2f, y7 + scrollbarWidth * 1f / 3f);
                pTop2 = new PointF(x7 + scrollbarWidth * 1f / 3f, y7 + scrollbarWidth * 2f / 3f);
                pTop3 = new PointF(x7 + scrollbarWidth * 2f / 3f, y7 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pTop1, pTop2, pTop3 });

                // Represents 1
                float x1 = 0;
                float y1 = parentCtrl.Height - scrollbarWidth;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x1, y1, scrollbarWidth, scrollbarWidth);
                pBottom1 = new PointF(x1 + scrollbarWidth / 2f, y1 + scrollbarWidth * 2f / 3f);
                pBottom2 = new PointF(x1 + scrollbarWidth * 1f / 3f, y1 + scrollbarWidth * 1f / 3f);
                pBottom3 = new PointF(x1 + scrollbarWidth * 2f / 3f, y1 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pBottom1, pBottom2, pBottom3 });
            }
            else if (corner == ScrollBarCorner._9_Right_Top)
            {
                //=============================================
                // At verticality from 9 to 3
                //=============================================

                // Represents 9
                float x9 = parentCtrl.Width - scrollbarWidth;
                float y9 = scrollbarWidth;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x9, y9, scrollbarWidth, scrollbarWidth);
                pTop1 = new PointF(x9 + scrollbarWidth / 2f, y9 + scrollbarWidth * 1f / 3f);
                pTop2 = new PointF(x9 + scrollbarWidth * 1f / 3f, y9 + scrollbarWidth * 2f / 3f);
                pTop3 = new PointF(x9 + scrollbarWidth * 2f / 3f, y9 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pTop1, pTop2, pTop3 });

                // Represents 3
                float x3 = parentCtrl.Width - scrollbarWidth;
                float y3 = parentCtrl.Height - scrollbarWidth;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x3, y3, scrollbarWidth, scrollbarWidth);
                pBottom1 = new PointF(x3 + scrollbarWidth / 2f, y3 + scrollbarWidth * 2f / 3f);
                pBottom2 = new PointF(x3 + scrollbarWidth * 1f / 3f, y3 + scrollbarWidth * 1f / 3f);
                pBottom3 = new PointF(x3 + scrollbarWidth * 2f / 3f, y3 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pBottom1, pBottom2, pBottom3 });
            }

            // 4. Vertical
            e.Graphics.DrawRectangle(new Pen(lineColor, lineWeight), x, y, width, height);
        }

        #endregion

        #region Horizontal

        private void DoHorizontal(System.Windows.Forms.PaintEventArgs e)
        {
            float x = 0, y = 0, width = parentCtrl.Width - scrollbarWidth, height = scrollbarWidth;
            PointF pLeft1, pLeft2, pLeft3;
            PointF pRight1, pRight2, pRight3;

            // Retrieve coordinates
            switch (corner)
            {
                case ScrollBarCorner._1_Left_Bottom:
                    x = scrollbarWidth;
                    y = parentCtrl.Height - scrollbarWidth;
                    break;
                case ScrollBarCorner._3_Right_Bottom:
                    x = 0;
                    y = parentCtrl.Height - scrollbarWidth;
                    break;
                case ScrollBarCorner._7_Left_Top:
                    x = scrollbarWidth;
                    y = 0;
                    break;
                case ScrollBarCorner._9_Right_Top:
                    x = 0;
                    y = 0;
                    break;
            }

            // Fill horizontal
            // 1. Corner (dropped)
            // 2. Horizontal
            e.Graphics.FillRectangle(new SolidBrush(backgroundColor), x, y, width, height);

            // 3. Arrows
            if (corner == ScrollBarCorner._1_Left_Bottom)
            {
                //=============================================
                // At horizontality from 1 to 3
                //=============================================

                // Represents 1
                float x1 = scrollbarWidth;
                float y1 = parentCtrl.Height - scrollbarWidth;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x1, y1, scrollbarWidth, scrollbarWidth);
                pLeft1 = new PointF(x1 + scrollbarWidth * 1f / 3f, y1 + scrollbarWidth / 2f);
                pLeft2 = new PointF(x1 + scrollbarWidth * 2f / 3f, y1 + scrollbarWidth * 2f / 3f);
                pLeft3 = new PointF(x1 + scrollbarWidth * 2f / 3f, y1 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pLeft1, pLeft2, pLeft3 });

                // Represents 3
                float x3 = parentCtrl.Width - scrollbarWidth;
                float y3 = parentCtrl.Height - scrollbarWidth;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x3, y3, scrollbarWidth, scrollbarWidth);
                pRight1 = new PointF(x3 + scrollbarWidth * 2f / 3f, y3 + scrollbarWidth / 2f);
                pRight2 = new PointF(x3 + scrollbarWidth * 1f / 3f, y3 + scrollbarWidth * 1f / 3f);
                pRight3 = new PointF(x3 + scrollbarWidth * 1f / 3f, y3 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pRight1, pRight2, pRight3 });
            }
            else if (corner == ScrollBarCorner._3_Right_Bottom)
            {
                //=============================================
                // At horizontality from 1 to 3
                //=============================================

                // Represents 1
                float x1 = 0;
                float y1 = parentCtrl.Height - scrollbarWidth;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x1, y1, scrollbarWidth, scrollbarWidth);
                pLeft1 = new PointF(x1 + scrollbarWidth * 1f / 3f, y1 + scrollbarWidth / 2f);
                pLeft2 = new PointF(x1 + scrollbarWidth * 2f / 3f, y1 + scrollbarWidth * 2f / 3f);
                pLeft3 = new PointF(x1 + scrollbarWidth * 2f / 3f, y1 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pLeft1, pLeft2, pLeft3 });

                // Represents 3
                float x3 = parentCtrl.Width - 2 * scrollbarWidth;
                float y3 = parentCtrl.Height - scrollbarWidth;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x3, y3, scrollbarWidth, scrollbarWidth);
                pRight1 = new PointF(x3 + scrollbarWidth * 2f / 3f, y3 + scrollbarWidth / 2f);
                pRight2 = new PointF(x3 + scrollbarWidth * 1f / 3f, y3 + scrollbarWidth * 1f / 3f);
                pRight3 = new PointF(x3 + scrollbarWidth * 1f / 3f, y3 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pRight1, pRight2, pRight3 });
            }
            else if (corner == ScrollBarCorner._7_Left_Top)
            {
                //=============================================
                // At horizontality from 7 to 9
                //=============================================

                // Represents 7
                float x7 = scrollbarWidth;
                float y7 = 0;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x7, y7, scrollbarWidth, scrollbarWidth);
                pLeft1 = new PointF(x7 + scrollbarWidth * 1f / 3f, y7 + scrollbarWidth / 2f);
                pLeft2 = new PointF(x7 + scrollbarWidth * 2f / 3f, y7 + scrollbarWidth * 2f / 3f);
                pLeft3 = new PointF(x7 + scrollbarWidth * 2f / 3f, y7 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pLeft1, pLeft2, pLeft3 });

                // Represents 9
                float x9 = parentCtrl.Width - scrollbarWidth;
                float y9 = 0;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x9, y9, scrollbarWidth, scrollbarWidth);
                pRight1 = new PointF(x9 + scrollbarWidth * 2f / 3f, y9 + scrollbarWidth / 2f);
                pRight2 = new PointF(x9 + scrollbarWidth * 1f / 3f, y9 + scrollbarWidth * 1f / 3f);
                pRight3 = new PointF(x9 + scrollbarWidth * 1f / 3f, y9 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pRight1, pRight2, pRight3 });
            }
            else if (corner == ScrollBarCorner._9_Right_Top)
            {
                //=============================================
                // At horizontality from 7 to 9
                //=============================================

                // Represents 7
                float x7 = 0;
                float y7 = 0;
                // At top
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x7, y7, scrollbarWidth, scrollbarWidth);
                pLeft1 = new PointF(x7 + scrollbarWidth * 1f / 3f, y7 + scrollbarWidth / 2f);
                pLeft2 = new PointF(x7 + scrollbarWidth * 2f / 3f, y7 + scrollbarWidth * 2f / 3f);
                pLeft3 = new PointF(x7 + scrollbarWidth * 2f / 3f, y7 + scrollbarWidth * 1f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pLeft1, pLeft2, pLeft3 });

                // Represents 9
                float x9 = parentCtrl.Width - 2 * scrollbarWidth;
                float y9 = 0;
                // At bottom
                e.Graphics.FillRectangle(new SolidBrush(elementsColor), x9, y9, scrollbarWidth, scrollbarWidth);
                pRight1 = new PointF(x9 + scrollbarWidth * 2f / 3f, y9 + scrollbarWidth / 2f);
                pRight2 = new PointF(x9 + scrollbarWidth * 1f / 3f, y9 + scrollbarWidth * 1f / 3f);
                pRight3 = new PointF(x9 + scrollbarWidth * 1f / 3f, y9 + scrollbarWidth * 2f / 3f);
                e.Graphics.FillPolygon(new SolidBrush(foregroundColor), new PointF[] { pRight1, pRight2, pRight3 });
            }

            // 4. Horizontal
            e.Graphics.DrawRectangle(new Pen(lineColor, lineWeight), x, y, width, height);
        }

        #endregion

    }
}
