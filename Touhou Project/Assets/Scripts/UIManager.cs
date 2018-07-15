﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void Play()
   {
      Debug.Log("Play");
      SceneManager.LoadScene("Main");
   }

   public void Exit()
   {
      Debug.Log("Quit");
      Application.Quit();
   }
   
}
