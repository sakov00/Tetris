namespace Tetris.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private BlockQueueViewModel blockQueueVM;

        private WorkBlocksViewModel workBlocksVM;

        private GameStateViewModel gameStateVM;

        private GameGridViewModel gameGridVM;

        public BlockQueueViewModel BlockQueueVM
        {
            get => blockQueueVM;
            set
            {
                blockQueueVM = value;
                OnPropertyChanged();
            }
        }

        public WorkBlocksViewModel WorkBlocksVM
        {
            get => workBlocksVM;
            set
            {
                workBlocksVM = value;
                OnPropertyChanged();
            }
        }


        public GameStateViewModel GameStateVM
        {
            get => gameStateVM;
            set
            {
                gameStateVM = value;
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

        public MainViewModel(BlockQueueViewModel blockQueueVM,
            WorkBlocksViewModel workBlocksViewModel, GameStateViewModel gameStateVM, GameGridViewModel gameGridVM)
        {
            BlockQueueVM = blockQueueVM;
            WorkBlocksVM = workBlocksViewModel;
            GameStateVM = gameStateVM;
            GameGridVM = gameGridVM;
        }
    }
}
