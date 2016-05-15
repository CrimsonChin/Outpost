using UnityEngine;


public class FireWeapon : MonoBehaviour
{
    private bool hasWeaponEquipped = false;
    private bool canFire = true;

    public float fireDelayCounter = 0;

    private Movement playerMovement;

    private float _weaponSpeed;
    private GameObject _weaponPrefab;
    private float _weaponDamage;
    private float _weaponFireDelay;

    // Use this for initialization
    void Start () {
        playerMovement = GetComponent<Movement>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Fire1") > 0 && hasWeaponEquipped && canFire)
        {
            FireBullet(playerMovement.FacingDirection);
        }

        if (fireDelayCounter > 0)
        {
            fireDelayCounter--;
        }
        else
        {
            canFire = true;
        }
    }

    public void EquipWeapon(WeaponLootItemScript weapon)
    {
        hasWeaponEquipped = true;

        // SET THE PROPERTIES THE ITEM IS ABOUT TO BE DESTROYED
        _weaponSpeed = weapon.Speed;
        _weaponPrefab = weapon.BulletPrefab;
        _weaponDamage = weapon.Damage;
        _weaponFireDelay = weapon.FireDelay;
    }

    void FireBullet(Vector2 direction)
    {
        direction.Scale(new Vector2(_weaponSpeed, _weaponSpeed));
    
        Vector3 bulletPosition = gameObject.transform.position;
        bulletPosition.x += direction.x * 0.07f;
        bulletPosition.y += direction.y * 0.07f;
        GameObject bulletInstance = (GameObject)Instantiate(_weaponPrefab, bulletPosition, Quaternion.identity);
        bulletInstance.GetComponent<BulletScript>().Setup(direction, _weaponDamage);

        canFire = false;
        fireDelayCounter = _weaponFireDelay;
    }
}
