using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        
        if (charID == -1 && (player0 && player1))
        {
            //Start Game

            SceneManager.LoadScene("Main");
            //if (isNetworked)
              //  NetworkPlugin.Instance.SendMessages(null);
            StartCoroutine(SpawnCharacters());
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
        GameObject p1 = (GameObject)Instantiate(player1);
        p1.GetComponent<PlayerController>().mPlayerIndex = 1;
        p1.GetComponent<PlayerController>().mCanInput = false;

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
