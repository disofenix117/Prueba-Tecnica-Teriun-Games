using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static string nextLevel;

    public static void LoadLevel(string NameLevel)
    {
        nextLevel = NameLevel;
        SceneManager.LoadScene("Loading");
    }
    // Start is called before the first frame update
    
}
