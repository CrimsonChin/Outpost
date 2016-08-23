using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnScript : MonoBehaviour
{
    public GameObject Enemy;             
    public float SpawnTime = 3f;
    public float MaxEnemies = 10f;
    public Transform[] SpawnPoints;

    void Start ()
    {
        Spawn();
        InvokeRepeating("Spawn", SpawnTime, SpawnTime);
    }
	
	void Spawn()
    {
        if (GameObject.FindGameObjectsWithTag("Spider").Length > MaxEnemies)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, SpawnPoints.Length);
        Transform spawnPoint = SpawnPoints[spawnPointIndex];
        if (spawnPoint != null)
        {
            Instantiate(Enemy, SpawnPoints[spawnPointIndex].position, Quaternion.identity);
        }

        if (CheckIfAllNestsDestroyed())
        {
            Application.LoadLevel("PlayerWin");
        }
    }

    private bool CheckIfAllNestsDestroyed()
    {
        bool isEmpty = true;
        foreach (Transform transform in SpawnPoints)
        {
            if (transform != null)
            {
                isEmpty = false;
                break;
            }
        }

        return isEmpty;
    }

    void OnDisable()
    {
        CancelInvoke("Spawn");
    }

    void OnDestroy()
    {
        CancelInvoke("Spawn");
    }
}
