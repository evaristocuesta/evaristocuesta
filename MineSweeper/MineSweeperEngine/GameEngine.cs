using SerializerLib;
using System;
using System.IO.Abstractions;
using System.Linq;

namespace MineSweeperEngine
{
    public class GameEngine
    {
        public const int NUM_COLUMNS = 10;
        public const int NUM_ROWS = 10;
        public const int NUM_MINES = 10;

        private readonly ISerializer _serializer;
        private readonly IFileSystem _fileSystem;

        public GameBoard GameBoard { get; private set; }

        public GameEngine(ISerializer serializer,
                          IFileSystem fileSystem)
        {
            _serializer = serializer;
            _fileSystem = fileSystem;
        }

        public void NewGame()
        {
            GameBoard = new GameBoard();
            AddCells();
            AddMines();
            AddAdjacents();
        }

        public void LoadGame(string fileGame)
        {
            try
            {
                GameBoard = _serializer.DeserializeFile<GameBoard>(fileGame);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading the game: {ex.Message}");
            }
        }

        public void SaveGame(string fileGame)
        {
            try
            {
                _fileSystem.File.WriteAllText(fileGame, _serializer.Serialize<GameBoard>(GameBoard));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving the game: {ex.Message}");
            }
        }

        private void AddCells()
        {
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
        }

        private void AddMines()
        {
            Random random = new Random();
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

        private void AddAdjacents()
        {
            var mines = GameBoard.Cells.Where(c => c.IsMine);
            foreach (var mine in mines)
            {
                AddAdjacent(mine.PosX - 1, mine.PosY - 1);
                AddAdjacent(mine.PosX, mine.PosY - 1);
                AddAdjacent(mine.PosX + 1, mine.PosY - 1);
                AddAdjacent(mine.PosX - 1, mine.PosY);
                AddAdjacent(mine.PosX + 1, mine.PosY);
                AddAdjacent(mine.PosX - 1, mine.PosY + 1);
                AddAdjacent(mine.PosX, mine.PosY + 1);
                AddAdjacent(mine.PosX + 1, mine.PosY + 1);
            }
        }

        private void AddAdjacent(int x, int y)
        {
            Cell cell = GameBoard.Cells.FirstOrDefault(c => c.PosX == x
                                    && c.PosY == y
                                    && !c.IsMine);
            if (cell != null)
                cell.AdjacentMines++;
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
