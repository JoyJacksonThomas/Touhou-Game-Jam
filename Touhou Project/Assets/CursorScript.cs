using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    public bool isClient;
    public int playerID;

    List<Image> characterIcons = new List<Image>();
    List<CharacterSelectImageScript> characters = new List<CharacterSelectImageScript>();

    private void Awake()
    {
        CharacterSelectImageScript[] foundcharacters = GameObject.FindObjectsOfType<CharacterSelectImageScript>();
        foreach(CharacterSelectImageScript c in foundcharacters)
        {
            characterIcons.Add(c.GetComponent<Image>());
            characters.Add(c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isClient)
            return;

        Vector2 pos = transform.position;

        pos.x += (2 * (int)Input.GetAxisRaw(PlayerController.prefix[GameManagerScript.Instance.isNetworked ? 0 : playerID] + "Horizontal"));
        pos.y += (2 * (int)Input.GetAxisRaw(PlayerController.prefix[GameManagerScript.Instance.isNetworked ? 0 : playerID] + "Vertical"));

       transform.position = pos;
        CheckOver();
    }

    void CheckOver()
    {
        if ((int)Input.GetAxisRaw(PlayerController.prefix[GameManagerScript.Instance.isNetworked ? 0 : playerID] + "Attack") != 1)
            return;
        for (int i = 0; i < characterIcons.Count; i++)
        {
            if (CheckPos(characterIcons[i].rectTransform.position.x, characterIcons[i].rectTransform.position.y,
                characterIcons[i].rectTransform.rect.width, characterIcons[i].rectTransform.rect.height))
            {
                GameManagerScript.Instance.SelectCharacter(characters[i].characterID, playerID);
            }
        }
    }


    bool CheckPos(float x, float y, float width, float height)
    {
        bool check;
        check =( transform.position.x < (x + (width / 2f)) &&
            transform.position.x > (x - (width / 2f)) ) &&
            (transform.position.y < (y + (height/ 2f)) &&
            transform.position.y > (y- (height / 2f)));
        return check;
    }

}
