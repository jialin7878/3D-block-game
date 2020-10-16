using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] blocks;

    public void spawnNext()
    {
        int i = Random.Range(0, blocks.Length);

        Instantiate(blocks[i], transform.position, Quaternion.identity);
    }

}
