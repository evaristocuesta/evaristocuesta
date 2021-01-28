using MineSweeperEngine;
using SerializerLib;
using System;
using System.IO.Abstractions;

namespace MineSweeper
{
    class Program
    {
        public static readonly string CURRENT_GAME_FILE = "game.xml";

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
        }

        private static void NewGame()
        {
            try
            {
                GameEngine game = new GameEngine(new XMLSerializer(), new FileSystem());
                game.LoadGame(CURRENT_GAME_FILE);
                if (game.GameBoard.Status == GameStatus.Completed)
                {
                    game.NewGame();
                    game.SaveGame(CURRENT_GAME_FILE);
                }
                DrawBoard(game);
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
                if (game.GameBoard.Status == GameStatus.Completed
                    || game.GameBoard.Status == GameStatus.Failed)
                    game.NewGame();
                game.FlagCell(x, y);
                game.SaveGame(CURRENT_GAME_FILE);
                DrawBoard(game);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private static void DrawBoard(GameEngine game)
        {
            int i = 0;
            string row = "";
            foreach (var cell in game.GameBoard.Cells)
            {
                if (cell.IsFlagged)
                    row += "F ";
                else if (cell.IsMine)
                    row += "X ";
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
    }
}
