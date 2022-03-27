namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class BallStateDisable : BallState
   {
      public override void OnEnterState()
      {
         pro_ball.pub_ballView.pub_scaling = true;
         pro_ball.pub_ballView.pub_scale = 1f;
         pro_ball.pub_ballView.transform.localScale = new Vector3(pro_ball.pub_ballView.pub_scale, pro_ball.pub_ballView.pub_scale, pro_ball.pub_ballView.pub_scale);
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

