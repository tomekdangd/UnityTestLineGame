namespace UnityTestLineGame
{
   // An abstract class for Ball state every Ball state will need to extends this class
   public abstract class BallState
   {
      
      protected BallState() { }

      protected Ball pro_ball;
      public void SetBall(Ball ball)
      {
         pro_ball = ball;
      }

      public abstract void OnEnterState();
      public abstract void OnExecuteState(float time);
      public virtual void OnExitState()
      {
         pro_ball = null;
      }
      public abstract void OnHandleInput(float time);
   }
}
