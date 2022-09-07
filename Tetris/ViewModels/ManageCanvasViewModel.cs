using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris.Commands;
using Tetris.Models;

namespace Tetris.ViewModels
{
    class ManageCanvasViewModel : BaseViewModel
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileEmpty.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileCyan.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileBlue.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileOrange.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileYellow.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileGreen.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TilePurple.png", UriKind.Absolute)),
            new BitmapImage(new Uri(@"C:\Work\Tetris\Tetris\Assets\TileRed.png", UriKind.Absolute))
        };

        private readonly string[] blockImages = new string[]
        {
            @"C:\Work\Tetris\Tetris\Assets\Block-Empty.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-I.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-J.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-L.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-O.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-S.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-T.png",
            @"C:\Work\Tetris\Tetris\Assets\Block-Z.png",
        };

        private Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

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

        public string HoldImage
        {
            get => holdImage;
            set
            {
                holdImage = value;
                OnPropertyChanged();

            }
        }
        private string holdImage;

        public bool GameOverVisibility
        {
            get => gameOverVisibility;
            set
            {
                gameOverVisibility = value;
                OnPropertyChanged();

            }
        }
        private bool gameOverVisibility;

        public string FinalScoreText
        {
            get => finalScoreText;
            set
            {
                finalScoreText = value;
                OnPropertyChanged();

            }
        }
        private string finalScoreText;

        public GameGridViewModel GameGridVM { get; set; }

        public WorkBlocksViewModel WorkBlocksVM { get; set; }

        public ManageCanvasViewModel()
        {
            GameCanvasCommand = new RelayCommand(GameCanvas_Executed);
            PlayAgainCommand = new RelayCommand(PlayAgain_Executed);
        }

        private Image[,] SetupGameCanvas(GameGridViewModel grid, Canvas gameCanvas)
        {
            imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    gameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }
        #region Commands

        #region --- GameCanvas ---

        public ICommand GameCanvasCommand { get; private set; }

        private async void GameCanvas_Executed(object param)
        {
            SetupGameCanvas(GameGridVM, (Canvas)param);
            await GameLoop();
        }

        #endregion --- GameCanvas ---

        #region --- PlayAgain ---

        public ICommand PlayAgainCommand { get; private set; }

        private async void PlayAgain_Executed(object param)
        {
            GameOverVisibility = false;
            await GameLoop();
        }

        #endregion --- PlayAgain ---

        #endregion Commands

        private async Task GameLoop()
        {
            Draw();

            while (!WorkBlocksVM.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (WorkBlocksVM.Score * delayDecrease));
                await Task.Delay(delay);
                WorkBlocksVM.MoveBlockDown.Execute(null);
                Draw();
            }

            GameOverVisibility = true;
            FinalScoreText = $"Score: {WorkBlocksVM.Score}";
        }

        private void Draw()
        {
            DrawGrid(GameGridVM);
            DrawGhostBlock(WorkBlocksVM.CurrentBlock);
            DrawBlock(WorkBlocksVM.CurrentBlock);
            DrawNextBlock(WorkBlocksVM);
            DrawHeldBlock(WorkBlocksVM.HeldBlock);
        }

        private void DrawGrid(GameGridViewModel grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = WorkBlocksVM.BlockDropDistance();

            foreach (Position p in WorkBlocksVM.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in WorkBlocksVM.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(WorkBlocksViewModel blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage = blockImages[0];
            }
            else
            {
                HoldImage = blockImages[heldBlock.Id];
            }
        }
    }
}
