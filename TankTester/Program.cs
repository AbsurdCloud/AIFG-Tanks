using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GridWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // initializing training ground
            GameForFastTesting game = new GameForFastTesting(new GameSpecifics());
            // loading board 
            game.LoadTerrain("..\\..\\..\\AIFG-TANKS\\Help files\\defaultTerrain.TanksTerrain");

            // game.PlayGame() method needs an array of BasePlayers to work
            BasePlayer[] testplayers = new BasePlayer[4];
            //initialize players
            testplayers[0] = new BasePlayer();
            testplayers[1] = new BasePlayer();
            testplayers[2] = new BasePlayer();
            testplayers[3] = new BasePlayer();
            List<ArrayList> gameResult;
            // if testType = true => check match execution time; if false => write game replay in ReplayFile folder 
            bool testType = false;
            if (testType)
            {
                
                Stopwatch stopwatch;
                for (int i = 0; i < 1000; i++)
                {
                    stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
                    gameResult = game.PlayGame(testplayers);
                    stopwatch.Stop();
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                }
            }
            else
            {
                String filePath = "..\\..\\..\\AIFG-TANKS\\ReplayFiles\\Game.txt";
                gameResult = game.PlayGameAndGetReplayFile(testplayers, filePath);
            }
            
            Console.ReadKey();
            
        }
    }
}
