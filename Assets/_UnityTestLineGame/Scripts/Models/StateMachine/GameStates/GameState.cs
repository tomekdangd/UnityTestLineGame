namespace UnityTestLineGame
{
   // An abstract class for game state every game state will need to extends this class
   public abstract class GameState
   {
      protected GameState() { }

      protected Board pro_board;

      public void SetBoard(Board board)
      {
         pro_board = board;
      }

      public abstract void OnEnterState();
      public abstract void OnExecuteState(float time);
      public virtual void OnExitState()
      {
         pro_board = null;
      }
      public abstract void OnHandleInput(float time);
   }
}

