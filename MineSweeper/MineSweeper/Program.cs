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
            GameEngine game = new GameEngine(new XMLSerializer(), new FileSystem());
            if (args.Length == 1 && args[0] == "newgame")
            {
                game.LoadGame("game.xml");
                if (game.GameBoard.Status == GameStatus.Completed)
                {
                    game.NewGame();
                    game.SaveGame("game.xml");
                }
                DrawBoard(game);
            }
        }

        private static void DrawBoard(GameEngine game)
        {
            int i = 0;
            string row = "";
            foreach (var cell in game.GameBoard.Cells)
            {
                row += cell.IsMine ? "X " : cell.AdjacentMines + " ";
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
