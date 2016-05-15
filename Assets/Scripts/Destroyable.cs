using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float toughness = 2f;

    public float health;
    
    void Start()
    {
        health = toughness;
    }

	public void Damage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        HealthBar healthBar = GetComponent<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetHealth(Mathf.Clamp(health / toughness, 0, 1));
        }
    }
}
