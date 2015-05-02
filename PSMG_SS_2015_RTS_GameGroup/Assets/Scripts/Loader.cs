﻿using UnityEngine;
using System.Collections;

/*
 *  Loads the game manager and is called when the game is started.   
 */

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    
    void Awake()
    {
        Instantiate(gameManager);
    }

}
