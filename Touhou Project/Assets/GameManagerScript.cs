using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(playerID == 0)
        {
            player0 = characterPrefabs[charID]; 
        }
        else
        {
            player1 = characterPrefabs[charID];
        }

    }

}
