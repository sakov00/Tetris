using System.Collections.Generic;

namespace Tetris.Models
{
    public class Block
    {
        public virtual int Id { get; set; }

        public virtual Position[][] Tiles { get; set; }

        public virtual Position StartOffset { get; set; }

        public virtual int RotationState { get; set; }

        public virtual Position Offset { get; set; }
    }
}
