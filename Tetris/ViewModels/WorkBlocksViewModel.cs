using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tetris.Commands;
using Tetris.Models;

namespace Tetris.ViewModels
{
    internal class WorkBlocksViewModel : BaseViewModel
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

        public Block NextBlock { get; private set; }

        public Block HeldBlock { get; private set; }

        public bool CanHold { get; private set; }

        public int Score { get; private set; }

        public bool GameOver { get; set; }

        public GameGridViewModel GameGridVM;

        public WorkBlocksViewModel()
        {
            MoveBlockLeft = new RelayCommand(MoveBlockLeft_Executed);
            MoveBlockRight = new RelayCommand(MoveBlockRight_Executed);
            MoveBlockDown = new RelayCommand(MoveBlockDown_Executed);
            RotateBlockCW = new RelayCommand(RotateBlockCW_Executed);
            RotateBlockCCW = new RelayCommand(RotateBlockCCW_Executed);
            HoldBlock = new RelayCommand(HoldBlock_Executed);
            DropBlock = new RelayCommand(DropBlock_Executed);
            NextBlock = RandomBlock();
            CurrentBlock = GetAndUpdate();
        }

        #region Commands

        #region --- MoveBlockLeft ---

        public ICommand MoveBlockLeft { get; private set; }

        private void MoveBlockLeft_Executed(object param)
        {
            Move(0, -1);

            if (!BlockFits())
            {
                Move(0, 1);
            }
        }

        #endregion --- MoveBlockLeft ---

        #region --- MoveBlockRight ---

        public ICommand MoveBlockRight { get; private set; }

        private void MoveBlockRight_Executed(object param)
        {
            Move(0, 1);

            if (!BlockFits())
            {
                Move(0, -1);
            }
        }

        #endregion --- MoveBlockRight ---

        #region --- MoveBlockDown ---

        public ICommand MoveBlockDown { get; private set; }

        private void MoveBlockDown_Executed(object param)
        {
            Move(1, 0);

            if (!BlockFits())
            {
                Move(-1, 0);
                PlaceBlock();
            }
        }

        #endregion --- MoveBlockRight ---

        #region --- RotateBlockCW ---

        public ICommand RotateBlockCW { get; private set; }

        private void RotateBlockCW_Executed(object param)
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

        #endregion --- RotateBlockCW ---

        #region --- RotateBlockCCW ---

        public ICommand RotateBlockCCW { get; private set; }

        private void RotateBlockCCW_Executed(object param)
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

        #endregion --- RotateBlockCCW ---

        #region --- HoldBlock ---

        public ICommand HoldBlock { get; private set; }

        private void HoldBlock_Executed(object param)
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        #endregion --- HoldBlock ---

        #region --- DropBlock ---

        public ICommand DropBlock { get; private set; }

        private void DropBlock_Executed(object param)
        {
            Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

        #endregion --- DropBlock ---

        #endregion Commands

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in CurrentBlock.Tiles[CurrentBlock.RotationState])
            {
                yield return new Position(p.Row + CurrentBlock.Offset.Row, p.Column + CurrentBlock.Offset.Column);
            }
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

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = GetAndUpdate();
                CanHold = true;
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

        public int BlockDropDistance()
        {
            int drop = GameGridVM.Rows;

            foreach (Position p in TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }


        public void Move(int rows, int columns)
        {
            CurrentBlock.Offset.Row += rows;
            CurrentBlock.Offset.Column += columns;
        }

        public void Reset()
        {
            CurrentBlock.RotationState = 0;
            CurrentBlock.StartOffset = new Position(0,0);
        }

        private Block RandomBlock()
        {
            return (Block)Activator.CreateInstance(blocksTypes[random.Next(blocksTypes.Length)]);
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

        private bool IsGameOver()
        {
            return !(GameGridVM.IsRowEmpty(0) && GameGridVM.IsRowEmpty(1));
        }
    }
}
