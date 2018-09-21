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
using Sudoku.Classes;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SudokuWrapper current = null;
        List<SolverBase> Solvers = new List<SolverBase>()
        {
            new SolverSubtraction()
            , new SolverNakedSingles()
        };

        private void Form1_Load(object sender, EventArgs e)
        {

            this.KeyPreview = true;




            //string[] f = File.ReadAllLines("p096_sudoku.txt");
            //List<int[]> Puzzles = new List<int[]>();
            //int grid = 1;
            //for (int ctr = 0; ctr < f.Length; ctr += 10)
            //{
            //    var p = f.Skip(ctr + 1)
            //                    .Take(9)
            //                    .Select(i => i.Replace('0','.'))
            //                    .ToArray();
            //    File.WriteAllLines(String.Format("Grid {0}.sdk", grid++), p );

            //}

            //current = new SudokuWrapper(Puzzles.First());


            foreach (var s in Solvers)
                listBox1.Items.Add(s.Name);

        }

        private void Current_Changed(Cell sender, bool blink, bool selected, bool haschanged)
        {
            sudokoGrid1.SetCell(sender.Row, sender.Col, blink, selected, haschanged, sender.Values);
        }

        private void SetGrid(string pFile, FileTypes.FilterTypes pFilt)
        {
            if (pFile != null)
            {
                current = new SudokuWrapper(FormatReader.Open(pFilt, pFile));
                current.FileName = pFile;
            }
            else
            {
                current = new SudokuWrapper();
            }

            
            propertyGrid1.SelectedObject = current;
            current.Changed += Current_Changed;
            sudokoGrid1.Reset();
            current.Refresh();

            foreach (var s in Solvers)
                s.Set(current.Cells);
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            var cell = current.Cells.First(c => !c.Solved);
            sudokoGrid1.SelectCell(cell.Row, cell.Col);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
                Solvers[listBox1.SelectedIndex].Tick();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetGrid(null, (FileTypes.FilterTypes)0);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Filter = FileTypes.GetFilter;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SetGrid(ofd.FileName, (FileTypes.FilterTypes)(ofd.FilterIndex - 1) );
                    
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = FileTypes.GetFilter;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, current.GetAsFormat((FileTypes.FilterTypes)(sfd.FilterIndex-1)));
                }

            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (current != null && char.IsDigit(e.KeyChar))
            {
                var p = sudokoGrid1.SelectedCell;
                if (e.KeyChar != '0')
                    current.SetCell(p.Y, p.X, (int)char.GetNumericValue(e.KeyChar));
                else
                    current.ResetCell(p.Y, p.X);
            }
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                lbSolverDescription.Text = Solvers[listBox1.SelectedIndex].Description;
            }
        }
    }
}
