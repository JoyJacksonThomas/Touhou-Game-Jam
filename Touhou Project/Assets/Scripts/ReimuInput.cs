using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ReimuInput : MonoBehaviour
{
    private Rigidbody2D mRigidBody2D;

    public bool mPlayerAttacking = false;
   Animator mAnimator;

   GameObject currentAttackCollider;
   public GameObject[] hitBoxArray;

   public GameObject mNeedlePrefab;
   public GameObject mYinYangPrefab;
   public GameObject mStunBallPrefab;
    public GameObject mParticleBurst;
    GameObject mCurrentParticle;

    public float mRecoveryForce = 0;

   public bool mNeutralSpecialUsed = false;
   public bool mSideSpecialUsed = false;
   public bool mDownSpecialUsed = false;
   public bool mUpSpecialUsed = false;

   // Use this for initialization
   void Start()
   {
      mAnimator = GetComponent<Animator>();
        mRigidBody2D = GetComponent<Rigidbody2D>();
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
            
            Debug.Log("X:" + directionX + "Y:" + directionY);
            if ((directionX > 0.5 || directionX < -0.5) && !mSideSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("SideSpecial");
               mSideSpecialUsed = true;
            }
            else if (directionY < -0.5 && !mDownSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("DownSpecial");
               mDownSpecialUsed = true;
            }
            else if (directionY > 0.5 && !mUpSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("UpSpecial");
               mUpSpecialUsed = true;
            }
            else if (!mNeutralSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("NeutralSpecial");
               mNeutralSpecialUsed = true;
            }
         }
      }
   }

   public void SwitchAttackBool()
   {
      mPlayerAttacking = false;
      GetComponent<PlayerMotor>().mCanMove = true;
   }

   public void SpawnAttackCollider(int index)
   {
      currentAttackCollider = (GameObject)Instantiate(hitBoxArray[index], gameObject.transform);
      Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), currentAttackCollider.GetComponent<Collider2D>());
   }

   public void YinYangBlast()
   {
      Vector2[] direction = { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, -1) };
      for (int i = 0; i < 4; i++)
      {
         GameObject star = Instantiate(mYinYangPrefab, transform.position, Quaternion.identity) as GameObject;
         Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), star.GetComponent<Collider2D>());
         star.GetComponent<Projectile>().SetDirection(direction[i].x, direction[i].y);
      }
   }

   public void NeedleBlast()
   {
      GameObject needle = (GameObject)Instantiate(mNeedlePrefab, gameObject.transform.position, Quaternion.identity);
      if (gameObject.GetComponent<PlayerMotor>().mFacingRight)
      {

         needle.GetComponent<SpriteRenderer>().flipX = true;
         needle.GetComponent<Projectile>().SetDirection(1, 0);
      }
      else
      {

         needle.GetComponent<Projectile>().SetDirection(-1, 0);
      }
      Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), needle.GetComponent<Collider2D>());


   }

   public void Stun()
   {
      GameObject stunball;
      if (GetComponent<PlayerMotor>().mFacingRight == true)
      {
         stunball = Instantiate(mStunBallPrefab, transform.position + transform.right, Quaternion.identity);
      }
      else
      {
         stunball = Instantiate(mStunBallPrefab, transform.position - transform.right, Quaternion.identity);
      }
      Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), stunball.GetComponent<Collider2D>());
   }

   public void DestroyAttackCollider()
   {
      Destroy(currentAttackCollider);
   }
    public void Recovery()
    {
        mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
        mRigidBody2D.AddForce(new Vector2(0, mRecoveryForce));
        mCurrentParticle = (GameObject)Instantiate(mParticleBurst, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), mCurrentParticle.GetComponent<Collider2D>());


    }

    public void DestroyParticle()
    {
        
        Destroy(mCurrentParticle);

    }

}
