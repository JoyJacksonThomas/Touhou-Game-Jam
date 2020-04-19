using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public bool isClient;
    public int playerID;


    // Update is called once per frame
    void Update()
    {
        if (isClient)
            return;

        Vector2 pos = transform.position;
        if (Input.GetKey(KeyCode.RightArrow))
            pos.x += 20;
        if (Input.GetKey(KeyCode.LeftArrow))
            pos.x -= 20;
        if (Input.GetKey(KeyCode.UpArrow))
            pos.y += 20;
        if (Input.GetKey(KeyCode.DownArrow))
            pos.y -= 20;

       transform.position = pos;
    }
}
