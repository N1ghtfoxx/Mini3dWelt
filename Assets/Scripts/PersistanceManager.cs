using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// summary:
// This script manages the persistence of game objects across scene loads.
// It ensures that the game object this script is attached to is not destroyed when loading a new scene.
public class PersistanceManager : MonoBehaviour
{
    private static PersistanceManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}