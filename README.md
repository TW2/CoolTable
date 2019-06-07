# CoolTable
A simple table with something else !
-
Wanted functionnalities :

* Renderer for one base type - OK (String)
* Renderer for String - OK
* Renderer for Boolean - not implemented
* Renderer for Integer - not implemented
* Renderer for Font - not implemented
* Renderer for ComboBox (visible entry) - not implemented

others...

Example :

<img src="https://github.com/TW2/CoolTable/blob/master/screenshots/v6%20-%20002.PNG" />

```c#
public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            table1.AddLineNumberColumn();
            table1.AddColumn(Column.Create(typeof(string), "A"));
            table1.AddColumn(Column.Create(typeof(string), "B"));
            table1.AddColumn(Column.Create(typeof(string), "C"));

            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
            table1.AddRow(new object[] { "", "Bananas", "35€", "1kg" });
        }
    }
```
