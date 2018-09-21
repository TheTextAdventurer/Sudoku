using System;
using System.Linq;

namespace Sudoku.Classes.Solvers
{
    public class SolverSubtraction : SolverBase
    {
        public SolverSubtraction()
        {
            Name = "Subtraction";
            Description = "The value of solved cell can be removed as a potenital value of it's neighbours in the same row, column and box.";
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
