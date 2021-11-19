using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{

    public GameObject powerUp;
    public float spawnWait = 500;
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
        //var newPowerUp = null;
        while (true)
        {
            randPowerUp = Random.Range(0,1); //(min, max)

            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            var newPowerUp = Instantiate(powerUp, transform.position + Vector3.up , gameObject.transform.rotation);

            while(newPowerUp != null)
            {
                yield return new WaitForSeconds(spawnWait);
            }

            //yield return new WaitForSeconds(spawnWait);
        }
    }
}
