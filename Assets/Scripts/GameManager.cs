using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject TowerMenu;
    private TowerMenu towerMenu;

    public GameObject TopMenu;
    private TopMenu topMenu;

    private ConstructionSite selectedSite;
    public List<GameObject> Archers;
    public List<GameObject> Swords;
    public List<GameObject> Wizards;

    private int credits = 200, health = 10, currentWave = 0;
    private bool waveActive = false;

    private int enemyInGameCounter = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        towerMenu = TowerMenu.GetComponent<TowerMenu>();
        topMenu = TopMenu.GetComponent<TopMenu>();
        StartGame();
    }

    void StartGame()
    {
        credits = 200;
        health = 10;
        currentWave = 0;
        waveActive = false; // Zorg ervoor dat waveActive false is bij het starten van het spel
        UpdateLabels();
    }

    public void AddInGameEnemy()
    {
        enemyInGameCounter++;
    }

    public void RemoveInGameEnemy()
    {
        enemyInGameCounter--;
        if (!waveActive && enemyInGameCounter <= 0)
        {
            if (!waveActive && enemyInGameCounter <= 0) 
            {
                // Logica voor het einde van de game
            }
            else
            {
                // Activeer de wave button in het top menu
                topMenu.EnableWaveButton();
            }
        }
    }

    public void StartWave()
    {
        if (!waveActive)
        {
            waveActive = true;
            currentWave++;
            enemyInGameCounter = 0; // Reset de teller aan het begin van de wave
            UpdateLabels();
            EnemySpawner.instance.StartWave(currentWave);
        }
    }

    public void EndWave()
    {
        waveActive = false;
    }

    private void UpdateLabels()
    {
        topMenu.SetCreditsLabel("Credits: " + credits);
        topMenu.SetGateHealthLabel("Health: " + health);
        topMenu.SetWaveLabel("Wave: " + currentWave);
    }


    public void AttackGate()
    {
        health -= 1;
        topMenu.SetGateHealthLabel("Health: " + health);
    }

    public void AddCredits(int amount)
    {
        credits += amount;
        topMenu.SetCreditsLabel("Credits: " + credits);
        // Hier toekomstige logica voor towerMenu evaluatie
    }

    public void RemoveCredits(int amount)
    {
        credits -= amount;
        topMenu.SetCreditsLabel("Credits: " + credits);
        // Hier toekomstige logica voor towerMenu evaluatie
    }

    public int GetCredits()
    {
        return credits;
    }

    public int GetCost(Enums.TowerType type, Enums.SiteLevel level, bool selling = false)
    {
        // Basis kosten bepalen op basis van type en level, dit is een voorbeeld
        int cost = 100; // Stel dit in op de daadwerkelijke kosten

        // Pas de kosten aan op basis van of het een verkoop is
        if (selling)
        {
            // Verkoopwaarde is bijvoorbeeld de helft van de aankoopprijs
            return cost / 2;
        }
        else
        {
            return cost;
        }
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
            return;
        }

        // Logica voor het aanpassen van credits afhankelijk van aankoop of verkoop
        if (level == Enums.SiteLevel.Onbebouwd)
        {
            // Verkooplogica
            AddCredits(GetCost(type, selectedSite.Level, true));
        }
        else
        {
            // Aankooplogica
            int cost = GetCost(type, level);
            if (GetCredits() >= cost)
            {
                RemoveCredits(cost);
            }
            else
            {
                Debug.LogError("Niet genoeg credits om de toren te bouwen.");
                return;
            }
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

        // Bereken de verkoopwaarde van de toren
        int sellValue = GetCost(selectedSite.TowerType, selectedSite.Level, selling: true);

        // Voeg de verkoopwaarde toe aan de spelercredits
        AddCredits(sellValue);

        // Roep de RemoveTower methode aan van de selectedSite
        selectedSite.RemoveTower();

        // Verberg het towerMenu als dat nodig is
        if (towerMenu != null)
        {
            towerMenu.SetSite(null);
        }
    }
}
