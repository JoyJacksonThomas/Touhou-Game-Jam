using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
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

    public float mDefaultGravityScale = 3;
    public float mHeavyGravityScale = 5;

    public float mDamageMultiplier = 0.0f;

    Text mHealthCounter;

    void Start()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mPlayerAnimator = GetComponent<Animator>();
        if (gameObject.name == "Player1")
        {
            mHealthCounter = GameObject.Find("P1Health/Health").GetComponentInChildren<Text>();
        }
        else if (gameObject.name == "Player2")
        {
            mHealthCounter = GameObject.Find("P2Health/Health").GetComponentInChildren<Text>();
        }


    }

    void Update()
    {
        mIsGrounded = Physics2D.OverlapCircle(mGroundCheck.position, 0.01f, mLayerMask);
        mPlayerAnimator.SetBool("Grounded", mIsGrounded);

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
        if (mCanMove && _direction != 0)
        {
            mRigidBody2D.velocity = new Vector2(_direction * mSpeed, mRigidBody2D.velocity.y);
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
            if (mIsGrounded)
            {
                mRigidBody2D.AddForce(new Vector2(0, mJumpForce));
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Attack")
        {
            mDamageMultiplier += .02f;
            if (col.transform.parent.GetComponent<PlayerMotor>().mFacingRight)
            {
                Debug.Log("Hit " + col.GetComponent<HitboxContents>().mDamage);
                mRigidBody2D.AddForce(col.GetComponent<HitboxContents>().mDirection * col.GetComponent<HitboxContents>().mDamage * mDamageMultiplier);
            }
            else
            {

                Debug.Log("Hit " + col.GetComponent<HitboxContents>().mDamage);
                mRigidBody2D.AddForce(new Vector2(-(col.GetComponent<HitboxContents>().mDirection.x), col.GetComponent<HitboxContents>().mDirection.y) * col.GetComponent<HitboxContents>().mDamage * mDamageMultiplier);
            }

            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
        }
    }
}
