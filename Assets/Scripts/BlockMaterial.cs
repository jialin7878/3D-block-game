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

    public void touchDown()
    {
        Color c = new Color(0.5f, 0.5f, 0.6f);
        r.material.SetColor("Color_96A321DA", c);
        r.material.SetFloat("Vector1_45E4353E", 0.2f);
        r.material.SetFloat("Vector1_714C8909", 0.95f);
    }

    public void selfDestruct()
    {
        Debug.Log("called self destruct");
        Color c = new Color(1f, 1f, 1f);
        r.material.SetColor("Color_96A321DA", c);
        r.material.SetFloat("Vector1_45E4353E", 2f);
    }


}
