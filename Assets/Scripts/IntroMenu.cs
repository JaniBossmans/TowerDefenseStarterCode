using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{
    private TextField nameField;
    private Button playButton;
    private Button quitButton;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        nameField = root.Q<TextField>("Name");
        playButton = root.Q<Button>("Play-Button");
        quitButton = root.Q<Button>("Quit-Button");

        playButton.clicked += OnPlayButtonClick;
        quitButton.clicked += OnQuitButtonClick;

        playButton.SetEnabled(false);

        if (nameField != null)
        {
            nameField.RegisterValueChangedCallback(evt =>
            {
                OnNameValueChanged(evt.newValue);
            });
        }
    }

    private void OnPlayButtonClick()
    {
        SoundManager.Instance.PlayUISound();
        SceneManager.LoadScene("GameScene");
    }

    private void OnQuitButtonClick()
    {
        SoundManager.Instance.PlayUISound();
        // Application.Quit does not work in the editor, so this will be empty for now
    }

    private void OnNameValueChanged(string newName)
    {
        playButton.SetEnabled(newName.Length > 2);

        // Update de PlayerName property in de HighScoreManager
        if (HighScoreManager.Instance != null)
        {
            HighScoreManager.Instance.PlayerName = newName;
        }
    }

    private void OnDestroy()
    {
        playButton.clicked -= OnPlayButtonClick;
        quitButton.clicked -= OnQuitButtonClick;
    }

}
