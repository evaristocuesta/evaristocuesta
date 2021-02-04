using MineSweeperEngine;
using SerializerLib;
using System;
using System.IO;
using System.IO.Abstractions;

namespace MineSweeper
{
    class Program
    {
        public static readonly string CURRENT_GAME_FILE = "../../../../Game/game.xml";
        public static readonly string README_TEMPLATE = "../../../../../README.md.template";
        public static readonly string README = "../../../../../README.md";

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "newgame")
            {
                NewGame();
            }
            else if (args.Length == 3 && args[0] == "flagcell" 
                && int.TryParse(args[1], out int x)
                && int.TryParse(args[2], out int y))
            {
                FlagCell(x, y);
            }
            else if (args.Length == 3 && args[0] == "revealcell"
                && int.TryParse(args[1], out x)
                && int.TryParse(args[2], out y))
            {
                RevealCell(x, y);
            }
            
        }

        private static void NewGame()
        {
            try
            {
                GameEngine game = new GameEngine(new XMLSerializer(), new FileSystem());
                game.LoadGame(CURRENT_GAME_FILE);
                if (game.GameBoard.Status == GameStatus.Completed 
                    || game.GameBoard.Status == GameStatus.Failed)
                {
                    game.NewGame();
                    game.SaveGame(CURRENT_GAME_FILE);
                    DrawBoardInTemplate(game.GameBoard, README_TEMPLATE, README);
                }
                DrawBoardRevealed(game.GameBoard);
                Console.WriteLine("");
                DrawBoard(game.GameBoard);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void FlagCell(int x, int y)
        {
            try
            {
                GameEngine game = new GameEngine(new XMLSerializer(), new FileSystem());
                game.LoadGame(CURRENT_GAME_FILE);
                if (game.GameBoard.Status == GameStatus.InProgress)
                {
                    game.FlagCell(x, y);
                    game.SaveGame(CURRENT_GAME_FILE);
                    DrawBoardRevealed(game.GameBoard);
                    Console.WriteLine("");
                    DrawBoard(game.GameBoard);
                    DrawBoardInTemplate(game.GameBoard, README_TEMPLATE, README);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void RevealCell(int x, int y)
        {
            try
            {
                GameEngine game = new GameEngine(new XMLSerializer(), new FileSystem());
                game.LoadGame(CURRENT_GAME_FILE);
                if (game.GameBoard.Status == GameStatus.InProgress)
                {
                    game.RevealCell(x, y);
                    game.SaveGame(CURRENT_GAME_FILE);
                    DrawBoardRevealed(game.GameBoard);
                    Console.WriteLine("");
                    DrawBoard(game.GameBoard);
                    DrawBoardInTemplate(game.GameBoard, README_TEMPLATE, README);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void DrawBoardRevealed(GameBoard gameBoard)
        {
            int i = 0;
            string row = "";
            foreach (var cell in gameBoard.Cells)
            {
                if (cell.IsMine)
                    row += "M ";
                else
                    row += cell.AdjacentMines + " ";
                i++;
                if (i == 10)
                {
                    i = 0;
                    Console.WriteLine(row);
                    row = "";
                }
            }
        }

        private static void DrawBoard(GameBoard gameBoard)
        {
            int i = 0;
            string row = "";
            foreach (var cell in gameBoard.Cells)
            {
                if (cell.IsRevealed && !cell.IsMine)
                    row += cell.AdjacentMines + " ";
                else if (cell.IsRevealed && cell.IsMine)
                    row += "M ";
                else if (cell.IsFlagged)
                    row += "F ";
                else
                    row += "X ";
                i++;
                if (i == 10)
                {
                    i = 0;
                    Console.WriteLine(row);
                    row = "";
                }
            }
        }

        private static void DrawBoardInTemplate(GameBoard gameBoard, string readmeTemplateFile, string readmeFile)
        {
            try
            {
                string template = File.ReadAllText(readmeTemplateFile);
                string board = string.Empty;
                if (gameBoard.Status == GameStatus.Completed)
                {
                    board += $"## YOU HAVE WON - [NEW GAME]({BuildIssueNewGameLink()})\n";
                }
                else if (gameBoard.Status == GameStatus.Failed)
                {
                    board += $"## YOU HAVE LOST - [NEW GAME]({BuildIssueNewGameLink()})\n";
                }
                board += "|   |   |   |   |   |   |   |   |   |   |\n";
                board += "| - | - | - | - | - | - | - | - | - | - |\n";
                int i = 0;
                string row = "|";
                foreach (var cell in gameBoard.Cells)
                {
                    if (cell.IsRevealed && !cell.IsMine)
                        row += $"![{cell.AdjacentMines}](MineSweeper/Resources/cell-{cell.AdjacentMines}.jpg \"{cell.AdjacentMines}\")|";
                    else if (cell.IsRevealed && cell.IsMine)
                        row += "![Mine](MineSweeper/Resources/cell-mine.jpg \"Mine\")|";
                    else if (cell.IsFlagged)
                        row += $"[![Flag](MineSweeper/Resources/cell-flag.jpg \"Flag\")]({BuildIssueLink(cell)})|";
                    else
                        row += $"[![Not revealed](MineSweeper/Resources/cell-not-revealed.jpg \"Not revealed\")]({BuildIssueLink(cell)})|";
                    i++;
                    if (i == 10)
                    {
                        i = 0;
                        board += row;
                        row = "\n|";
                    }
                }
                template = template.Replace("{GameBoard}", board);
                File.WriteAllText(readmeFile, template);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving readme.md: {ex.Message}");
            }
        }

        private static string BuildIssueLink(Cell cell)
        {
            return $"https://github.com/evaristocuesta/evaristocuesta/issues/new?title=revealcell%7C{cell.PosX}%7C{cell.PosY}" +
                $"&body=Just+push+%27Submit+new+issue%27+without+editing+the+title+if+you+want+to+reveal+the+cell.+If+you+want+to+flag+" +
                $"the+cell,+replace+%27revealcell%27+by+%27flagcell%27+in+the+title.+The+README.md+will+be+updated+after+some+minutes.";
        }

        private static string BuildIssueNewGameLink()
        {
            return $"https://github.com/evaristocuesta/evaristocuesta/issues/new?title=newgame" +
                $"&body=Just+push+%27Submit+new+issue%27+without+editing+the+title+if+you+want+to+play+a+new+game." +
                $"The+README.md+will+be+updated+after+some+minutes.";
        }
    }
}
