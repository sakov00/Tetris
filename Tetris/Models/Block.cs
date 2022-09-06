using System.Collections.Generic;

namespace Tetris.Models
{
    public class Block
    {
        public int Id { get; set; }

        public Position[][] Tiles { get; set; }

        public Position StartOffset { get; set; }

        public int RotationState { get; set; }

        public Position Offset { get; set; }

        public Block()
        {
            Offset = new Position(StartOffset.Row, StartOffset.Column);
        }
    }
}
