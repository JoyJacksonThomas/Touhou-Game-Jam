using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaInput : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack(bool attack, float directionX, float directionY)
    {
        if (attack)
        {
            Debug.Log("X:" + directionX + "Y:" + directionY);
        }
    }
}
