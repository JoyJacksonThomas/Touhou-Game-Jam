using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    int mPlayerIndex;

    string[] prefix = { "P1 ", "P2 " };
    private Animator mPlayerAnimator;
    private PlayerMotor mPlayerMotor;

    public bool mCanInput = true;

    bool mGamePaused = false;

    void Start()
    {
        mPlayerMotor = GetComponent<PlayerMotor>();
        mPlayerAnimator = GetComponent<Animator>();
        if (name == "Player1")
        {
            mPlayerIndex = 0;
        }
        else if (name == "Player2")
        {
            mPlayerIndex = 1;
        }
    }

    void Update()
    {
        if (mCanInput)
        {
            bool _pause = Input.GetButtonDown(prefix[mPlayerIndex] + "Pause");

            if (_pause)
            {
                Debug.Log("Paused");
                if (Time.timeScale == 0.0f && mGamePaused)
                {
                    mGamePaused = false;
                    Time.timeScale = 1.0f;
                }
                else
                {
                    mGamePaused = true;
                    Time.timeScale = 0.0f;
                }

            }
           
                float _horizontal = Input.GetAxis(prefix[mPlayerIndex] + "Horizontal");
                

               

                float _vertical = Input.GetAxis(prefix[mPlayerIndex] + "Vertical");

                mPlayerMotor.FastFall(_vertical);



                bool _jump = Input.GetButtonDown(prefix[mPlayerIndex] + "Jump");

                bool _attack = Input.GetButtonDown(prefix[mPlayerIndex] + "Attack");

                bool _special = Input.GetButtonDown(prefix[mPlayerIndex] + "Special");


            if (Time.timeScale != 0.0f)
            {
                mPlayerMotor.Move(_horizontal);
                mPlayerAnimator.SetFloat("Horizontal", Mathf.Abs(_horizontal));

                if (gameObject.GetComponent<ReimuInput>() != null)
                {
                    gameObject.GetComponent<ReimuInput>().Attack(_attack, _special, _horizontal, _vertical);
                }
                else if (gameObject.GetComponent<MarisaInput>() != null)
                {
                    gameObject.GetComponent<MarisaInput>().Attack(_attack, _special, _horizontal, _vertical);
                }

                if (_jump)
                {
                    mPlayerMotor.Jump();
                }
            }
        }
    }
}