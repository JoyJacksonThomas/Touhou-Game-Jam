using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
   public AudioClip mAttackAudio;

   private Rigidbody2D mRigidBody2D;
   private Animator mPlayerAnimator;

   public float mSpeed;
   public float mJumpForce;
   public float mDashDistance = 3;

   public bool mFacingRight = true;
   public bool mCanMove = true;

   public bool mIsGrounded = false;
   public LayerMask mLayerMask;
   public Transform mGroundCheck;
   private bool mHasDoubleJumped = false;

   public float mDefaultGravityScale = 3;
   public float mHeavyGravityScale = 5;

   public int mDamageMultiplier = 0;

   Text mHealthCounter;

   void Start()
   {
      mRigidBody2D = GetComponent<Rigidbody2D>();
      mPlayerAnimator = GetComponent<Animator>();
       PlayerController pc= GetComponent<PlayerController>();

      if (pc.mPlayerIndex == 0)
      {
         mHealthCounter = GameObject.Find("P1Health/Health").GetComponentInChildren<Text>();
      }
      else if (pc.mPlayerIndex == 1)
      {
         mHealthCounter = GameObject.Find("P2Health/Health").GetComponentInChildren<Text>();
      }


   }

   void Update()
   {
      mIsGrounded = Physics2D.OverlapCircle(mGroundCheck.position, 0.01f, mLayerMask);
      mPlayerAnimator.SetBool("Grounded", mIsGrounded);
      if(mIsGrounded)
      {
         mHasDoubleJumped = false;
      }

      mHealthCounter.text = mDamageMultiplier.ToString() + " %";
   }

   public void Flip()
   {
      Vector3 theScale = transform.localScale;
      theScale.x *= -1;
      transform.localScale = theScale;
      mFacingRight = !mFacingRight;
   }

   public void Move(float _direction)
   {
      if (mCanMove && _direction != 0 )
      {
         mRigidBody2D.velocity = new Vector2(_direction * mSpeed, mRigidBody2D.velocity.y);
            Debug.Log(mRigidBody2D.velocity);
         if ((_direction < 0 && mFacingRight) || (_direction > 0 && !mFacingRight))
         {
            Flip();
         }
      }
   }

   public void Dash()
   {

   }

   public void FastFall(float _vertical)
   {

      if (name == "Player2")
      {
         Debug.Log(_vertical);
      }

      if (_vertical < -.9f)
      {
         mRigidBody2D.gravityScale = mHeavyGravityScale;

      }
      else
      {
         mRigidBody2D.gravityScale = mDefaultGravityScale;
      }
   }

   public void Jump()
   {
      if (mCanMove)
      {
         if (mIsGrounded || !mHasDoubleJumped)
         {
            mRigidBody2D.velocity = new Vector2(mRigidBody2D.velocity.x, 0);
            mRigidBody2D.AddForce(new Vector2(0, mJumpForce));
            if(mIsGrounded == false)
            {
               mHasDoubleJumped = true;
            }
         }
      }
   }

   void OnTriggerEnter2D(Collider2D col)
   {
      if (col.tag == "Attack")
      {
         mDamageMultiplier += col.GetComponent<HitboxContents>().mDamageMultiplierFactor;
         if (col.transform.parent.GetComponent<PlayerMotor>().mFacingRight)
         {
            mRigidBody2D.AddForce(col.GetComponent<HitboxContents>().mDirection * col.GetComponent<HitboxContents>().mDamage * (mDamageMultiplier / 100f));
         }
         else
         {
            mRigidBody2D.AddForce(new Vector2(-(col.GetComponent<HitboxContents>().mDirection.x), col.GetComponent<HitboxContents>().mDirection.y) * col.GetComponent<HitboxContents>().mDamage * (mDamageMultiplier / 100f));
         }
         GameObject.Find("SoundEffects").GetComponent<AudioSource>().clip = mAttackAudio;
         GameObject.Find("SoundEffects").GetComponent<AudioSource>().Play(0);
         Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
      }
      else if (col.tag == "SpecialAttack")
      {
         mDamageMultiplier += col.GetComponent<HitboxContents>().mDamageMultiplierFactor;
         if (col.name == "Explosion(Clone)")
         {
            col.GetComponent<HitboxContents>().mDirection = ((Vector2)transform.position - (Vector2)col.transform.position).normalized;
            mRigidBody2D.AddForce(col.GetComponent<HitboxContents>().mDirection * col.GetComponent<HitboxContents>().mDamage * (mDamageMultiplier/100f));
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
         }
         else
         {
            mRigidBody2D.AddForce(col.GetComponent<HitboxContents>().mDirection * col.GetComponent<HitboxContents>().mDamage * (mDamageMultiplier / 100f));
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
         }
      }
      else if (col.tag == "Stun")
      {
         GetComponent<PlayerController>().mCanInput = false;
         StartCoroutine("Stun");
      }
   }

   IEnumerator Stun()
   {
      yield return new WaitForSeconds(2);
      GetComponent<PlayerController>().mCanInput = true;
   }
}
