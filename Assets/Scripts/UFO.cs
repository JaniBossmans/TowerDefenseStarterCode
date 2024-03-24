using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public float speed = 1f;
    public float health = 10f;
    public int points = 1;

    public Enums.Path path { get; set; }
    public GameObject target { get; set; }
    private int pathIndex = 1;


    public void Damage(int damageAmount)
    {
        // Verlaag de gezondheidswaarde met de ontvangen schade
        health -= damageAmount;

        // Controleer of de gezondheid kleiner of gelijk aan nul is
        if (health <= 0)
        {
            // Voeg credits toe aan de speler
            GameManager.Instance.AddCredits(points);

            // Roep RemoveInGameEnemy aan op de GameManager voordat de UFO wordt vernietigd
            GameManager.Instance.RemoveInGameEnemy();

            // Vernietig de UFO GameObject
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (target != null) // Controleer of target niet null is
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);

            // Controleer hoe dicht we bij het doel zijn 
            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                // Als we dichtbij zijn, vraag dan een nieuw waypoint aan 
                target = EnemySpawner.instance.RequestTarget(path, pathIndex);
                pathIndex++;

                // Als het doel null is, hebben we het einde van het pad bereikt. 
                if (target == null)
                {
                    GameManager.Instance.AttackGate();

                    // Roep RemoveInGameEnemy aan op de GameManager voordat de vijand wordt vernietigd
                    GameManager.Instance.RemoveInGameEnemy();

                    // Vernietig de vijand
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Debug.LogError("target is null");
        }
    }
}
        
    

    

