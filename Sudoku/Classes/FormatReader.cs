using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Classes
{
    public static class FormatReader
    {
        //http://www.sudocue.net/fileformats.php


        public static int[] Open (FileTypes.FilterTypes fileType, string pFile)
        {
            int[] retval;
            string f = null;

            switch (fileType)
            {
                case FileTypes.FilterTypes.sdk:

                    f = File.ReadAllLines(pFile)
                            .SkipWhile(l => l.StartsWith("#"))
                            .Aggregate((a, b) => a + b);

                    retval = f.ToCharArray()
                                .Select(i => i == '.' ? 0 : (int)char.GetNumericValue(i))
                                .ToArray();

                    break;

                default:
                    throw new NotImplementedException("File type not implemented: " + fileType.ToString());
            }


            return retval;
        }
    }
}
