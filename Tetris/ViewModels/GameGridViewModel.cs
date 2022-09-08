using System.Windows.Controls;

namespace Tetris.ViewModels
{
    internal class GameGridViewModel : BaseViewModel
    {

        public int[,] Grid { get; set; }

        public bool SizeCanvasVisibility
        {
            get => sizeCanvasVisibility;
            set
            {
                sizeCanvasVisibility = value;
                OnPropertyChanged();

            }
        }
        private bool sizeCanvasVisibility = true;

        public int Rows
        {
            get => rows;
            set
            {
                rows = value;
                OnPropertyChanged();
            }
        }
        private int rows;

        public int Columns
        {
            get => columns;
            set
            {
                columns = value;
                OnPropertyChanged();
            }
        }
        private int columns;

        public int this[int r, int c]
        {
            get => Grid[r, c];
            set => Grid[r, c] = value;
        }

        public bool IsInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && Grid[r, c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (Grid[r, c] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (Grid[r, c] != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                Grid[r, c] = 0;
            }
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                Grid[r + numRows, c] = Grid[r, c];
                Grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }

            return cleared;
        }

        
    }
}
