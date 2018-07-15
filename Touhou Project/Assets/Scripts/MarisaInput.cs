using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MarisaInput : MonoBehaviour
{
   public AudioClip mLaserAudio;
   public AudioClip mExplosionAudio;

   private Rigidbody2D mRigidBody2D;

   public bool mPlayerAttacking = false;
   Animator mAnimator;

   GameObject currentAttackCollider;
   public GameObject[] hitBoxArray;

   public GameObject mStarPrefab;
   public GameObject mLandMinePrefab;
   public GameObject mLaserPrefab;

   private LandMine mLandMine;
   private bool mLandMinePlanted = false;

   public float mRecoveryForce = 0;

   public bool mNeutralSpecialUsed = false;
   public bool mSideSpecialUsed = false;
   public bool mDownSpecialUsed = false;
   public bool mUpSpecialUsed = false;

   public GameObject mSealAnim;

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
            

            if ((directionX > 0.5 || directionX < -0.5) && !mSideSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("SideSpecial");
               mSideSpecialUsed = true;
               GameObject temp = (GameObject)Instantiate(mSealAnim, transform);
            }
            else if (directionY < -0.5 && !mDownSpecialUsed)
            {
               if (mLandMinePlanted == false)
               {
                  mPlayerAttacking = true;
                  mAnimator.SetTrigger("DownSpecial");
               }
               else
               {
                  mPlayerAttacking = true;
                  mLandMine.Detonate();
                  mLandMinePlanted = false;
                  mPlayerAttacking = false;
                  mDownSpecialUsed = true;
                  GameObject temp = (GameObject)Instantiate(mSealAnim, transform);
                  GameObject.Find("SoundEffects").GetComponent<AudioSource>().clip = mExplosionAudio;
                  GameObject.Find("SoundEffects").GetComponent<AudioSource>().Play();
               }
            }
            else if (directionY > 0.5 && !mUpSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("UpSpecial");
               mUpSpecialUsed = true;
               GameObject temp = (GameObject)Instantiate(mSealAnim, transform);
            }
            else if(!mNeutralSpecialUsed)
            {
               mPlayerAttacking = true;
               mAnimator.SetTrigger("NeutralSpecial");
               mNeutralSpecialUsed = true;
               GameObject temp = (GameObject)Instantiate(mSealAnim, transform);
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
         laser.transform.GetChild(0).eulerAngles = new Vector3(0, -90, 0);
      }
      else
      {

         laser.GetComponent<Projectile>().SetDirection(-1, 0);
         gameObject.GetComponent<Rigidbody2D>().velocity.Set(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
         laser.transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
      }
      Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), laser.GetComponent<Collider2D>());
   }

   public void PlantMine()
   {
      GameObject mine = Instantiate(mLandMinePrefab, transform.position, Quaternion.identity) as GameObject;
      mLandMine = mine.GetComponent<LandMine>();
      mLandMinePlanted = true;
   }

   public void Recovery()
   {
      mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
      mRigidBody2D.AddForce(new Vector2(0, mRecoveryForce));
   }
}