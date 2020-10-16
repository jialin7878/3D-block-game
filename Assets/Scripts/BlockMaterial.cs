using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterial : MonoBehaviour
{
    public Texture3D image;
    public int borderWidth = 30;
 
    void Start()
    {


        Color borderColor = new Color(1, 0, 0, 1);

        for (int x = 0; x < image.width; x++) {
            for (int y = 0; y < image.height; y++) {
                for(int z = 0; z < image.depth; z++)
                {
                    if (x < borderWidth || x > image.width - 1 - borderWidth)
                    {
                        image.SetPixel(x, y, z, borderColor);
                    }
                    if (y < borderWidth || y > image.width - 1 - borderWidth)
                    {
                        image.SetPixel(x, y, z, borderColor);
                    }
                    if (z < borderWidth || z > image.width - 1 - borderWidth)
                    {
                        image.SetPixel(x, y, z, borderColor);
                    }
                }
            }
        }

        image.Apply();
    }
}
