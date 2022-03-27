namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class BallStateIdle : BallState
   {
      public override void OnEnterState()
      {
         // Play Idle Animation?
         pro_ball.pub_ballView.transform.localScale = new Vector3(1f, 1f, 1f);
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

