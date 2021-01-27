using MineSweeperEngine;
using SerializerLib;
using System;

namespace MineSweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            GameEngine game = new GameEngine(new XMLSerializer());
            if (args.Length == 1 && args[0] == "newgame")
            {
                game.NewGame();
                DrawBoard(game);
                game.SaveGame("game.xml");
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
