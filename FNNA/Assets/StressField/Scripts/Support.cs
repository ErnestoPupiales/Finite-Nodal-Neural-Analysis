using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is use to activate a boolean when the user set
/// the boundary condition on the border of the plate.
/// The game object is move from the TouchManager with the first finger touch.
/// </summary>
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

        //Code to implement when the ANNs predict stress applying loads on vertical direction

        /*else if (collision.tag == "UP")
        {
            fixImageUP.SetActive(true);
            VerticalFixSupport = true;

        }

        else if (collision.tag == "Down")
        {
            fixImageDown.SetActive(true);
            VerticalFixSupport = true;

        }*/

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

        //Code to implement when the ANNs predict stress applying loads on vertical direction

        /*else if (collision.tag == "UP")
        {
            fixImageUP.SetActive(false);
            VerticalFixSupport = false;

        }

        else if (collision.tag == "Down")
        {
            fixImageDown.SetActive(false);
            VerticalFixSupport = false;

        }*/
    }

}
