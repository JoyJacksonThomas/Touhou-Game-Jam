using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBrosCam : MonoBehaviour
{

   public List<Transform> mTargets;

   public Vector3 mOffset;

   public float xMin;
   public float xMax;
   public float yMin;
   public float yMax;

   public float lerpSpeed;

   void LateUpdate()
   {
      Vector3 centerPoint = GetCenterPoint();

      centerPoint += mOffset;
      centerPoint.x = Mathf.Clamp(centerPoint.x, xMin, xMax);
      centerPoint.y = Mathf.Clamp(centerPoint.y, yMin, yMax);

      transform.position = Vector3.Lerp(transform.position, centerPoint, lerpSpeed);

      SpawnPlayer();
   }

   Vector3 GetCenterPoint()
   {
      var bounds = new Bounds(mTargets[0].position, Vector3.zero);
      for(int i = 0; i < mTargets.Count; i++)
      {
         bounds.Encapsulate(mTargets[i].position);
      }

      return bounds.center;
   }

   void SpawnPlayer()
   {
      for (int i = 0; i < mTargets.Count; i++)
      {
         if(!mTargets[i].GetComponent<Renderer>().isVisible)
         {
            mTargets[i].position = Vector3.zero;
            mTargets[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
         }
      }
   }
}
