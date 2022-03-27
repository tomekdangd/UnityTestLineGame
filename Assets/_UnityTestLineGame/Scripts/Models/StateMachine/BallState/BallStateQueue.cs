namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class BallStateQueue : BallState
   {
      public override void OnEnterState()
      {
         pro_ball.pub_ballView.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
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

