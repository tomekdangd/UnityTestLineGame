namespace UnityTestLineGame
{
   // This class is a statemachine for game state use to navigate between different game states
   public class GameStateMachine
   {
      private GameState pri_currentState;
      private GameState pri_previousState;

      public void InitStateMachine() { }
      public void SetState(GameState state)
      {
         if (state != null)
         {
            if (pri_currentState != null)
            {
               pri_currentState.OnExitState();
            }
            pri_previousState = pri_currentState;
            pri_currentState = state;
            pri_currentState.OnEnterState();
         }
      }

      public void OnExecuteState(float time)
      {
         if (pri_currentState != null)
         {
            pri_currentState.OnHandleInput(time);
            pri_currentState.OnExecuteState(time);
         }
      }

      public void SwitchToPrevious()
      {
         if (pri_currentState != null && pri_previousState != null)
         {
            pri_currentState.OnExitState();
            pri_currentState = pri_previousState;
            pri_currentState.OnEnterState();
         }
      }

      public GameState GetCurrentState()
      {
         return pri_currentState;
      }
   }
}

