using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   int mPlayerIndex;

   string[] prefix = { "P1 ", "P2 " };
   private Animator mPlayerAnimator;
   private PlayerMotor mPlayerMotor;

   public bool mCanInput = true;


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
            if (Time.timeScale == 0.0f)
            {
               Time.timeScale = 1.0f;
            }
            else
            {;
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
         else if(Time.timeScale == 0.0f)
         {
            
            bool Quit1 = Input.GetButton(prefix[mPlayerIndex] + "Jump");

            bool Quit2 = Input.GetButton(prefix[mPlayerIndex] + "Attack");

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