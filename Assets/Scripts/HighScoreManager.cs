using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; private set; }

    public string PlayerName { get; set; }
    public bool GameIsWon { get; set; }

    public class HighScore
    {
        public string Name;
        public int Score;
    }

    public List<HighScore> HighScores = new List<HighScore>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScoresFromFile(); // Load the high scores as soon as the instance is set
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddHighScore(int score, string playerName)
    {
        // Check if the new score is higher than any existing scores
        if (HighScores.Count < 5 || score > HighScores[HighScores.Count - 1].Score)
        {
            HighScores.Add(new HighScore { Name = playerName, Score = score });
            HighScores.Sort((x, y) => y.Score.CompareTo(x.Score)); // Sort descending

            // Keep only top 5 scores
            if (HighScores.Count > 5)
            {
                HighScores.RemoveRange(5, HighScores.Count - 5);
            }

            SaveHighScoresToFile();
        }
    }

    private void SaveHighScoresToFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        string json = JsonUtility.ToJson(new { HighScores = HighScores }, true);
        File.WriteAllText(filePath, json);
    }

    private void LoadHighScoresFromFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "highscores.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
}
