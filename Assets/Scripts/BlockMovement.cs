using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockMovement : MonoBehaviour
{
    float lastFall = 0;

    void Start()
    {
        // Block is above upper bound
        if (!isValidGridPos())
        {
            GameManager.manager.gameOver();
            enabled = false;
        }
    }

    bool isValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector3 v = Playfield.roundVec3(child.position);

            // Not inside Border
            if (!Playfield.insideBorder(v))
                return false;

            // another block already in grid cell
            if (Playfield.grid[(int)v.x, (int)v.y, (int)v.z] != null &&
                Playfield.grid[(int)v.x, (int)v.y, (int)v.z].parent != transform)
            {
                return false;
            }
        }
        return true;
    }

    bool isValidGridBlock(Vector3 v)
    {
        if(!Playfield.insideBorder(v))
        {
            return false;
        }
        if (Playfield.grid[(int)v.x, (int)v.y, (int)v.z] != null &&
            Playfield.grid[(int)v.x, (int)v.y, (int)v.z].parent != transform)
        {
            return false;
        }
        return true;
    }

    void updateGrid()
    {
        // Remove old children from grid
        for (int y = 0; y < Playfield.h; ++y)
            for (int x = 0; x < Playfield.w; ++x)
                for (int z = 0; z < Playfield.d; ++z)
                    if (Playfield.grid[x, y, z] != null && Playfield.grid[x, y, z].parent == transform)
                            Playfield.grid[x, y, z] = null;

        // Add new children to grid
        foreach (Transform child in transform)
        {
            Vector3 v = Playfield.roundVec3(child.position);
            Playfield.grid[(int)v.x, (int)v.y, (int)v.z] = child;
        }
    }

    bool tryToMove(Vector3 v)
    {
        transform.position += v;
        if (isValidGridPos())
        {
            updateGrid();
            return true;
        }

        transform.position -= v;
        return false;
    }

    void hardDrop()
    {
        for(int i = 14; i >= 0; i--)
        {
            Vector3 v = new Vector3(0, -i, 0);
            if (tryToMove(v))
            {
                break;
            }
        }
    }

    void touchDown()
    {
        Playfield.deleteFullLayers();
        GameManager.manager.spawnNext();
        enabled = false;
        foreach(Transform child in transform)
        {
            child.GetComponent<BlockMaterial>().changeColor();
        }

    }

    void rotate(Vector3 vRotate)
    {
        //int minX, minZ, maxX, maxZ;
        //minX = minZ = int.MaxValue;
        //maxX = maxZ = int.MinValue;
        //foreach (Transform child in transform)
        //{
        //    Vector3 v = Playfield.roundVec3(child.position);
        //    minX = Math.Min(minX, (int) v.x);
        //    minZ = Math.Min(minZ, (int)v.z);
        //    maxX = Math.Max(maxX, (int)v.x);
        //    maxZ = Math.Max(maxZ, (int)v.z);
        //}

        transform.Rotate(vRotate, relativeTo: Space.World);
        if(!isValidGridPos())
        {
            transform.Rotate(-vRotate, relativeTo: Space.World);
        }
        else
        {
            updateGrid();
        }
        //int newMinX, newMinZ, newMaxX, newMaxZ;
        //newMinX = newMinZ = int.MaxValue;
        //newMaxX = newMaxZ = int.MinValue;
        //foreach (Transform child in transform)
        //{
        //    Vector3 v = Playfield.roundVec3(child.position);
        //    newMinX = Math.Min(newMinX, (int)v.x);
        //    newMinZ = Math.Min(newMinZ, (int)v.z);
        //    newMaxX = Math.Max(newMaxX, (int)v.x);
        //    newMaxZ = Math.Max(newMaxZ, (int)v.z);
        //}

        //Debug.Log("x: (" + minX + ", " + maxX + ')'
        //    + "z: (" + minZ + ", " + maxZ + ")");

        //Debug.Log("new x: (" + newMinX + ", " + newMaxX + ')'
        //    + "new z: (" + newMinZ + ", " + newMaxZ + ")");


        // updateGrid();
    }

    void parseKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tryToMove(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tryToMove(new Vector3(1, 0, 0));

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tryToMove(new Vector3(0, 0, -1));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tryToMove(new Vector3(0, 0, 1));

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rotate(new Vector3(-90, 0, 0));

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            rotate(new Vector3(0, 0, -90));

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            rotate(new Vector3(0, -90, 0));

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            hardDrop();
            touchDown();

        }
        else if (Input.GetKeyDown(KeyCode.S) || Time.time - lastFall >= 2)
        {
            if (!tryToMove(new Vector3(0, -1, 0)))
            {
                touchDown();
            }

            lastFall = Time.time;
        }
    }

    void Update()
    {
        if(GameManager.manager.isGameStarted && !GameManager.manager.isGamePaused)
        {
            parseKeyInput();
        }
    }

}
