using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HighScoreMenu : MonoBehaviour
{
    private Button playAgainButton;
    private Label winLoseLabel;
    private UIDocument uiDocument;

    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        playAgainButton = uiDocument.rootVisualElement.Q<Button>("Play_Again");
        winLoseLabel = uiDocument.rootVisualElement.Q<Label>("Win-Lose_Label");

        playAgainButton.clicked += OnPlayAgainClicked;
        SetWinLoseLabel();
    }

    private void SetWinLoseLabel()
    {
        if (HighScoreManager.Instance == null)
        {
            Debug.LogError("HighScoreManager instance is null.");
            return;
        }

        if (winLoseLabel == null)
        {
            Debug.LogError("WinLoseLabel is not found.");
            return;
        }

        winLoseLabel.text = HighScoreManager.Instance.GameIsWon ? "You Won!" : "You Lost!";
    }

    private void OnPlayAgainClicked()
    {
        if (HighScoreManager.Instance == null)
        {
            Debug.LogError("HighScoreManager instance is null.");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null.");
            return;
        }

        HighScoreManager.Instance.GameIsWon = false; // Reset the GameIsWon flag
        GameManager.Instance.StartGame();
        SceneManager.LoadScene("GameScene");
    }
    void UpdateHighScoreLabels()
    {
        // Retrieve and update labels for high scores
        for (int i = 0; i < 5; i++)
        {
            Label highScoreLabel = uiDocument.rootVisualElement.Q<Label>($"HighScore{i + 1}");
            if (HighScoreManager.Instance.HighScores.Count > i)
            {
                HighScoreManager.HighScore scoreEntry = HighScoreManager.Instance.HighScores[i];
                highScoreLabel.text = $"{scoreEntry.Name}: {scoreEntry.Score}";
            }
            else
            {
                highScoreLabel.text = "...";
            }
        }
    }
}
