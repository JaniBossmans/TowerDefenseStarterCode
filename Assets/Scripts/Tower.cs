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
        // Vind alle vijanden binnen het aanvalsbereik
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        // Check elke vijand en val de eerste aan die je vindt
        foreach (Collider2D enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                GameObject enemy = enemyCollider.gameObject;

                // Instantieer het kogel prefab
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

                // Configureer de projectieleigenschappen
                Projectile projectile = bullet.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.target = enemy.transform;
                    projectile.damage = attackDamage;

                    // Speel het torengeluid af na het schieten
                    SoundManager.Instance.PlayTowerSound(type);
                }

                // Stel de grootte van de kogel in
                bullet.transform.localScale = new Vector3(attackSize, attackSize, 1f);

                // Onderbreek na de eerste gevonden vijand
                break;
            }
        }
    }

}