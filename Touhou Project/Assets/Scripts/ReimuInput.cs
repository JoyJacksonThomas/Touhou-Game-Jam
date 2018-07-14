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

    public GameObject mNeedlePrefab;


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
               if (directionX > 0.5 || directionX < -0.5)
                {
                    mAnimator.SetTrigger("SideAttack");
                }
                else if (directionY < -0.5)
                {
                    mAnimator.SetTrigger("DownAttack");
                }
                else if (directionY > 0.5)
                {
                    mAnimator.SetTrigger("UpAttack");
                }
                else
                {
                    mAnimator.SetTrigger("NeutralAttack");
                }
            }
            if (special)
            {
                mPlayerAttacking = true;
                Debug.Log("X:" + directionX + "Y:" + directionY);
               if (directionX > 0.5 || directionX < -0.5)
                {
                    mAnimator.SetTrigger("SideSpecial");
                }
                else if (directionY < -0.5)
                {
                    mAnimator.SetTrigger("DownSpecial");
                }
                else if (directionY > 0.5)
                {
                    mAnimator.SetTrigger("UpSpecial");
                }
                else
                {
                    mAnimator.SetTrigger("NeutralSpecial");
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

    public void NeedleBlast()
    {
        GameObject needle = (GameObject)Instantiate(mNeedlePrefab, gameObject.transform.position, Quaternion.identity);
        if (gameObject.GetComponent<PlayerMotor>().mFacingRight)
        {

            needle.GetComponent<SpriteRenderer>().flipX = true;
            needle.GetComponent<Projectile>().SetDirection(1,0);
        }
        else
        {
            
            needle.GetComponent<Projectile>().SetDirection(-1, 0);
        }
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), needle.GetComponent<Collider2D>());
        

    }

    public void DestroyAttackCollider()
    {
        Destroy(currentAttackCollider);
    }

}
