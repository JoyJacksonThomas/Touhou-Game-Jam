﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public int playerID;

    public static GameManagerScript Instance;

    public List<GameObject> characterPrefabs;

    GameObject player0;
    GameObject player1;

    public bool isNetworked;


    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }
    

    public void StartCharacterSelectScreen()
    {
        if(!isNetworked)
        {

        }
    }


    public void SelectCharacter(int charID, int playerID)
    {
        Debug.Log(playerID + " " + charID);
        if (charID == -1)
        {
            //Start Game
            if((player0 && player1))
            {
                SceneManager.LoadScene("Main");
            //if (isNetworked)
            //  NetworkPlugin.Instance.SendMessages(null);
            StartCoroutine(SpawnCharacters());
            }
            return;
        }

        if (playerID == 0)
        {
            player0 = characterPrefabs[charID];
        Debug.Log("player " + playerID + " selected " + player0.name);
            
        }
        else
        {
            player1 = characterPrefabs[charID];
        Debug.Log("player " + playerID + " selected " + player1.name);
        }

    }

    bool startCalled = false;
    IEnumerator SpawnCharacters()
    {
        if (startCalled)
            yield return null;
        startCalled = true;
        while(SceneManager.GetActiveScene().name != "Main")
        {
            yield return new  WaitForEndOfFrame();
        }

        GameObject p0 = (GameObject)Instantiate(player0);
        p0.GetComponent<PlayerController>().mPlayerIndex = 0;
        p0.GetComponent<PlayerController>().mCanInput = false;        
        p0.GetComponent<PlayerController>().mClient = isNetworked && NetworkPlugin.Instance.userIdentifier == 1;
        p0.transform.position = new Vector3(-2.5f, 0, 0);
        GameObject p1 = (GameObject)Instantiate(player1);
        p1.GetComponent<PlayerController>().mPlayerIndex = 1;
        p1.GetComponent<PlayerController>().mCanInput = false;
        p1.GetComponent<PlayerController>().mClient = isNetworked && NetworkPlugin.Instance.userIdentifier == 0;
        p1.transform.position = new Vector3(2.5f, 0, 0);
        if (isNetworked)
        {
            NetworkPlugin.Instance.Players.Clear();
            NetworkPlugin.Instance.Players.Add(0, p0);
            NetworkPlugin.Instance.Players.Add(1, p1);
        }

        SmashBrosCam smc = GameObject.FindObjectOfType<SmashBrosCam>();
        while (smc == null)
        {
            smc = GameObject.FindObjectOfType<SmashBrosCam>();
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.1f);
        smc.mTargets = new List<Transform>();
        smc.mTargets.Add(p0.transform);
        smc.mTargets.Add(p1.transform);
        smc.StartUp();


    }

}
