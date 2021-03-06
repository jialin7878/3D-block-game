﻿using System;
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

    private void OnEnable()
    {
        GameManager.manager.OnHoldBlock += selfDestruct;
    }

    private void OnDisable()
    {
        GameManager.manager.OnHoldBlock -= selfDestruct;
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

    void selfDestruct()
    {
        foreach (Transform child in transform)
        {
            Vector3 v = Playfield.roundVec3(child.position);
            if (Playfield.grid[(int)v.x, (int)v.y, (int)v.z] != null)
            {
                Destroy(Playfield.grid[(int)v.x, (int)v.y, (int)v.z].gameObject);
                Playfield.grid[(int)v.x, (int)v.y, (int)v.z] = null;
            }
        }
        enabled = false;
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
            Playfield.topLevel = Math.Max(Playfield.topLevel, (int) v.y + 1);
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
            child.GetComponent<BlockMaterial>().touchDown();
        }

    }

    void rotate(Vector3 vRotate)
    {
        transform.Rotate(vRotate, relativeTo: Space.World);
        if(!isValidGridPos())
        {
            transform.Rotate(-vRotate, relativeTo: Space.World);
        }
        else
        {
            updateGrid();
        }
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
