using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public GameObject[] blocks;

    public int holdingBlock = -1;
    public int currentBlock;

    private System.Random rng = new System.Random();
    private List<int> order = new List<int>();

    private void Awake()
    {
        generateOrders();
    }

    void generateOrders()
    {
        List<int> list = new List<int>() { 0, 1, 2, 3, 4, 5, 6};
        //shuffle list
        int n = 7;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
        order.AddRange(list);
    }

    public void spawnNext()
    {
        Debug.Log("called spawn next");
        if(order.Count <= 2)
        {
            generateOrders();
        }
        int i = order[0];
        order.RemoveAt(0);
        currentBlock = i;
        Instantiate(blocks[i], transform.position, Quaternion.identity);
    }

    public int getNext()
    {
        return order[0];
    }

    public int holdBlock()
    {
        Debug.Log("called hold block");
        int og = holdingBlock;
        holdingBlock = currentBlock;
        if(og != -1)
        {
            currentBlock = og;
            Instantiate(blocks[og], transform.position, Quaternion.identity);
        }
        return og;
    }
}
