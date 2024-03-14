using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject TowerMenu;
    private TowerMenu towerMenu;

    public List<GameObject> Archers;
    public List<GameObject> Swords;
    public List<GameObject> Wizards;

    // Variabele voor het bijhouden van de geselecteerde ConstructionSite
    private ConstructionSite selectedSite;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Maak het persistent tussen scenes
        }
        else
        {
            Destroy(gameObject); // Zorgt ervoor dat er geen dubbele instanties zijn
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        towerMenu = TowerMenu.GetComponent<TowerMenu>();
    }

    public void SelectSite(ConstructionSite site)
    {
        // Onthoud de geselecteerde site
        selectedSite = site;

        // Controleer of towerMenu niet null is
        if (towerMenu != null)
        {
            // Gebruik de reeds bestaande referentie naar TowerMenu
            towerMenu.SetSite(site);
        }
        else
        {
            // Log een fout als TowerMenu om een of andere reden null is.
            Debug.LogError("TowerMenu component is null in GameManager.");
        }
    }
}
