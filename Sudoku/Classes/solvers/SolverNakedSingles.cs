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
            Description = "Naked Singles";
        }

        public override bool Tick()
        {
            foreach (var cell in cells.Where(c => !c.Solved))
            {

                Cell[] neigbours = null;

                if ((neigbours = GetRowNeighbours(cell).Where(c => c.Solved).ToArray()).Count() == 8)
                {
                    doOperation(neigbours, cell);
                    return true;
                }
                else if ((neigbours = GetColNeighbours(cell).Where(c => c.Solved).ToArray()).Count() == 8)
                {
                    doOperation(neigbours, cell);
                    return true;
                }
                else if ((neigbours = GetBoxNeighbours(cell).Where(c => c.Solved).ToArray()).Count() == 8)
                {
                    doOperation(neigbours, cell);
                    return true;
                }
                else if ((neigbours = GetAllNeighbours(cell).Where(c => c.Solved).ToArray()).Count() == 8)
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
