using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingSpikes : MonoBehaviour
{
    public Transform SpawnPoint;
    public GameObject spike;
    public float reload;
    public bool waves;
    public int ProjectilesCountInWave;
    public float reloadwaves;
    void Start()
    {
        if (waves)
            StartCoroutine(generateObjWithWaves());
        else
            StartCoroutine(generateObj());
    }
    private IEnumerator generateObj()
    {
        while (true) {           
            Instantiate(spike, SpawnPoint.position, SpawnPoint.rotation);
            yield return new WaitForSeconds(reload);
        }
    }
    private IEnumerator generateObjWithWaves()
    {
        while (true)
        {
            for (int i = 0; i < ProjectilesCountInWave; ++i) {                
                Instantiate(spike, SpawnPoint.position, SpawnPoint.rotation);
                yield return new WaitForSeconds(reload);
            }
            yield return new WaitForSeconds(reloadwaves);
        }
    }

}
