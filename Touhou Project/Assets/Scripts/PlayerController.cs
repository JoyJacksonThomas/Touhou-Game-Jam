using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int mPlayerIndex;
    public bool mClient = false;

    public static string[] prefix = { "P1 ", "P2 " };
    private Animator mPlayerAnimator;
    private PlayerMotor mPlayerMotor;
    private SpriteRenderer mSpriteRenderer;


    public bool mCanInput = true;


    void Start()
    {
        mPlayerMotor = GetComponent<PlayerMotor>();
        mPlayerAnimator = GetComponent<Animator>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();


    }

    void Update()
    {
        if (mClient)
            return;
        if (mCanInput)
        {
            bool _pause = Input.GetButtonDown(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Pause");

            if (_pause)
            {
                if (Time.timeScale == 0.0f)
                {
                    Time.timeScale = 1.0f;
                }
                else
                {
                    Time.timeScale = 0.0f;
                }

            }

            float _horizontal = (int)Input.GetAxisRaw(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Horizontal");
            


            float _vertical = (int)Input.GetAxisRaw(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Vertical");

            mPlayerMotor.FastFall(_vertical);


            bool _jump = Input.GetButtonDown(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Jump");

            bool _attack = Input.GetButtonDown(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Attack");

            bool _special = Input.GetButtonDown(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Special");


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
            else if (Time.timeScale == 0.0f)
            {

                bool Quit1 = Input.GetButton(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Jump");

                bool Quit2 = Input.GetButton(prefix[GameManagerScript.Instance.isNetworked ? 0 : mPlayerIndex] + "Attack");

                if (Quit1 && Quit2)
                {
                    Debug.Log("Quit Game");
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene("Menu");
                }
            }
        }
    }

    public void NetworkUpdate(int[] inputStates)
    {
        if (mCanInput)
        {
            bool _pause = inputStates[(int)NetworkPlugin.InputIDs.PAUSE] == 2;

            if (_pause)
            {
                if (Time.timeScale == 0.0f)
                {
                    Time.timeScale = 1.0f;
                }
                else
                {
                    Time.timeScale = 0.0f;
                }

            }


            float _horizontal = inputStates[(int)NetworkPlugin.InputIDs.HORIZONTAL];

            if (_horizontal == 2 || _horizontal == 3)
            {
                mSpriteRenderer.color = Color.red;
                _horizontal = 0;
            }
            else if (_horizontal == 1 || _horizontal == -1)
            {
                mSpriteRenderer.color = Color.green;
            }
            else if (_horizontal == 0)
            {
                mSpriteRenderer.color = Color.blue;
            }


            float _vertical = inputStates[(int)NetworkPlugin.InputIDs.VERTICAL];

            mPlayerMotor.FastFall(_vertical);



            bool _jump = inputStates[(int)NetworkPlugin.InputIDs.JUMP] == 2;

            bool _attack = inputStates[(int)NetworkPlugin.InputIDs.ATTACK] == 2;

            bool _special = inputStates[(int)NetworkPlugin.InputIDs.SPECIAL] == 2;


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
            else if (Time.timeScale == 0.0f)
            {

                bool Quit1 = inputStates[(int)NetworkPlugin.InputIDs.JUMP] == 2;

                bool Quit2 = inputStates[(int)NetworkPlugin.InputIDs.ATTACK] == 2;

                if (Quit1 && Quit2)
                {
                    Debug.Log("Quit Game");
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene("Menu");
                }
            }
        }
    }
}