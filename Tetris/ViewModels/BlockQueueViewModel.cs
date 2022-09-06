using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tetris.Models;

namespace Tetris.ViewModels
{
    internal class BlockQueueViewModel
    {
        private readonly Block[] blocks = new Block[7];

        private readonly Random random = new Random();

        public Block NextBlock { get; private set; }

        public BlockQueueViewModel()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetAndUpdate()
        {
            Block block = NextBlock;
            NextBlock = RandomBlock();
            return block;
        }
    }
}
