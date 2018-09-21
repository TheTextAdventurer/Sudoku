using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp3
{
    /// <summary>
    /// Custom control to represent a sudoku grid. Each cell is a custom class
    /// DataGridViewSudokueCell contained in a custom column DataGridViewSudokuCellColumn
    /// </summary>
    public partial class SudokoGrid : UserControl
    {
        #region public properties

        public Color BlinkColour { get; set; } = Color.LightYellow;
        public int BlinkDelay { get; set; } = 500;
        public int BlinkFrequency { get; set; } = 150;

        public Point SelectedCell { get { return new Point(dataGridView1.SelectedCells[0].ColumnIndex, dataGridView1.SelectedCells[0].RowIndex); } }

        #endregion

        int cellSize = 50;

        public void Clear()
        {
            dataGridView1.ClearSelection();

            
        }

        

        public void Reset()
        {
            dataGridView1.ClearSelection();
            foreach (DataGridViewRow row in dataGridView1.Rows)
                foreach (DataGridViewSudokueCell cell in row.Cells)
                {
                    cell.Value = null;
                }
        }

        public void SelectCell(int row, int col)
        {
            dataGridView1.Rows[row].Cells[col].Selected = true;
        }

        private void BlinkCell(int row, int col)
        {
            dataGridView1.Invoke((MethodInvoker)delegate ()
            {
                dataGridView1.Rows[row].Cells[col].Style.BackColor = BlinkColour;
            });

            lock (_blinkData)
            {
                _blinkData.Add(new CellData(row,col));
            }
        }

        public void SetCell(int row, int col, bool blink, bool selected, bool haschanged, int[] values)
        {
            if (selected)
                dataGridView1.Rows[row].Cells[col].Selected = true;

            if (blink)
                BlinkCell(row, col);

            if (haschanged)
                dataGridView1[col, row].Value = values;
        }

        public SudokoGrid()
        {

            InitializeComponent();
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowTemplate.Height = cellSize;
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = false;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = dataGridView1.BackgroundColor;
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = dataGridView1.BackgroundColor;
            dataGridView1.ShowCellToolTips = false;

            for (int ctr = 0; ctr < 9; ctr++)
            {
                dataGridView1.Columns.Add(new DataGridViewSudokuCellColumn() { HeaderText = (ctr + 1).ToString(), Width = cellSize });
                dataGridView1.Columns[ctr].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }



            dataGridView1.Rows.Add(9);

            foreach (DataGridViewRow row in dataGridView1.Rows)
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);

            foreach (DataGridViewColumn column in dataGridView1.Columns)
                column.SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridView1.Width = 9 * cellSize + dataGridView1.RowHeadersWidth + 2;
            dataGridView1.Height = 9 * cellSize + dataGridView1.ColumnHeadersHeight + 2;

            //
            _gridBlinkThread = new Thread(new ThreadStart(GridBlinkThreadFunc))
            {
                IsBackground = true
            };
            _gridBlinkThread.Start();

        }




        #region cell blinking
        //The following code is taken from here
        //https://www.codeproject.com/Articles/20387/Cell-Blink-for-DataGridView
        private Thread _gridBlinkThread = null;
        private List<CellData> _blinkData  = new List<CellData>();

        class CellData
        {
            public int Row;
            public int Col;
            public DateTime Time = DateTime.Now;

            public CellData(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }

        private void GridBlinkThreadFunc()
        {
            while (true)
            {
                // Make a copy to avoid invalid operation exception while iterating through the map
                List<CellData> tempBlinkData;
                lock (_blinkData)
                {
                    tempBlinkData = new List<CellData>(_blinkData);
                }

                foreach (CellData data in tempBlinkData)
                {
                    TimeSpan elapsed = DateTime.Now - data.Time;
                    if (elapsed.TotalMilliseconds > BlinkDelay) 
                    {
                        if (dataGridView1.IsDisposed)
                            return;

                        dataGridView1.Invoke((MethodInvoker)delegate ()
                        {
                            dataGridView1.Rows[data.Row].Cells[data.Col]
                                .Style.BackColor = dataGridView1.Columns[data.Col]
                                .DefaultCellStyle.BackColor;
                        });

                        lock (_blinkData)
                        {
                            _blinkData.Remove(data);
                        }
                    }
                }

                Thread.Sleep(BlinkFrequency); 
            }
        }

        #endregion
    }

    public class DataGridViewSudokueCell : DataGridViewTextBoxCell
    {

        public DataGridViewSudokueCell()
        {

        }

        private int[] HighLight { get; set; }

        public void ValuesToHighlight(int[] pVal)
        {
            HighLight = pVal;
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates cellState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // Call the base class method to paint the default cell appearance.
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState,
                null, null, errorText, cellStyle,
                advancedBorderStyle, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border | DataGridViewPaintParts.ContentBackground | DataGridViewPaintParts.Focus);

            //check it's a int array
            if (value is System.Int32[])
            {

                using (StringFormat cellTextFormatting = new StringFormat()
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center
                })
                {
                    

                    var vals = value as System.Int32[];

                    if (vals.Count() == 1)//single value
                    {
                        if (HighLight!= null && HighLight.Contains(vals.First()))
                            graphics.FillRectangle(new SolidBrush(cellStyle.SelectionBackColor), cellBounds);

                        using (Brush fontBrush = new SolidBrush(cellStyle.ForeColor))
                        {

                            graphics.DrawString(
                                vals.First().ToString()
                                , cellStyle.Font
                                , fontBrush
                                , cellBounds
                                , cellTextFormatting
                                );
                        }
                    }
                    else //multiple values
                    {

                        using (Brush fontBrush = new SolidBrush(Color.LightGray))
                        {


                            float cellWidth = (float)cellBounds.Width / 3;
                            float cellHeight = (float)cellBounds.Height / 3;

                            for (int ctr = 0; ctr < vals.Length; ctr++)
                            {
                                int y = ctr / 3;
                                int x = ctr - (y * 3);

                                graphics.DrawString(
                                    vals[ctr].ToString()
                                    , cellStyle.Font
                                    , new SolidBrush(Color.LightGray)
                                    , new RectangleF(
                                                    cellBounds.X + x * cellWidth
                                                    , cellBounds.Y + y * cellHeight
                                                    , cellWidth
                                                    , cellHeight
                                                )
                                    , cellTextFormatting);
                            }
                        }
                    }
                }
            }


            // Retrieve the client location of the mouse pointer.
            Point cursorPosition =
                this.DataGridView.PointToClient(Cursor.Position);

            // If the mouse pointer is over the current cell, draw a custom border.
            if (cellBounds.Contains(cursorPosition)|| this.Selected)
            {
                Rectangle newRect = new Rectangle(cellBounds.X + 1,
                    cellBounds.Y + 1, cellBounds.Width - 4,
                    cellBounds.Height - 4);
                graphics.DrawRectangle(Pens.Red, newRect);
            }
        }

        // Force the cell to repaint itself when the mouse pointer enters it.
        protected override void OnMouseEnter(int rowIndex)
        {
            this.DataGridView.InvalidateCell(this);
        }

        // Force the cell to repaint itself when the mouse pointer leaves it.
        protected override void OnMouseLeave(int rowIndex)
        {
            this.DataGridView.InvalidateCell(this);
        }

    }

    public class DataGridViewSudokuCellColumn : DataGridViewColumn
    {
        public DataGridViewSudokuCellColumn()
        {
            this.CellTemplate = new DataGridViewSudokueCell();
        }
    }
}

