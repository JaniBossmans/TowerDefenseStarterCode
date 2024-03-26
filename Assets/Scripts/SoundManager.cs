using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource menuMusic;
    public AudioSource gameMusic;
    public GameObject audioSourcePrefab;
    public AudioClip[] uiSounds;

    public AudioClip[] towerSounds; // Array voor torenschoten

    void Awake()
    {
        // Singleton setup to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static SoundManager Instance { get; private set; }

    public void StartMenuMusic()
    {
        if (!menuMusic.isPlaying)
        {
            gameMusic.Stop(); // Ensure game music is stopped
            menuMusic.Play(); // Start playing menu music
        }
    }

    public void StartGameMusic()
    {
        if (!gameMusic.isPlaying)
        {
            menuMusic.Stop(); // Ensure menu music is stopped
            gameMusic.Play(); // Start playing game music
        }
    }

    public void PlayUISound()
    {
        int index = Random.Range(0, uiSounds.Length);

        // Instantiate an AudioSource GameObject
        GameObject soundGameObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();

        // Configure and play the sound
        audioSource.clip = uiSounds[index];
        audioSource.Play();

        // Destroy the AudioSource GameObject after the clip finishes playing
        Destroy(soundGameObject, uiSounds[index].length);
    }

    public void PlayTowerSound(Enums.TowerType towerType)
    {
        AudioClip clip = GetTowerSound(towerType);
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
        }
    }

    private AudioClip GetTowerSound(Enums.TowerType towerType)
    {
        switch (towerType)
        {
            case Enums.TowerType.Archer:
                return towerSounds.Length > 0 ? towerSounds[0] : null;
            case Enums.TowerType.Sword:
                return towerSounds.Length > 1 ? towerSounds[1] : null;
            case Enums.TowerType.Wizard:
                return towerSounds.Length > 2 ? towerSounds[2] : null;
            default:
                return null;
        }
    }
}
