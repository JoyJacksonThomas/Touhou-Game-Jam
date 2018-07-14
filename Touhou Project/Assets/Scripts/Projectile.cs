using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private Vector2 mDirection = Vector2.zero;
    private Rigidbody2D mRigidbody2D;

    public float mSpeed = 2;
    public float mLifeTime = 1;
    
	void Start () {
        mRigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        mRigidbody2D.velocity = mDirection*mSpeed;
        mLifeTime -= Time.deltaTime;
        if (mLifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(float x, float y)
    {
        mDirection = new Vector2(x, y).normalized;
        GetComponent<HitboxContents>().mDirection = mDirection;
    }
}
