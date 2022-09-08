using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tetris.Commands;

namespace Tetris.ViewModels
{
    class ManageCanvasViewModel : BaseViewModel
    {
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

        public DrawViewModel DrawVM { get; set; }

        public ManageCanvasViewModel(DrawViewModel drawVM)
        {
            DrawVM = drawVM;
            GameCanvas = new RelayCommand(GameCanvas_Executed);
            PlayAgain = new RelayCommand(PlayAgain_Executed);
            MoveBlockLeft = new RelayCommand(MoveBlockLeft_Executed, (param) => DrawVM.CurrentBlock!= null);
            MoveBlockRight = new RelayCommand(MoveBlockRight_Executed, (param) => DrawVM.CurrentBlock != null);
            MoveBlockDown = new RelayCommand(MoveBlockDown_Executed, (param) => DrawVM.CurrentBlock != null);
            RotateBlockCW = new RelayCommand(RotateBlockCW_Executed, (param) => DrawVM.CurrentBlock != null);
            RotateBlockCCW = new RelayCommand(RotateBlockCCW_Executed, (param) => DrawVM.CurrentBlock != null);
            DropBlock = new RelayCommand(DropBlock_Executed, (param) => DrawVM.CurrentBlock != null);
        }

        #region Commands

        #region --- GameCanvas ---

        public ICommand GameCanvas { get; private set; }

        private void GameCanvas_Executed(object param)
        {
            DrawVM.SetupGameCanvas((Canvas)param);
            StartVisibility = false;
            DrawVM.GameGridVM.SizeCanvasVisibility = false;
            DrawVM.NextBlock = DrawVM.RandomBlock();
            DrawVM.CurrentBlock = DrawVM.RandomBlock();
            GameLoop();
        }

        #endregion --- GameCanvas ---

        #region --- PlayAgain ---

        public ICommand PlayAgain { get; private set; }

        private void PlayAgain_Executed(object param)
        {
            DrawVM.GameOver = false;
            DrawVM.Score = 0;
            DrawVM.GameGridVM.Grid = new int[DrawVM.GameGridVM.Rows, DrawVM.GameGridVM.Columns];
            GameLoop();
        }

        #endregion --- PlayAgain ---

        #region --- MoveBlockLeft ---

        public ICommand MoveBlockLeft { get; private set; }

        private void MoveBlockLeft_Executed(object param)
        {
            DrawVM.MoveBlockLeft();
            DrawVM.Draw();
        }

        #endregion --- MoveBlockLeft ---

        #region --- MoveBlockRight ---

        public ICommand MoveBlockRight { get; private set; }

        private void MoveBlockRight_Executed(object param)
        {
            DrawVM.MoveBlockRight();
            DrawVM.Draw();
        }

        #endregion --- MoveBlockRight ---

        #region --- MoveBlockDown ---

        public ICommand MoveBlockDown { get; private set; }

        private void MoveBlockDown_Executed(object param)
        {
            DrawVM.MoveBlockDown();
            DrawVM.Draw();
        }

        #endregion --- MoveBlockRight ---

        #region --- RotateBlockCW ---

        public ICommand RotateBlockCW { get; private set; }

        private void RotateBlockCW_Executed(object param)
        {
            DrawVM.RotateBlockCW();
            DrawVM.Draw();
        }

        #endregion --- RotateBlockCW ---

        #region --- RotateBlockCCW ---

        public ICommand RotateBlockCCW { get; private set; }

        private void RotateBlockCCW_Executed(object param)
        {
            DrawVM.RotateBlockCCW();
            DrawVM.Draw();
        }

        #endregion --- RotateBlockCCW ---

        #region --- DropBlock ---

        public ICommand DropBlock { get; private set; }

        private void DropBlock_Executed(object param)
        {
            DrawVM.DropBlock();
            DrawVM.Draw();
        }

        #endregion --- DropBlock ---

        #endregion Commands

        private async Task GameLoop()
        {
            DrawVM.Draw();

            while (!DrawVM.GameOver)
            {
                await Task.Delay(500);
                DrawVM.MoveBlockDown();
                DrawVM.Draw();
            }
        }
    }
}
