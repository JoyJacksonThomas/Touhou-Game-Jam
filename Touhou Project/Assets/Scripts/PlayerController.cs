﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
   int mPlayerIndex;

   string[] prefix = { "P1 ", "P2 "};
   private Animator mPlayerAnimator;
   private PlayerMotor mPlayerMotor;

   void Start()
   {
      mPlayerMotor = GetComponent<PlayerMotor>();
      mPlayerAnimator = GetComponent<Animator>();
      if (name == "Player1")
      {
         mPlayerIndex = 0;
      }
      else if(name == "Player2")
      {
         mPlayerIndex = 1;
      }
   }

   void Update()
   {
      float _horizontal = Input.GetAxis( prefix[mPlayerIndex] + "Horizontal");
      mPlayerMotor.Move(_horizontal);
      
      mPlayerAnimator.SetFloat("Horizontal", Mathf.Abs(_horizontal));

      float _vertical = Input.GetAxis( prefix[mPlayerIndex] + "Vertical");
      
      mPlayerMotor.FastFall(_vertical);

      

      bool _jump = _jump = Input.GetButtonDown( prefix[mPlayerIndex] + "Jump");
      if (_jump)
      {
         mPlayerMotor.Jump();
      }

   }
}