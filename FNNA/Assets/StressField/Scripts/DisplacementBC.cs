using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplacementBC : MonoBehaviour
{
    public bool displacement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LH" || collision.tag == "RH")
        {
            displacement = true;
        }

    }





}
