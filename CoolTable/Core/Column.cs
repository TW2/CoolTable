using CoolTable.Renderer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CoolTable.Core
{
    public class Column
    {
        private string title = "Column Header";
        private bool hasHeader = false;
        private Color headerBackground = Color.LightGray;
        private Color headerBackgroundWhenSelected = Color.Gray;
        private Color headerForeground = Color.Black;
        private Color headerForegroundWhenSelected = Color.White;
        private Type type = typeof(object);
        private bool hasMultiType = false;
        private int columnIndex = -1;
        private bool isLineNumberColumn = false;
        private float columnWidth = 150f;

        private bool canEdit = false;

        private IRenderer renderer = new StringRenderer();


        public Column()
        {
        }

        public string Title { get => title; set => title = value; }
        public bool HasHeader { get => hasHeader; set => hasHeader = value; }
        public Color HeaderBackground { get => headerBackground; set => headerBackground = value; }
        public Color HeaderBackgroundWhenSelected { get => headerBackgroundWhenSelected; set => headerBackgroundWhenSelected = value; }
        public Color HeaderForeground { get => headerForeground; set => headerForeground = value; }
        public Color HeaderForegroundWhenSelected { get => headerForegroundWhenSelected; set => headerForegroundWhenSelected = value; }
        public Type Type { get => type; set => type = value; }
        public bool HasMultiType { get => hasMultiType; set => hasMultiType = value; }
        public int ColumnIndex { get => columnIndex; set => columnIndex = value; }
        public bool IsLineNumberColumn { get => isLineNumberColumn; set => isLineNumberColumn = value; }
        public float ColumnWidth { get => columnWidth; set => columnWidth = value; }

        public bool CanEdit { get => canEdit; set => canEdit = value; }

        public IRenderer Renderer { get => renderer; set => renderer = value; }


        /// <summary>
        /// Create a new column with auto-loader faculties!
        /// </summary>
        /// <param name="type">Class determining by typeof(object) which automatically loads known renderer.</param>
        /// <param name="headerTitle">Title of header. Empty -> no header (if same on each column).</param>
        /// <param name="width">Width. Default is 150 pixels.</param>
        /// <param name="canEdit">If object has editor and true then we can modify otherwise nothing will happens.</param>
        /// <returns>A semi configured Column.</returns>
        public static Column Create(Type type, string headerTitle = "", float width = 150f, bool canEdit = false)
        {
            Column c = new Column();

            c.Type = type == null ? typeof(object) : type;
            c.Title = headerTitle == "" ? "# >_< #" : headerTitle;
            c.HasHeader = headerTitle != "";
            c.ColumnWidth = width;
            c.canEdit = canEdit;

            if (type == typeof(object)) { c.Renderer = new StringRenderer(); } // <- TODO AdaptiveRenderer
            if (type == typeof(string)) { c.Renderer = new StringRenderer(); }

            return c;
        }

        private List<object> data = new List<object>();

        public int DataCount { get => data.Count; }

        public void AddValue(object value)
        {
            //Search for placeholder
            //if (data.Count == 1 && PlaceHolder == true)
            //{
            //    data.Clear(); PlaceHolder = false;
            //}

            //Add data
            data.Add(value);
        }

        public void SetValueAt(object value, int index)
        {
            if (data.Count - 1 >= index)
            {
                data.RemoveAt(index);
                data.Insert(index, value);
            }
        }

        public object GetValueAt(int index)
        {
            try
            {
                if (data.Count - 1 >= index)
                {
                    return data[index];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //Case when click on a line then index == -1
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public object[] ToArray()
        {
            if (data.Count > 0)
            {
                return data.ToArray();
            }
            //else
            //{
            //    data.Add(""); PlaceHolder = true;
            //    return data.ToArray();
            //}
            return null;
        }

        public void Clear()
        {
            data.Clear();
        }

        public void RemoveValueAt(int index)
        {
            if (index > 0 && index < data.Count)
            {
                data.RemoveAt(index);
            }
        }
    }
}
