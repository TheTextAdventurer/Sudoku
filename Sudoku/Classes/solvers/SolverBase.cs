using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Classes.Solvers
{
    public abstract class SolverBase
    {
        protected List<Cell> cells;
        public string Name { get; set; }
        public string Description { get; set; }

        protected static int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        /// <summary>
        /// Get the neighbours of the provided cell,the same
        /// column, row and box
        /// </summary>
        /// <param name="pCell"></param>
        /// <returns></returns>
        protected Cell[] GetAllNeighbours(Cell pCell)
        {
            return cells.Where(c => c != pCell)
                        .Where(c =>
                                    c.Row == pCell.Row
                                    || c.Col == pCell.Col
                                    || c.Box == pCell.Box
                                ).ToArray();
        }

        protected Cell[] GetRowNeighbours(Cell pCell)
        {
            return GetAllNeighbours(pCell)
                    .Where(c => c.Row == pCell.Row)
                    .ToArray();
        }

        protected Cell[] GetColNeighbours(Cell pCell)
        {
            return GetAllNeighbours(pCell)
                    .Where(c => c.Col == pCell.Col)
                    .ToArray();
        }

        protected Cell[] GetBoxNeighbours(Cell pCell)
        {
            return GetAllNeighbours(pCell)
                    .Where(c => c.Box == pCell.Box)
                    .ToArray();
        }

        /// <summary>
        /// set the change flag of any changed cells to false
        /// </summary>
        protected void MarkUnchanged()
        {
            cells.Where(c => c.HasChanged)
                    .All(c => { c.HasChanged = false; return true; });

            cells.Where(c => c.Selected)
                    .All(c => { c.Select(false); return true; });
        }

        public void Set(List<Cell> pCells)
        {
            cells = pCells;
        }

        public abstract bool Tick();
    }
}
