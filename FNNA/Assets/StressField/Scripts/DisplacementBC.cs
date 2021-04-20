using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is use to activate a boolean when the user initialize
/// displacement on the border of the plate.
/// The game object is move from the TouchManager with the first finger touch.
/// </summary>
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
