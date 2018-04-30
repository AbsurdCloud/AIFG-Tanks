using System;
using System.Collections.Generic;
using System.Text;

namespace GridWorld
{
    // quick version 1 of AI
    public class TankyMcTankFace : BasePlayer
    {
        // Variable that holds the current state of the board
        PlayerWorldState myWorldState;


        public TankyMcTankFace()
            : base()
        {
            this.Name = "TankyMcTankFace";

        }

        public override ICommand GetTurnCommands(IPlayerWorldState igrid)
        {
            myWorldState = (PlayerWorldState)igrid;
            // bot can only move, but not shoot
            // 0 - Up, 1-Down, 2- Left, 3-Right, 4-RotateLeft, 5-RotateRight, 6-Stay
            Random rand = new Random();
            int direction = rand.Next(7);
            for (int i=0; i<myWorldState.MyVisibleSquares.Count; i++)
            {
                WriteTrace(myWorldState.MyVisibleSquares[i] + " ");
            }

            //WriteTrace("ScoreEmptySquare " + myWorldState.ScorePerEmptySquareSeen);
            //WriteTrace("ScoreTankDestroyed " + myWorldState.ScorePerOpposingTankDestroyed);
            //WriteTrace("ScoreRockSquare " + myWorldState.ScorePerRockSquareSeen);

            switch (direction)
            {
                case 0: return new Command(Command.Move.Up, false);
                case 1: return new Command(Command.Move.Down, false);
                case 2: return new Command(Command.Move.Left, false);
                case 3: return new Command(Command.Move.Right, false);
                case 4: return new Command(Command.Move.RotateLeft, false);
                case 5: return new Command(Command.Move.RotateRight, false);
                case 6: return new Command(Command.Move.Stay, false);
            }
            
            return null;
        }
        
       
    }
}
