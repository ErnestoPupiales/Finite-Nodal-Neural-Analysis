using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Support : MonoBehaviour
{
    public bool lateralFixSupport;
    public bool VerticalFixSupport;
    public GameObject fixImageLH = null;
    public GameObject fixImageRH = null;
    public GameObject fixImageUP = null;
    public GameObject fixImageDown = null;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "LH")
        {
            lateralFixSupport = true;
            fixImageLH.SetActive(true);            
        }

        else if (collision.tag == "RH")
        {
            fixImageRH.SetActive(true);
            lateralFixSupport = true;

        }

        else if (collision.tag == "UP")
        {
            fixImageUP.SetActive(true);
            VerticalFixSupport = true;

        }

        else if (collision.tag == "Down")
        {
            fixImageDown.SetActive(true);
            VerticalFixSupport = true;

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "LH")
        {
            lateralFixSupport = false;
            fixImageLH.SetActive(false);

        }

        else if (collision.tag == "RH")
        {
            fixImageRH.SetActive(false);
            lateralFixSupport = false;

        }

        else if (collision.tag == "UP")
        {
            fixImageUP.SetActive(false);
            VerticalFixSupport = false;

        }

        else if (collision.tag == "Down")
        {
            fixImageDown.SetActive(false);
            VerticalFixSupport = false;

        }
    }

}
