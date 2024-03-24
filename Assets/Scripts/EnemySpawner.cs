using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{

    public static EnemySpawner instance;
    
    

    public List<GameObject> Path1 = new List<GameObject>();
    public List<GameObject> Path2 = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();
    
    private int ufoCounter = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void SpawnEnemy(int type, Enums.Path path)
    {
        // Instantiate een nieuwe vijand
        var newEnemy = Instantiate(Enemies[type], Path1[0].transform.position, Quaternion.identity);

        // Haal het UFO-script op dat is gekoppeld aan de nieuw gespawnde vijand
        var ufoScript = newEnemy.GetComponent<UFO>();

        // Controleer of het UFO-script niet null is voordat je verder gaat
        if (ufoScript != null)
        {
            // Stel het pad in voor de vijand
            ufoScript.path = path;

            // Vraag het eerste waypoint aan voor het opgegeven pad
            ufoScript.target = EnemySpawner.instance.RequestTarget(path, 0);

            // Roep AddInGameEnemy aan op de GameManager
            GameManager.Instance.AddInGameEnemy();
        }
        else
        {
            Debug.LogError("UFO script not found on spawned enemy.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Verwijder of commenteer de volgende regel uit
        // InvokeRepeating("SpawnTester", 1f, 1f);
    }



    private void SpawnTester()
    {

       SpawnEnemy(0, Enums.Path.Path1);

    }

    public GameObject RequestTarget(Enums.Path path, int index)
    {
        List<GameObject> selectedPath = null;

        // Kies het pad op basis van de opgegeven Path enum
        switch (path)
        {
            case Enums.Path.Path1:
                selectedPath = Path1;
                break;
            case Enums.Path.Path2:
                selectedPath = Path2;
                break;
            default:
                Debug.LogError("Unsupported path.");
                break;
        }

        // Controleer of het indexnummer binnen de grenzen van het geselecteerde pad ligt
        if (index >= 0 && index < selectedPath.Count)
        {
            // Verhoog de index en retourneer het volgende waypoint
            return selectedPath[index];
        }
        else
        {
            // Als de index buiten de grenzen van het pad ligt, retourneer null
            return null;
        }
        
    }
    public void StartWave(int number)
    {
        ufoCounter = 0; // Reset de teller voor elke nieuwe golf

        switch (number)
        {
            case 1:
                InvokeRepeating("StartWave1", 1f, 1.5f);
                break;
                // Voeg hier cases toe voor toekomstige golven
        }
    }
    public void StartWave1()
    {
        ufoCounter++;

        if (ufoCounter % 6 <= 1) return; // Laat enkele gaten tussen de spawns

        if (ufoCounter < 30)
        {
            SpawnEnemy(0, Enums.Path.Path1);
        }
        else
        {
            SpawnEnemy(1, Enums.Path.Path1); // De laatste vijand van deze golf is level 2
        }

        if (ufoCounter > 30)
        {
            CancelInvoke("StartWave1"); // Stop deze golf
            GameManager.Instance.EndWave(); // Laat de GameManager weten dat de golf voorbij is
        }
    }
}
