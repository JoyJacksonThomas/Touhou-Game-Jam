﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public bool isClient;


    // Update is called once per frame
    void Update()
    {
        if (isClient)
            return;

        Vector2 pos = transform.position;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            pos.x += 20;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            pos.x -= 20;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            pos.y += 20;
        if (Input.GetKeyDown(KeyCode.DownArrow))
            pos.y -= 20;

       transform.position = pos;
    }
}
