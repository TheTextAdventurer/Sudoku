using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Classes
{
    public class SolverSubtraction : SolverBase
    {
        public SolverSubtraction()
        {
            Description = "Subtraction";
        }
            

        public override bool Tick()
        {
            foreach (var cell in cells.Where(c => c.Solved))
            {
                Console.WriteLine(cell);

                var neighours = GetAllNeighbours(cell)
                                .Where(c => !c.Solved && c.Values.Contains(cell.Values.First()));

                if(neighours.Count() > 0)
                {
                    cell.Select(true);
                    neighours.All(c => { c.RemoveValue(cell.Values.First()); return true; });
                    MarkUnchanged();
                    return true;
                }
            }
            return false;
        }
    }
}
