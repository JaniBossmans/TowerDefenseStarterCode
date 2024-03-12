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
            // Target no longer exists, destroy the projectile
            Destroy(gameObject);
        }
        else
        {
            // Move the projectile towards the target
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            // Check if the projectile is close enough to the target
            if (Vector3.Distance(transform.position, target.position) < 0.2f)
            {
                // Destroy the projectile
                Destroy(gameObject);
            }
        }
    }

}
