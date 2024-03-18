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

    public void Build(Enums.TowerType type, Enums.SiteLevel level)
    {
        // Controleer of er een site geselecteerd is. Zo niet, log een fout en keer terug.
        if (selectedSite == null)
        {
            Debug.LogError("Er is geen bouwplaats geselecteerd. Kan de toren niet bouwen.");
            return; // Stop de methode hier als er geen site geselecteerd is.
        }

        GameObject towerPrefab = null;

        // Trek 1 af van de level waarde om de correcte index te krijgen
        int prefabIndex = (int)level - 1;

        switch (type)
        {
            case Enums.TowerType.Archer:
                towerPrefab = Archers[prefabIndex];
                break;
            case Enums.TowerType.Sword:
                towerPrefab = Swords[prefabIndex];
                break;
            case Enums.TowerType.Wizard:
                towerPrefab = Wizards[prefabIndex];
                break;
        }

        if (towerPrefab == null)
        {
            Debug.LogError("Geen tower prefab gevonden voor het geselecteerde type en niveau.");
            return;
        }

        // Gebruik de WorldPosition van de selectedSite voor het positioneren van de nieuwe toren
        GameObject tower = Instantiate(towerPrefab, selectedSite.WorldPosition, Quaternion.identity);

        // Gebruik de SetTower methode van de ConstructionSite om de nieuwe toren in te stellen en te configureren
        selectedSite.SetTower(tower, level, type);

        if (towerMenu != null)
        {
            towerMenu.SetSite(null); // Verberg het towerMenu
        }
    }

    public void DestroyTower()
    {
        if (selectedSite == null)
        {
            Debug.LogError("Er is geen bouwplaats geselecteerd. Kan de toren niet verwijderen.");
            return;
        }

        // Roep de RemoveTower methode aan van de selectedSite
        selectedSite.RemoveTower();

        // Verberg het towerMenu als dat nodig is
        if (towerMenu != null)
        {
            towerMenu.SetSite(null);
        }
    }
}
