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

    bool tryToMove(Vector3 v, bool isRotation)
    {
        if (isRotation)
            transform.Rotate(v, Space.World);
        else
            transform.position += v;

        if (isValidGridPos())
        {
            updateGrid();
            return true;
        }

        if (isRotation)
            transform.Rotate(-v, Space.World);
        else
            transform.position -= v;
        return false;
    }

    void hardDrop()
    {
        for(int i = 14; i >= 0; i--)
        {
            Vector3 v = new Vector3(0, -i, 0);
            if (tryToMove(v, false))
            {
                break;
            }
        }
    }

    void parseKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            tryToMove(new Vector3(-1, 0, 0), false);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            tryToMove(new Vector3(1, 0, 0), false);

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tryToMove(new Vector3(0, 0, -1), false);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tryToMove(new Vector3(0, 0, 1), false);

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tryToMove(new Vector3(-90, 0, 0), true);

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            tryToMove(new Vector3(0, 0, -90), true);

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            tryToMove(new Vector3(0, -90, 0), true);

        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            hardDrop();
            Playfield.deleteFullLayers();
            GameManager.manager.spawnNext();
            enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Time.time - lastFall >= 2)
        {
            if (!tryToMove(new Vector3(0, -1, 0), false))
            {
                Playfield.deleteFullLayers();
                GameManager.manager.spawnNext();
                enabled = false;
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
