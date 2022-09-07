namespace Tetris.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private WorkBlocksViewModel workBlocksVM;

        private GameGridViewModel gameGridVM;

        private ManageCanvasViewModel manageCanvasVM;

        public WorkBlocksViewModel WorkBlocksVM
        {
            get => workBlocksVM;
            set
            {
                workBlocksVM = value;
                OnPropertyChanged();
            }
        }

        public GameGridViewModel GameGridVM
        {
            get => gameGridVM;
            set
            {
                gameGridVM = value;
                OnPropertyChanged();

            }
        }

        public ManageCanvasViewModel ManageCanvasVM
        {
            get => manageCanvasVM;
            set
            {
                manageCanvasVM = value;
                OnPropertyChanged();

            }
        }

        public MainViewModel(WorkBlocksViewModel workBlocksViewModel, GameGridViewModel gameGridVM, ManageCanvasViewModel manageCanvasVM)
        {
            WorkBlocksVM = workBlocksViewModel;
            GameGridVM = gameGridVM;
            WorkBlocksVM.GameGridVM = GameGridVM;
            ManageCanvasVM = manageCanvasVM;
            ManageCanvasVM.GameGridVM = GameGridVM;
            ManageCanvasVM.WorkBlocksVM = WorkBlocksVM;
        }
    }
}
