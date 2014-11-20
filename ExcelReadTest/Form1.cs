using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelReadTest
{
    public partial class Form1 : Form
    {
        private Row[] _rows = null;
        private int _rowCount = 0;
        private TextBox[] _textBoxes = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            if (_textBoxes != null)
            {
                _rowCount = 0;
                foreach (var textbox in _textBoxes)
                    this.Controls.Remove(textbox);
                _textBoxes = null;
                _rows = null;
            }

            openFileDialog1.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileOpened = openFileDialog1.FileName;
                try
                {
                    filePath.Text = fileOpened;
                    filePath.Refresh();
                    var worksheets = Workbook.Worksheets(fileOpened);
                    var firstWorksheet = worksheets.FirstOrDefault();
                    _rows = firstWorksheet.Rows;

                    if (_rows != null && _rows.Length > 1)
                    {
                        nextButton.Enabled = true;
                        var pt1 = 42;
                        var colCount = _rows[0].Cells.Length;
                        _textBoxes = new TextBox[colCount];
                        for (var i = 0; i < colCount; i++)
                        {
                            TextBox newBox = new TextBox
                            {
                                Name = "textbox" + i.ToString(),
                                Location = new System.Drawing.Point(13, pt1),
                                Size = new System.Drawing.Size(400, 20),
                                TabIndex = i + 2,
                            };
                            this.Controls.Add(newBox);
                            _textBoxes[i] = newBox;
                            pt1 += 25;
                        }
                        nextButton_Click(this, e);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            var thisRow = _rows[_rowCount];
            var colCount = thisRow.Cells.Length;

            for (var i = 0; i < colCount; i++)
            {
                _textBoxes[i].Text = thisRow.Cells[i] != null ? thisRow.Cells[i].Text : "";
                _textBoxes[i].Refresh();
            }
            _rowCount++;

            if (_rows.Length <= _rowCount)
            {
                //this.Controls.Remove(nextButton);
                nextButton.Enabled = false;
            }
        }
    }
}
