using System.Collections.Generic;

namespace MineSweeperEngine
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
