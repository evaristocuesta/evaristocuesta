using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class GameBoard
    {
        public GameStatus Status { get; private set; }

        public List<Cell> Cells { get; private set; }

        public GameBoard()
        {
            Status = GameStatus.InProgress;
            Cells = new List<Cell>();
        }
    }
}
