using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class Cell
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int AdjacentMines { get; set; }
        public bool IsMine { get; set; }
        public bool IsRevealed {get;set;}
        public bool IsFlagged { get; set; }
    }
}
