using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMaterial : MonoBehaviour
{
    Renderer r;

    private void Start()
    {
        r = this.GetComponent<Renderer>();
    }

    public void changeColor()
    {
        Color c = new Color(0.65f, 0.65f, 0.70f);
        r.material.SetColor("Color_96A321DA", c);
    }


}
