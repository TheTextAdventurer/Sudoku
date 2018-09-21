using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Classes.Solvers
{
    public class SolverHiddenSingles : SolverBase
    {
        public SolverHiddenSingles()
        {
            Name = "Hidden Singles";
            Description = "When a cell is the only one in a group that can receive a number, it must contain that number.";
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

    
    

