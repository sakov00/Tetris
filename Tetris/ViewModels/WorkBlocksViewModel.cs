using System;
using System.Collections.Generic;
using Tetris.Models;

namespace Tetris.ViewModels
{
    internal abstract class WorkBlocksViewModel : BaseViewModel
    {
        private readonly Random random = new Random();

        private readonly Type[] blocksTypes = new Type[]
        {
            typeof(IBlock),
            typeof(JBlock),
            typeof(LBlock),
            typeof(OBlock),
            typeof(SBlock),
            typeof(TBlock),
            typeof(ZBlock)
        };

        public Block CurrentBlock { get; set; }

        public Block NextBlock { get; set; }

        public GameGridViewModel GameGridVM { get; set; }

        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged();

            }
        }
        private int score;

        public bool GameOver
        {
            get => gameOver;
            set
            {
                gameOver = value;
                OnPropertyChanged();

            }
        }
        private bool gameOver;

        #region KeyManage

        public void MoveBlockLeft()
        {
            Move(0, -1);

            if (!BlockFits())
            {
                Move(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            Move(0, 1);

            if (!BlockFits())
            {
                Move(0, -1);
            }
        }

        public void MoveBlockDown()
        {
            Move(1, 0);

            if (!BlockFits())
            {
                Move(-1, 0);
                PlaceBlock();
            }
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotationState = (CurrentBlock.RotationState + 1) % CurrentBlock.Tiles.Length;

            if (!BlockFits())
            {
                if (CurrentBlock.RotationState == 0)
                {
                    CurrentBlock.RotationState = CurrentBlock.Tiles.Length - 1;
                }
                else
                {
                    CurrentBlock.RotationState--;
                }
            }
        }

        public void RotateBlockCCW()
        {
            if (CurrentBlock.RotationState == 0)
            {
                CurrentBlock.RotationState = CurrentBlock.Tiles.Length - 1;
            }
            else
            {
                CurrentBlock.RotationState--;
            }

            if (!BlockFits())
            {
                CurrentBlock.RotationState = (CurrentBlock.RotationState + 1) % CurrentBlock.Tiles.Length;
            }
        }

        public void DropBlock()
        {
            Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        #endregion KeyManage

        public void Move(int rows, int columns)
        {
            CurrentBlock.Offset.Row += rows;
            CurrentBlock.Offset.Column += columns;
        }

        private bool BlockFits()
        {
            foreach (Position p in TilePositions())
            {
                if (!GameGridVM.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }

            return true;
        }

        private void PlaceBlock()
        {
            foreach (Position p in TilePositions())
            {
                GameGridVM[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGridVM.ClearFullRows();

            if (!(GameGridVM.IsRowEmpty(0) && GameGridVM.IsRowEmpty(1)))
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = GetAndUpdate();
            }
        }

        public Block GetAndUpdate()
        {
            Block block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);
            return block;
        }

        public int BlockDropDistance()
        {
            int drop = GameGridVM.Rows;
            foreach (Position p in TilePositions())
            {
                drop = Math.Min(drop, TileDropDistance(p));
            }
            return drop;
        }

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in CurrentBlock.Tiles[CurrentBlock.RotationState])
            {
                yield return new Position(p.Row + CurrentBlock.Offset.Row, p.Column + CurrentBlock.Offset.Column);
            }
        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;
            while (GameGridVM.IsEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }
            return drop;
        }

        public void Reset()
        {
            CurrentBlock.RotationState = 0;
            CurrentBlock.StartOffset = new Position(0,0);
        }

        public Block RandomBlock()
        {
            var block = (Block)Activator.CreateInstance(blocksTypes[random.Next(blocksTypes.Length)]);
            int middleOnCanvas = (GameGridVM.Columns / 2) - 1;
            block.StartOffset = new Position(0, middleOnCanvas);
            block.Offset = new Position(block.StartOffset.Row, block.StartOffset.Column);
            return block;
        }
    }
}
