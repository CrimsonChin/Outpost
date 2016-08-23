using UnityEngine;


public class FireWeapon : MonoBehaviour
{
    private bool _hasWeaponEquipped;
    private bool _canFire = true;

    public float FireDelayCounter;

    private Movement _playerMovement;

    private float _weaponSpeed;
    private GameObject _weaponPrefab;
    private float _weaponDamage;
    private float _weaponFireDelay;

    void Start()
    {
        _playerMovement = GetComponent<Movement>();
    }

    void Update()
    {
        if (Input.GetAxis("Fire1") > 0 && _hasWeaponEquipped && _canFire)
        {
            FireBullet(_playerMovement.FacingDirection);
        }

        if (FireDelayCounter > 0)
        {
            FireDelayCounter--;
        }
        else
        {
            _canFire = true;
        }
    }

    public void EquipWeapon(WeaponLootItemScript weapon)
    {
        _hasWeaponEquipped = true;

        // SET THE PROPERTIES THE WEAPON IS ABOUT TO BE DESTROYED
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

        _canFire = false;
        FireDelayCounter = _weaponFireDelay;
    }
}
