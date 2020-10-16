
using System.Collections.Generic;
using System;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static int w = 5;
    public static int h = 14;
    public static int d = 5;
    public static Transform[,,] grid = new Transform[20, h, 20];

    public static Vector3 roundVec3(Vector3 v)
    {
        //convert into int
        return new Vector3((int) Math.Floor(v.x), (int) Math.Floor(v.y), (int) Math.Floor(v.z));
    }

    public static bool insideBorder(Vector3 pos)
    {
        bool x = pos.x >= 0 && pos.x <= w - 1;
        bool y = pos.y >= 0;
        bool z = pos.z >= 0 && pos.z <= d - 1;
        return x && y && z;
    }

    public static void deleteLayer(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            for(int z = 0; z < d; ++z)
            {
                Destroy(grid[x, y, z].gameObject);
                grid[x, y, z] = null;
            }
        }
        GameManager.manager.incrementScore();
    }

    public static void clearField()
    {
        for (int y = 0; y < h; ++y)
        {
            for (int x = 0; x < w; ++x)
            {
                for (int z = 0; z < d; ++z)
                {
                    if(grid[x, y, z] != null)
                    {
                        Destroy(grid[x, y, z].gameObject);
                        grid[x, y, z] = null;
                    }
                }
            }
        }
    }

    public static void lowerLayer(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            for(int z = 0; z < d; ++z)
            {
                if (grid[x, y, z] != null)
                {
                    grid[x, y - 1, z] = grid[x, y, z];
                    grid[x, y, z] = null;
                    grid[x, y - 1, z].position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    public static void lowerLayersAbove(int y)
    {
        for (int i = y; i < h; ++i)
            lowerLayer(i);
    }

    public static bool isLayerFull(int y)
    {
        for (int x = 0; x < w; ++x)
        {
            for (int z = 0; z < d; ++z)
            {
                if (grid[x, y, z] == null)
                    return false;
            }
        }

        return true;
    }

    public static void deleteFullLayers()
    {
        for (int y = 0; y < h; ++y)
        {
            if (isLayerFull(y))
            {
                deleteLayer(y);
                lowerLayersAbove(y + 1);
                --y;
            }
        }
    }

}
