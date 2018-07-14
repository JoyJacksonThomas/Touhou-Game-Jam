using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMine : MonoBehaviour {

   // Use this for initialization
   public GameObject mExplosionPrefab;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void Detonate()
   {
      GameObject Explosion = Instantiate(mExplosionPrefab, transform.position, Quaternion.identity) as GameObject;
      Destroy(gameObject);
   }
}
