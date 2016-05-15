using UnityEngine;
using System.Collections;

public class WeaponLootItemScript : MonoBehaviour
{
    public float Damage;

    public GameObject BulletPrefab;

    public float FireDelay;

    public float Speed;

    public bool isPerishable = true;

    void Start()
    {
        if (isPerishable)
        {
            Destroy(gameObject, 3);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<FireWeapon>().EquipWeapon(this);
            Destroy(gameObject);
        }
    }
}
