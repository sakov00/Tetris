using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Models;

namespace Tetris.ViewModels
{
    internal class GameStateViewModel
    {

        public GameGridViewModel GameGrid { get; set; }
        public BlockQueueViewModel BlockQueue { get; set; }
        public bool GameOver { get; private set; }

        public GameStateViewModel()
        {
            GameGrid = new GameGridViewModel(22, 10);
            BlockQueue = new BlockQueueViewModel();
        }
        

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        
    }
}
