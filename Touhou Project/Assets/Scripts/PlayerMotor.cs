﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        mRigidBody2D = GetComponent<Rigidbody2D>();
        mPlayerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        mIsGrounded = Physics2D.OverlapCircle(mGroundCheck.position, 0.01f, mLayerMask);
        mPlayerAnimator.SetBool("Grounded", mIsGrounded);
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
        if (mCanMove)
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
            mRigidBody2D.gravityScale = 5;

        }
        else
        {
            mRigidBody2D.gravityScale = 3;
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
            if(col.transform.parent.GetComponent<PlayerMotor>().mFacingRight)
            {

                Debug.Log("Hit " + col.GetComponent<HitboxContents>().mDamage);
                mRigidBody2D.AddForce(col.GetComponent<HitboxContents>().mDirection * col.GetComponent<HitboxContents>().mDamage);
            }
            else
            {

                Debug.Log("Hit " + col.GetComponent<HitboxContents>().mDamage);
                mRigidBody2D.AddForce(new Vector2(-(col.GetComponent<HitboxContents>().mDirection.x), col.GetComponent<HitboxContents>().mDirection.y) * col.GetComponent<HitboxContents>().mDamage);
            }

            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), col.GetComponent<Collider2D>());
        }
    }
}
