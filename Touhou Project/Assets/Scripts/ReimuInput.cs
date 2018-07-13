using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ReimuInput : MonoBehaviour {

    public bool mPlayerAttacking = false;
    Animator mAnimator;

    GameObject currentAttackCollider;
    public GameObject[] hitBoxArray;


	// Use this for initialization
	void Start () {
        mAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack(bool attack, float directionX, float directionY)
    {
        if (!mPlayerAttacking)
        {
            if (attack)
            {
                
                Debug.Log("X:" + directionX + "Y:" + directionY);
                if(directionX == 0 && directionY == 0)
                {                    
                    mAnimator.SetTrigger("NeutralAttack");
                }
            }
        }
    }

    public void SwitchAttackBool()
    {
        mPlayerAttacking = !mPlayerAttacking;
        GetComponent<PlayerMotor>().mCanMove = !GetComponent<PlayerMotor>().mCanMove;
    }

    public void SpawnAttackCollider(int index)
    {
        currentAttackCollider = (GameObject)Instantiate(hitBoxArray[index], gameObject.transform);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), currentAttackCollider.GetComponent<Collider2D>());
    }

    public void DestroyAttackCollider()
    {
        Destroy(currentAttackCollider);
    }

}
