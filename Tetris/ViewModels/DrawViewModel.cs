using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris.Models;

namespace Tetris.ViewModels
{
    class DrawViewModel : WorkBlocksViewModel
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("/Tetris;component/Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Tetris;component/Assets/TileRed.png", UriKind.Relative))
        };

        private readonly string[] blockImages = new string[]
        {
            "/Tetris;component/Assets/Block-Empty.png",
            "/Tetris;component/Assets/Block-I.png",
            "/Tetris;component/Assets/Block-J.png",
            "/Tetris;component/Assets/Block-L.png",
            "/Tetris;component/Assets/Block-O.png",
            "/Tetris;component/Assets/Block-S.png",
            "/Tetris;component/Assets/Block-T.png",
            "/Tetris;component/Assets/Block-Z.png",
            "/Tetris;component/Assets/Block-Empty.png"
        };

        public Image[,] ImageControls { get; set; }

        public string NextImage
        {
            get => nextImage;
            set
            {
                nextImage = value;
                OnPropertyChanged();

            }
        }
        private string nextImage;

        public DrawViewModel(GameGridViewModel gameGridVM)
        {
            GameGridVM = gameGridVM;
        }

        public void SetupGameCanvas(Canvas gameCanvas)
        {
            GameGridVM.Grid = new int[GameGridVM.Rows, GameGridVM.Columns];
            ImageControls = new Image[GameGridVM.Rows, GameGridVM.Columns];
            int cellSize = 25;

            for (int r = 0; r < GameGridVM.Rows; r++)
            {
                for (int c = 0; c < GameGridVM.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, r * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    gameCanvas.Children.Add(imageControl);
                    ImageControls[r, c] = imageControl;
                }
            }
            gameCanvas.Width = cellSize * GameGridVM.Columns;
            gameCanvas.Height = cellSize * GameGridVM.Rows;
        }

        public void Draw()
        {
            if (ImageControls == null)
                return;
            DrawGrid();
            DrawGhostBlock(CurrentBlock);
            DrawBlock(CurrentBlock);
            DrawNextBlock();
        }

        private void DrawGrid()
        {
            for (int r = 0; r < GameGridVM.Rows; r++)
            {
                for (int c = 0; c < GameGridVM.Columns; c++)
                {
                    int id = GameGridVM[r, c];
                    ImageControls[r, c].Opacity = 1;
                    ImageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = BlockDropDistance();

            foreach (Position p in TilePositions())
            {
                ImageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                ImageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in TilePositions())
            {
                ImageControls[p.Row, p.Column].Opacity = 1;
                ImageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock()
        {
            Block next = NextBlock;
            NextImage = blockImages[next.Id];
        }
    }
}
