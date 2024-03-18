using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSite
{
    

    public Vector3Int TilePosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public Enums.SiteLevel Level { get; private set; }
    public Enums.TowerType TowerType { get; private set; }

    private GameObject tower;

    public ConstructionSite(Vector3Int tilePosition, Vector3 worldPosition)
    {
        TilePosition = tilePosition;
        // Pas de Y waarde aan zoals gespecificeerd
        WorldPosition = new Vector3(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z);
        tower = null; // Stel de toren in op null
    }

    public void SetTower(GameObject newTower, Enums.SiteLevel level, Enums.TowerType type)
    {
        // Controleer of er al een toren aanwezig is
        if (tower != null)
        {
            // Verwijder het huidige toren GameObject
            GameObject.Destroy(tower);
        }

        // Update de toren naar de nieuwe toren
        tower = newTower;
        Level = level;
        TowerType = type;

        // Optioneel: Je kunt hier ook de positie van de nieuwe toren instellen
         tower.transform.position = WorldPosition;
    }

    public void RemoveTower()
    {
        if (tower != null)
        {
            GameObject.Destroy(tower);
            tower = null; // Verwijder de referentie naar de toren
        }

        // Reset de eigenschappen van de bouwplaats
        Level = Enums.SiteLevel.Onbebouwd;
        TowerType = default; // Gebruik 'default' om de standaardwaarde voor de enum in te stellen, mogelijk Enums.TowerType.None als je zo'n waarde hebt.
    }
}
