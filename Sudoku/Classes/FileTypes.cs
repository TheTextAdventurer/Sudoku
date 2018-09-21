namespace Sudoku
{
    public static class FileTypes
    {
        public enum FilterTypes  { sdk, sdm, sdx, scl, ss};

        public static string GetFilter { get { return string.Join("|", FileFilters); } }

        public static string[] FileFilters = {"sdk files (*.sdk)|*.sdk"
                                    , "sdm files (*.sdm)|*.sdm"
                                    , "sdx files (*.sdx)|*.sdx"
                                    , "scl files (*.scl)|*.scl"
                                    , "ss files (*.ss)|*.ss" };

    }
}
