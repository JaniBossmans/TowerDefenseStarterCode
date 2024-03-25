using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }

    public string PlayerName { get; set; }
    public bool GameIsWon { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zorgt ervoor dat dit object niet vernietigd wordt bij het laden van een nieuwe scene.
        }
        else if (Instance != this)
        {
            Destroy(gameObject); // Zorgt ervoor dat er geen dubbele instanties zijn.
        }
    }
}
