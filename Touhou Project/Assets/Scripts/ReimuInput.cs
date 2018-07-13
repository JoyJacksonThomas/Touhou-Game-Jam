using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ReimuInput : MonoBehaviour
{

    public bool mPlayerAttacking = false;
    Animator mAnimator;

    GameObject currentAttackCollider;
    public GameObject[] hitBoxArray;


    // Use this for initialization
    void Start()
    {
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetComponent<PlayerMotor>().mCanMove = !mPlayerAttacking;
    }

    public void Attack(bool attack, bool special, float directionX, float directionY)
    {
        if (!mPlayerAttacking)
        {
            if (attack)
            {
                mPlayerAttacking = true;
                Debug.Log("X:" + directionX + "Y:" + directionY);
                if (directionX == 0 && directionY == 0)
                {
                    mAnimator.SetTrigger("NeutralAttack");

                }
                else if (directionX > 0.9 || directionX < -0.9)
                {
                    mAnimator.SetTrigger("SideAttack");
                }
                else if (directionY < 0.9)
                {
                    mAnimator.SetTrigger("DownAttack");
                }
                else
                {
                    SwitchAttackBool();
                }
            }
            if (special)
            {
                mPlayerAttacking = true;
                Debug.Log("X:" + directionX + "Y:" + directionY);
                if (directionX == 0 && directionY == 0)
                {
                    mAnimator.SetTrigger("NeutralSpecial");

                }
                else if (directionX > 0.9 || directionX < -0.9)
                {
                    mAnimator.SetTrigger("SideSpecial");
                }
                else if (directionY < 0.9)
                {
                    mAnimator.SetTrigger("DownSpecial");
                }
                else
                {
                    SwitchAttackBool();
                }
            }
        }
    }

    public void SwitchAttackBool()
    {
        mPlayerAttacking = false;
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
