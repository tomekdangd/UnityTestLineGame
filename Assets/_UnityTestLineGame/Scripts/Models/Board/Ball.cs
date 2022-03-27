namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class Ball
   {
      // Ball idx in the board
      public int pub_idx;

      // just the view of the ball
      public BallView pub_ballView;

      // Ball Type 
      public int pub_ballType;
      public int pub_ballColor;

      public BallStateMachine pub_ballStateMachine = new BallStateMachine();
      public BallStateIdle pub_ballStateIdle = new BallStateIdle();
      public BallStateSelected pub_ballStateSelected = new BallStateSelected();
      public BallStateQueue pub_ballStateQueue = new BallStateQueue();
      public BallStateDisable pub_ballStateDisable = new BallStateDisable();

      public void Init(int idx, BallView ballView, int ballType, int ballColor)
      {
         pub_idx = idx;

         pub_ballView = ballView;

         pub_ballType = ballType;
         pub_ballColor = ballColor;

         pub_ballStateIdle.SetBall(this);
         pub_ballStateMachine.SetState(pub_ballStateIdle);
      }

      public int GetCurrentIdx()
      {
         return pub_idx;
      }

      public void BallTick(float time)
      {
         pub_ballStateMachine.OnExecuteState(time);
      }

      public void PlayAnimation(float time)
      {
         pub_ballView.transform.Rotate(new Vector3(100, 100, 100) * time, Space.World);
      }
   }
}

