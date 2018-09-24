using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Sudoku.Classes
{
    //http://www.sudocue.net/fileformats.php
    public class SudokuWrapper
    {
        public string FileName { get { return Path.GetFileName(FullPath); ; } }

        [Browsable(false)]
        public string FullPath { get; set; }

        private int[] Nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        private int[] Vals = { 0, 1, 2, 3, 4, 5, 6, 7, 8};

        [Browsable(false)]
        public List<Cell> Cells { get; private set; } = new List<Cell>();
        public int SolvedCells { get { return Cells.Count(c => c.Solved); } }
        public int UnsolvedCells { get { return Cells.Count(c => !c.Solved); } }

        public string GetAsFormat(FileTypes.FilterTypes pSaveAs)
        {
            StringBuilder sb = new StringBuilder(); 

            switch (pSaveAs)
            {
                case FileTypes.FilterTypes.sdk:

                    foreach (var row in GetAsRows())
                        sb.AppendLine(String.Join("", row.Select(c => c.Solved ? c.Values.First().ToString() : ".")));
                    break;

                default:
                    throw new NotImplementedException("File type not implemented: " + pSaveAs.ToString());
            }

            return sb.ToString();
        }

        private Cell[][] GetAsRows()
        {
            return Vals.Select(v => Cells.Where(c => c.Row == v).OrderBy(c1 => c1.Col).ToArray()).ToArray();
                                        
        }

        public SudokuWrapper()
        {
            for (int ctr = 0; ctr < 81; ctr++)
            {
                Cells.Add(new Cell(0, ctr));
                Cells.Last().Changed += SudokuWrapper_Changed;
            }
        }

        public SudokuWrapper(int[] pValues)
        {
            for (int ctr = 0; ctr < pValues.Length; ctr++)
            {
                Cells.Add(new Cell(pValues[ctr], ctr));
                Cells.Last().Changed += SudokuWrapper_Changed;
            }
        }

        public bool IsSolved()
        {
            if (Cells.Count(c => c.Solved) != Cells.Count())
                return false;

            for (int i = 0; i < 9; i++)
            {
                if (
                        Cells.Where(c => c.Col == i)
                                .SelectMany(c => c.Values)
                                .Intersect(Nums)
                                .Count() != 9
                    )
                    return false;

                if (
                        Cells.Where(c => c.Row == i)
                                .SelectMany(c => c.Values)
                                .Intersect(Nums)
                                .Count() != 9
                    )
                    return false;

                if (
                        Cells.Where(c => c.Box == i)
                                .SelectMany(c => c.Values)
                                .Intersect(Nums)
                                .Count() != 9
                    )
                    return false;
            }

            return true;
        }

        public void ResetCell(int row, int col)
        {
            Cells.First(c => c.Row == row && c.Col == col)
                .Reset();
        }

        public void SetCell(int row, int col, int val)
        {
            Cells.First(c => c.Row == row && c.Col == col)
                .SetValue(val);
        }

        public void Refresh()
        {
            Cells.All(c => { Changed?.Invoke(c, false, false, true); return true; });
        }

        private void SudokuWrapper_Changed(Cell sender)
        {
            Changed?.Invoke(sender, sender.HasChanged, sender.Selected, sender.HasChanged);
        }

        public Cell GetCell(int X, int Y) => Cells.First(c => c.Col == X && c.Row == Y);

        public delegate void eChanged(Cell sender, bool blink, bool selected, bool haschanged);
        public event eChanged Changed;

    }
}
