namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   // This class use to hold all the state of game states
   public static class GameStateStaticData
   {
      public static GameStateMachine GAME_STATE_MACHINE = new GameStateMachine();
      public static GameStateStartGame GAME_STATE_START_GAME = new GameStateStartGame();
      public static GameStateInitGame GAME_STATE_INIT_GAME = new GameStateInitGame();
   }
}

