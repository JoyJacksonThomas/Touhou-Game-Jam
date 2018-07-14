using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour {

   public void DestroySelf()
   {
      Destroy(gameObject);
   }
}
