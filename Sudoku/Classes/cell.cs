using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Classes
{

    public class Cell
    {
        public int Row { get; private set; }
        public int Col { get; private set; }
        public int Box { get; private set; }
        public bool HasChanged { get; set; }
        public bool Selected { get; private set; }
        public int[] Values { get; private set; }
        public bool Solved { get { return Values.Length == 1; } }

        public void Select (bool IsSelected)
        {
            Selected = IsSelected;
            Changed?.Invoke(this);
        }

        public void Reset()
        {
            Values = new int[] { 1,2,3,4,5,6,7,8,9 };
            HasChanged = true;
            Changed?.Invoke(this);
        }

        public void SetValue(int pVal)
        {
            Values = new int[] { pVal };
            HasChanged = true;
            Changed?.Invoke(this);
        }

        public void RemoveValue(params int[] pValues)
        {
            Values = Values.Except(pValues).ToArray();
            if (Values.Length == 0)
                throw new Exception("Empty cell " + this.ToString());
            HasChanged = true;
            Changed?.Invoke(this);            
        }

        public Cell(int pVal, int pInd)
        {
            if (pVal > 0)
                Values = new int[] { pVal };
            else
                Values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
               
            Row = pInd / 9;
            Col = pInd - Row * 9;
            Box = Col / 3 + ((Row / 3) * 3);
        }

        public override string ToString()
        {
            return string.Format("{0},{1}: {2}", Col, Row, String.Join("", Values.Select(i => i.ToString())));
        }

        public delegate void eChanged(Cell sender);
        public event eChanged Changed;
    }
}
