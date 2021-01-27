using System.Collections.Generic;

namespace MineSweeperEngine
{
    public class GameBoard
    {
        public GameStatus Status { get; set; }

        public List<Cell> Cells { get; set; }

        public GameBoard()
        {
            Status = GameStatus.InProgress;
            Cells = new List<Cell>();
        }
    }
}
