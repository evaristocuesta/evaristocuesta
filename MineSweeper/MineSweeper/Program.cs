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
                game.NewGame();
        }
    }
}
