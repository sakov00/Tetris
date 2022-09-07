using Autofac;
using System.Windows;
using Tetris.Containers;
using Tetris.ViewModels;

namespace Tetris
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = AutofacConfig.GetContainer.Resolve<MainViewModel>();
        }
    }
}
