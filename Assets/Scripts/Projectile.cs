using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;
    public float speed;
    public int damage;

    // Start is called before the first frame update 
    void Start()
    {
        if (target != null)
        {
            // Rotate the projectile to face the target
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            // Doelwit bestaat niet meer, vernietig het projectiel
            Destroy(gameObject);
        }
        else
        {
            // Beweeg het projectiel richting het doelwit
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            // Controleer of het projectiel dichtbij genoeg is bij het doelwit
            if (Vector3.Distance(transform.position, target.position) < 0.2f)
            {
                // Hier, net voordat je het projectiel vernietigt, breng schade toe
                UFO enemy = target.GetComponent<UFO>(); // Probeer de UFO component van het doelwit te krijgen
                if (enemy != null) // Controleer of het doelwit daadwerkelijk een UFO is
                {
                    enemy.Damage(damage); // Breng schade toe met de schadewaarde van het projectiel
                }

                // Vernietig het projectiel
                Destroy(gameObject);
            }
        }
    }

}
