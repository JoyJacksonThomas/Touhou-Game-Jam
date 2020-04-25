using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    public bool isClient;
    public int playerID;
    RectTransform rectTransform;

    List<RectTransform> characterIcons = new List<RectTransform>();
    List<CharacterSelectImageScript> characters = new List<CharacterSelectImageScript>();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        CharacterSelectImageScript[] foundcharacters = GameObject.FindObjectsOfType<CharacterSelectImageScript>();
        foreach(CharacterSelectImageScript c in foundcharacters)
        {
            characterIcons.Add(c.GetComponent<RectTransform>());
            characters.Add(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClient)
            return;

        Vector2 pos = transform.position;

        pos.x += (0.1f * (int)Input.GetAxisRaw(PlayerController.prefix[GameManagerScript.Instance.isNetworked ? 0 : playerID] + "Horizontal"));
        pos.y += (0.1f * (int)Input.GetAxisRaw(PlayerController.prefix[GameManagerScript.Instance.isNetworked ? 0 : playerID] + "Vertical"));

       rectTransform.position = pos;

        if ((int)Input.GetAxisRaw(PlayerController.prefix[GameManagerScript.Instance.isNetworked ? 0 : playerID] + "Attack") == 1)
        {
            CheckOver();
        }
        //
    }

    public void CheckOver()
    {
        
        for (int i = 0; i < characterIcons.Count; i++)
        {
            Debug.Log("Hyuck " + characterIcons[i].anchorMax.x + " " + characterIcons[i].anchorMax.y);
            if (CheckPos(characterIcons[i].position.x, characterIcons[i].position.y,
                characterIcons[i].anchorMax.x, characterIcons[i].anchorMax.y))
            {
                GameManagerScript.Instance.SelectCharacter(characters[i].characterID, playerID);
                GetComponent<Image>().color = Color.red;
            }
        }
    }


    bool CheckPos(float x, float y, float width, float height)
    {
        bool check;
        check =( rectTransform.position.x < (x + (width)) &&
            rectTransform.position.x > (x - (width)) ) &&
            (rectTransform.position.y < (y + (height)) &&
            rectTransform.position.y > (y- (height)));
        return check;
    }

}
