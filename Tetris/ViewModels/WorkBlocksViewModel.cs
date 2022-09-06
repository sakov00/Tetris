using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Models;

namespace Tetris.ViewModels
{
    internal class WorkBlocksViewModel
    {
        public Block CurrentBlock { get; set; }

        public Block HeldBlock { get; private set; }

        public bool CanHold { get; private set; }


        public GameGridViewModel GameGridVM;

        public BlockQueueViewModel BlockQueueVM;

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in CurrentBlock.Tiles[CurrentBlock.RotationState])
            {
                yield return new Position(p.Row + CurrentBlock.Offset.Row, p.Column + CurrentBlock.Offset.Column);
            }
        }

        public void RotateCW()
        {
            CurrentBlock.RotationState = (CurrentBlock.RotationState + 1) % CurrentBlock.Tiles.Length;
        }

        public void RotateCCW()
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

        //public void RotateBlockCW()
        //{
        //    CurrentBlock.RotateCW();

        //    if (!BlockFits())
        //    {
        //        CurrentBlock.RotateCCW();
        //    }
        //}

        //public void RotateBlockCCW()
        //{
        //    CurrentBlock.RotateCCW();

        //    if (!BlockFits())
        //    {
        //        CurrentBlock.RotateCW();
        //    }
        //}

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

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueueVM.GetAndUpdate();
            }
            else
            {
                Block tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        private void PlaceBlock()
        {
            foreach (Position p in TilePositions())
            {
                GameGridVM[p.Row, p.Column] = CurrentBlock.Id;
            }


            CurrentBlock = BlockQueueVM.GetAndUpdate();
            CanHold = true;
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

        public void DropBlock()
        {
            Move(BlockDropDistance(), 0);
            PlaceBlock();
        }

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
    }
}
