using UnityEngine;
using System.Collections;

public class Tower : MonoBehaviour
{
    public float attackRange = 1f; // Range within which the tower can detect and attack enemies 
    public float attackRate = 1f; // How often the tower attacks (attacks per second) 
    public int attackDamage = 1; // How much damage each attack does 
    public float attackSize = 1f; // How big the bullet looks 
    public GameObject bulletPrefab; // The bullet prefab the tower will shoot 
    public Enums.TowerType type; // the type of this tower 

    private float attackTimer;

    // Draw the attack range in the editor for easier debugging 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the attack timer
        attackTimer += Time.deltaTime;
        
        // Check if it's time to attack
        if (attackTimer >= 1f / attackRate)
        {
            attackTimer = 0f;
            Attack();
        }
    }

    void Attack()
    {
        // Find all enemies within the attack range
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        // Check each enemy and attack the first one found
        foreach (Collider2D enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                GameObject enemy = enemyCollider.gameObject;

                // Instantiate the bullet prefab
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // Set the projectile properties
                Projectile projectile = bullet.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.target = enemy.transform;
                    projectile.damage = attackDamage;
                }

                // Set the bullet size
                bullet.transform.localScale = new Vector3(attackSize, attackSize, 1f);

                // Break after attacking the first enemy found
                break;
            }
        }
    }
}