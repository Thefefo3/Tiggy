using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    public GameObject powerUp;
    public float spawnWait = 100;
    public float spawnMostWait;
    public float spawnLeasttWait;
    public int startWait = 0;

    int randPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            randPowerUp = Random.Range(0,1); //(min, max)

            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Instantiate(powerUp, spawnPosition + transform.TransformPoint(0,0,0), gameObject.transform.rotation);

            yield return new WaitForSeconds(spawnWait);
        }
    }
}
