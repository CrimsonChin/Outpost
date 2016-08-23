using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float Toughness = 2f;

    public float Health;
    private HealthBar _healthBar;

    void Start()
    {
        _healthBar = GetComponent<HealthBar>();
        Health = Toughness;
    }

    public void Damage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
        
        if (_healthBar != null)
        {
            _healthBar.SetHealth(Mathf.Clamp(Health / Toughness, 0, 1));
        }
    }
}
