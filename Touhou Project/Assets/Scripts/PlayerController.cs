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

    void Start()
    {
        mPlayerMotor = GetComponent<PlayerMotor>();
        mPlayerAnimator = GetComponent<Animator>();
        if (name == "Player1")
        {
            mPlayerIndex = 1;
        }
        else if (name == "Player2")
        {
            mPlayerIndex = 0;
        }
    }

    void Update()
    {
        float _horizontal = Input.GetAxis(prefix[mPlayerIndex] + "Horizontal");
        mPlayerMotor.Move(_horizontal);

        mPlayerAnimator.SetFloat("Horizontal", Mathf.Abs(_horizontal));

        float _vertical = Input.GetAxis(prefix[mPlayerIndex] + "Vertical");

        mPlayerMotor.FastFall(_vertical);



        bool _jump = Input.GetButtonDown(prefix[mPlayerIndex] + "Jump");

        bool _attack = Input.GetButtonDown(prefix[mPlayerIndex] + "Attack");

        if (gameObject.GetComponent<ReimuInput>() != null)
        {
            gameObject.GetComponent<ReimuInput>().Attack(_attack, _horizontal, _vertical);
        }
        else if(gameObject.GetComponent<MarisaInput>() != null)
        {
            gameObject.GetComponent<MarisaInput>().Attack(_attack, _horizontal, _vertical);
        }

        if (_jump)
        {
            mPlayerMotor.Jump();
        }

    }
}