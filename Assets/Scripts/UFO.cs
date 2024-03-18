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
                target = EnemySpawner.instance.RequestTarget(path, pathIndex); // Zorg ervoor dat EnemySpawner.instance correct is geïnitialiseerd
                pathIndex++;

                // Als het doel null is, hebben we het einde van het pad bereikt. 
                // Vernietig de vijand op dat moment 
                if (target == null)
                {
                    Destroy(gameObject);
                }
            }
        }
        else {
            Debug.LogError("target is null");
        }
    }

    public void Damage(int damageAmount)
    {
        // Verlaag de gezondheidswaarde met de ontvangen schade
        health -= damageAmount;

        // Controleer of de gezondheid kleiner of gelijk aan nul is
        if (health <= 0)
        {
            // Vernietig de UFO GameObject
            Destroy(gameObject);
        }
    }
}
        
    

    

