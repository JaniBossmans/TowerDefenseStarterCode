using UnityEngine;

public class ScreenLoader : MonoBehaviour
{
    public bool MenuScene; // Deze variabele bepaalt of de huidige scène een menu-scène is

    void Start()
    {
        // Controleer of de SoundManager bestaat om fouten te voorkomen
        if (SoundManager.Instance != null)
        {
            // Start de juiste muziek afhankelijk van of het een menu-scène is
            if (MenuScene)
            {
                SoundManager.Instance.StartMenuMusic();
            }
            else
            {
                SoundManager.Instance.StartGameMusic();
            }
        }
        else
        {
            Debug.LogError("SoundManager instance not found.");
        }
    }
}
