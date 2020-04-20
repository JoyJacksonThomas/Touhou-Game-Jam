using System.Collections;
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

    [SerializeField]
    GameObject startPanel;
    [SerializeField]
    GameObject networkPanel;
    [SerializeField]
    GameObject characterPanel;
    [SerializeField]
    GameObject cursorPrefab;

    public void Play()
   {
      Debug.Log("Play");
      startPanel.SetActive(false);
      networkPanel.SetActive(true);

     // SceneManager.LoadScene("Main");
   }

    public void CharacterSelectStart()
    {
        Debug.Log("Online");
        networkPanel.SetActive(false);
        characterPanel.SetActive(true);

    }
    public void BackButton()
    {
        networkPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void Exit()
   {
      Debug.Log("Quit");
      Application.Quit();
   }

    public void PlayerSelectButton(int id)
    {
        GameManagerScript.Instance.SelectCharacter(id, GameManagerScript.Instance.playerID);
    }

    public void OnlineGame(bool isNetworked)
    {
        GameManagerScript.Instance.isNetworked = isNetworked;
        if(!isNetworked)
        {
        GameObject player2 = (GameObject)Instantiate(cursorPrefab, characterPanel.transform);
        player2.GetComponent<CursorScript>().playerID = 1;
        }
    }
   
}
