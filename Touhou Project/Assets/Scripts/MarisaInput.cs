using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MarisaInput : MonoBehaviour
{

    public bool mPlayerAttacking = false;
    Animator mAnimator;

    GameObject currentAttackCollider;
    public GameObject[] hitBoxArray;

    public GameObject mStarPrefab;
    public GameObject mLandMinePrefab;
    public GameObject mLaserPrefab;

    private LandMine mLandMine;
    private bool mLandMinePlanted = false;

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

                /*if ((directionX < 0.5 && directionX >= 0) &&(directionX > -0.5 && directionX <= 0) && (directionY < 0.5 && directionY >= 0) && (directionY > -0.5 && directionY <= 0))
                {
                   mAnimator.SetTrigger("NeutralAttack");

                }
                else */
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

                if (directionX > 0.5 || directionX < -0.5)
                {
                    mAnimator.SetTrigger("SideSpecial");
                }
                else if (directionY < -0.5)
                {
                    if (mLandMinePlanted == false)
                    {
                        mAnimator.SetTrigger("DownSpecial");
                    }
                    else
                    {
                        mLandMine.Detonate();
                        mLandMinePlanted = false;
                        mPlayerAttacking = false;
                    }
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

    public void DestroyAttackCollider()
    {
        Destroy(currentAttackCollider);
    }

    public void StarBlast()
    {
        Vector2[] direction = { new Vector2(1, 0), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(0, -1) };
        for (int i = 0; i < 4; i++)
        {
            GameObject star = Instantiate(mStarPrefab, transform.position, Quaternion.identity) as GameObject;
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), star.GetComponent<Collider2D>());
            star.GetComponent<Projectile>().SetDirection(direction[i].x, direction[i].y);
        }
    }

    public void MiniMasterSpark()
    {
        GameObject laser = (GameObject)Instantiate(mLaserPrefab, gameObject.transform.position, Quaternion.identity);
        if (gameObject.GetComponent<PlayerMotor>().mFacingRight)
        {

            laser.GetComponent<SpriteRenderer>().flipX = true;
            laser.GetComponent<Projectile>().SetDirection(1, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity.Set(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
        else
        {

            laser.GetComponent<Projectile>().SetDirection(-1, 0);
            gameObject.GetComponent<Rigidbody2D>().velocity.Set(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), laser.GetComponent<Collider2D>());
    }

    public void PlantMine()
    {
        GameObject mine = Instantiate(mLandMinePrefab, transform.position, Quaternion.identity) as GameObject;
        mLandMine = mine.GetComponent<LandMine>();
        mLandMinePlanted = true;
    }
}