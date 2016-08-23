using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private float _damage = 2f;

    public AudioClip OnDestroySound;

    public void Setup(Vector2 speed, float damage)
    {
        GetComponent<Rigidbody2D>().velocity = speed;
        _damage = damage;

        Destroy(gameObject, 2);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.Instance.PlaySound(OnDestroySound);
        Destroy(gameObject);

        Destroyable destroyable = other.gameObject.GetComponent<Destroyable>();
        if (destroyable)
        {
            destroyable.Damage(_damage);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
