using UnityEngine;
using System.Collections;
using System.Linq;

public class SpawnScript : MonoBehaviour
{
    public GameObject enemy;             
    public float spawnTime = 3f;
    public float maxEnemies = 10f;
    public Transform[] spawnPoints;

    void Start ()
    {
        Spawn();
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }
	
	void Spawn()
    {
        if (GameObject.FindGameObjectsWithTag("Spider").Count() > maxEnemies)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnPointIndex];
        if (spawnPoint != null)
        {
            Instantiate(enemy, spawnPoints[spawnPointIndex].position, Quaternion.identity);
        }

        if (CheckIfAllNestsDestroyed())
        {
            Application.LoadLevel("PlayerWin");
        }
    }

    private bool CheckIfAllNestsDestroyed()
    {
        bool isEmpty = true;
        foreach (Transform transform in spawnPoints)
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
