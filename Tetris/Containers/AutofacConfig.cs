using Autofac;
using Tetris.Models;
using Tetris.ViewModels;

namespace Tetris.Containers
{
    public class AutofacConfig
    {
        public static IContainer GetContainer { get; set; }

        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Block>();
            builder.RegisterType<Position>();

            builder.RegisterType<GameGridViewModel>();
            builder.RegisterType<WorkBlocksViewModel>();
            builder.RegisterType<ManageCanvasViewModel>();
            builder.RegisterType<MainViewModel>();

            GetContainer = builder.Build();
        }
    }
}