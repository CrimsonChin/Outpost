using UnityEngine;

public class LootableScript : MonoBehaviour
{
    public WeaponLootItemScript[] Weapons;

    private bool isShuttingDown;

    void OnDestroy()
    {
        if (isShuttingDown || Application.isLoadingLevel)
        {
            return;
        }

        bool isLootable = Mathf.Floor(Random.value) == 0;
        if (isLootable)
        {
            GenerateLoot();
        }
    }

    void OnApplicationQuit()
    {
        isShuttingDown = true;
    }

    private void GenerateLoot()
    {
        int randomIndex = Random.Range(0, Weapons.Length);
        Instantiate(Weapons[randomIndex], gameObject.transform.position, Quaternion.identity);
    }
}
