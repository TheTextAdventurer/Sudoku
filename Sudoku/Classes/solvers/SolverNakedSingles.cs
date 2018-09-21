using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Classes
{
    //http://www.sudoku129.com/puzzles/tips_1.php
    public class SolverNakedSingles : SolverBase
    {
        public SolverNakedSingles()
        {
            Name = "Naked Singles";
            Description = "A number can be assigned to a cell when it is the only possibility, given the row, the column and the block that contain the cell in question.";
        }

        public override bool Tick()
        {
            foreach (var cell in cells.Where(c => !c.Solved))
            {
                Cell[] neigbours = null;
                if ((neigbours = GetAllNeighbours(cell).Where(c => c.Solved).ToArray()).Count() == 8)
                {
                    doOperation(neigbours, cell);
                    return true;
                }
            }
            return false;
        }

        private void doOperation(Cell[] neigbours, Cell currentCell)
        {
            currentCell.Select(true);
            currentCell.SetValue(nums.Except(neigbours.SelectMany(n => n.Values)).First());
            //not actually removing a value, just want to make them blink
            neigbours.All(c => { c.Select(true); return true; });
            MarkUnchanged();

        }
    }
}
