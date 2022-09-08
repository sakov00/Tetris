using System;
using System.Threading.Tasks;
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

        private Image[,] imageControls;

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

        public bool StartVisibility
        {
            get => startVisibility;
            set
            {
                startVisibility = value;
                OnPropertyChanged();

            }
        }
        private bool startVisibility = true;

        public GameGridViewModel GameGridVM { get; set; }

        public WorkBlocksViewModel WorkBlocksVM { get; set; }

        public ManageCanvasViewModel()
        {
            GameGridVM = new GameGridViewModel();
            WorkBlocksVM = new WorkBlocksViewModel(GameGridVM);
            GameCanvas = new RelayCommand(GameCanvas_Executed);
            PlayAgain = new RelayCommand(PlayAgain_Executed);
            MoveBlockLeft = new RelayCommand(MoveBlockLeft_Executed);
            MoveBlockRight = new RelayCommand(MoveBlockRight_Executed);
            MoveBlockDown = new RelayCommand(MoveBlockDown_Executed);
            RotateBlockCW = new RelayCommand(RotateBlockCW_Executed);
            RotateBlockCCW = new RelayCommand(RotateBlockCCW_Executed);
            DropBlock = new RelayCommand(DropBlock_Executed);
        }

        #region Commands

        #region --- GameCanvas ---

        public ICommand GameCanvas { get; private set; }

        private async void GameCanvas_Executed(object param)
        {
            SetupGameCanvas(GameGridVM, (Canvas)param);
            await GameLoop();
        }

        #endregion --- GameCanvas ---

        #region --- PlayAgain ---

        public ICommand PlayAgain { get; private set; }

        private void PlayAgain_Executed(object param)
        {
            GameGridVM = new GameGridViewModel();
            WorkBlocksVM.GameGridVM = GameGridVM;
            WorkBlocksVM.GameOver = false;
            GameGridVM.Grid = new int[GameGridVM.Rows, GameGridVM.Columns];
            WorkBlocksVM.Score = 0;
            GameLoop();
        }

        #endregion --- PlayAgain ---

        #region --- MoveBlockLeft ---

        public ICommand MoveBlockLeft { get; private set; }

        private void MoveBlockLeft_Executed(object param)
        {
            WorkBlocksVM.MoveBlockLeft();
            Draw();
        }

        #endregion --- MoveBlockLeft ---

        #region --- MoveBlockRight ---

        public ICommand MoveBlockRight { get; private set; }

        private void MoveBlockRight_Executed(object param)
        {
            WorkBlocksVM.MoveBlockRight();
            Draw();
        }

        #endregion --- MoveBlockRight ---

        #region --- MoveBlockDown ---

        public ICommand MoveBlockDown { get; private set; }

        private void MoveBlockDown_Executed(object param)
        {
            WorkBlocksVM.MoveBlockDown();
            Draw();
        }

        #endregion --- MoveBlockRight ---

        #region --- RotateBlockCW ---

        public ICommand RotateBlockCW { get; private set; }

        private void RotateBlockCW_Executed(object param)
        {
            WorkBlocksVM.RotateBlockCW();
            Draw();
        }

        #endregion --- RotateBlockCW ---

        #region --- RotateBlockCCW ---

        public ICommand RotateBlockCCW { get; private set; }

        private void RotateBlockCCW_Executed(object param)
        {
            WorkBlocksVM.RotateBlockCCW();
            Draw();
        }

        #endregion --- RotateBlockCCW ---

        #region --- DropBlock ---

        public ICommand DropBlock { get; private set; }

        private void DropBlock_Executed(object param)
        {
            WorkBlocksVM.DropBlock();
            Draw();
        }

        #endregion --- DropBlock ---

        #endregion Commands

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

                    Canvas.SetTop(imageControl, r * cellSize);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    gameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            gameCanvas.Width = cellSize * grid.Columns;
            gameCanvas.Height = cellSize * grid.Rows;
            StartVisibility = false;
            grid.Grid = new int[grid.Rows, grid.Columns];
            return imageControls;
        }

        private async Task GameLoop()
        {
            Draw();

            while (!WorkBlocksVM.GameOver)
            {
                await Task.Delay(500);
                WorkBlocksVM.MoveBlockDown();
                Draw();
            }
            WorkBlocksVM.GameOver = true;
        }

        private void Draw()
        {
            if (imageControls == null)
                return;
            DrawGrid(GameGridVM);
            DrawGhostBlock(WorkBlocksVM.CurrentBlock);
            DrawBlock(WorkBlocksVM.CurrentBlock);
            DrawNextBlock(WorkBlocksVM);
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
    }
}
