using UnityEngine;
using UnityEngine.UIElements;

public class TopMenu : MonoBehaviour
{
    private Label waveLabel;
    private Label creditsLabel;
    private Label gateHealthLabel;
    private Button playButton;

    private VisualElement root;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        waveLabel = root.Q<Label>("Wave");
        creditsLabel = root.Q<Label>("Credits");
        gateHealthLabel = root.Q<Label>("Gate-Health");
        playButton = root.Q<Button>("Play-Button");

        if (playButton != null)
        {
            playButton.clicked += OnPlayButtonClicked;
        }
    }

    public void SetWaveLabel(string text)
    {
        if (waveLabel != null)
        {
            waveLabel.text = text;
        }
    }

    public void SetCreditsLabel(string text)
    {
        if (creditsLabel != null)
        {
            creditsLabel.text = text;
        }
    }

    public void SetGateHealthLabel(string text)
    {
        if (gateHealthLabel != null)
        {
            gateHealthLabel.text = text;
        }
    }

    private void OnPlayButtonClicked()
    {
        // Start the wave
    }

    private void OnDestroy()
    {
        if (playButton != null)
        {
            playButton.clicked -= OnPlayButtonClicked;
        }
    }
}
