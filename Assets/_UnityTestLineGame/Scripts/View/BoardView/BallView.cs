namespace UnityTestLineGame
{
   using System.Collections;
   using System.Collections.Generic;
   using UnityEngine;

   public class BallView : MonoBehaviour
   {
      public MeshRenderer pub_meshRenderer;
      public float pub_movingSpeed;
      public float pub_scale;
      public bool pub_scaling;

      public float pub_scaleSpeed = 20.0f;

      // Start is called before the first frame update
      void Start()
      {

      }

      // Update is called once per frame
      void Update()
      {
         if(!pub_scaling)
         {
            return;
         }
         if (pub_scale > 0)
         {
            pub_scale -= pub_scaleSpeed * Time.deltaTime;
            if (pub_scale <= 0)
            {
               pub_scale = 0;
               gameObject.SetActive(false);
            }
            transform.localScale = new Vector3(pub_scale, pub_scale, pub_scale);
         }
      }
   }
}

