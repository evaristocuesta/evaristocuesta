using SerializerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSweeper
{
    public class GameEngine
    {
        public const int NUM_COLUMNS = 10;
        public const int NUM_ROWS = 10;
        public const int NUM_MINES = 10;

        private readonly ISerializer _serializer;

        public GameBoard GameBoard { get; private set; }

        public GameEngine(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public void NewGame()
        {
            Random random = new Random();
            GameBoard = new GameBoard();
            for (int i = 0; i < NUM_COLUMNS; i++)
            {
                for (int j = 0; j < NUM_ROWS; j++)
                {
                    GameBoard.Cells.Add(new Cell()
                    {
                        PosX = i,
                        PosY = j
                    });
                }
            }
            for (int i = 0; i < NUM_MINES; i++)
            {
                bool mineCreated = false;
                while (!mineCreated)
                {
                    int x = random.Next(0, NUM_COLUMNS - 1);
                    int y = random.Next(0, NUM_ROWS - 1);
                    if (!MineExists(x, y))
                    {
                        SearchCell(x, y)!.IsMine = true;
                        mineCreated = true;
                    }
                }
            }
        }

        public void LoadCurrentGame()
        {
        }

        private bool MineExists(int x, int y)
        {
            if (x < 0 || y < 0 || x >= NUM_COLUMNS || y >= NUM_ROWS)
                throw new IndexOutOfRangeException($"The coordinate ({x}, {y}) is out of range");

            return GameBoard.Cells.Exists(c => c.PosX == x && c.PosY == y && c.IsMine);
        }

        private Cell SearchCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= NUM_COLUMNS || y >= NUM_ROWS)
                throw new IndexOutOfRangeException($"The coordinate ({x}, {y}) is out of range");

            return GameBoard.Cells.FirstOrDefault(c => c.PosX == x && c.PosY == y);
        }
    }
}
