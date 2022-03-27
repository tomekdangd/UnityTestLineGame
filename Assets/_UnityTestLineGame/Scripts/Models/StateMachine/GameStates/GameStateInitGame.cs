namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   // Only use for initialization
   public class GameStateInitGame : GameState
   {
      public override void OnEnterState()
      {
         SaveLoadData saveLoadData = new SaveLoadData();
         int highestScore = saveLoadData.LoadHighestScore();
         pro_board = new Board();

         pro_board.Init(Line98StaticData.LINE_98.pub_boardSize, Line98StaticData.LINE_98.pub_numberOfBallAtStart);
         GameStateStaticData.GAME_STATE_START_GAME.SetBoard(pro_board);
         pro_board.pub_highestScore = highestScore;
         Line98StaticData.LINE_98.pub_lineGameUI.SetHighestScore(highestScore);
         saveLoadData.LoadBoard(pro_board);
         GameStateStaticData.GAME_STATE_MACHINE.SetState(GameStateStaticData.GAME_STATE_START_GAME);
      }

      public override void OnExecuteState(float time)
      {
         
      }

      public override void OnExitState()
      {
         base.OnExitState();
      }

      public override void OnHandleInput(float time)
      {
         
      }
   }
}

