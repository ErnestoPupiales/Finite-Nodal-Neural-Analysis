using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// This class uses logic to activate or deactivate the choosen fields.
/// </summary>
public class TouchManager : MonoBehaviour
{
    private bool activeLateral = false;

    public GameObject support;
    public Support supportScript;

    public GameObject displacementBC;
    public DisplacementBC DisplacementBDScript;

    public GraphCode grapchode;

    public bool test;
    public bool rainsingedge;


    [SerializeField] private TMP_Text TimeCountClear11 = null;
    public double beginning1;

    void Update()
    {

        if (Input.touchCount > 0 & EventSystem.current.currentSelectedGameObject == null)
        {

            Touch Fijacion = Input.GetTouch(0);

            support.transform.position = Fijacion.position;

            if (supportScript.lateralFixSupport)
            {
                if(Input.touchCount == 2)
                {
                    Touch Desplazamiento = Input.GetTouch(1);

                    displacementBC.transform.position = Desplazamiento.position;

                    if(Desplazamiento.phase == TouchPhase.Moved && DisplacementBDScript.displacement)
                    {
                        activeLateral = true;
                    }

                    else if (Desplazamiento.phase == TouchPhase.Ended)
                    {

                        DisplacementBDScript.displacement = false;
                        activeLateral = false;
                        rainsingedge = false;
                    }
                }
                else
                {
                    displacementBC.transform.position = Vector3.zero;
                }
            }
            else if (Fijacion.phase == TouchPhase.Ended)
            {

                DisplacementBDScript.displacement = false;
                activeLateral = false;
                rainsingedge = false;
            }
        }
        else
        {
            support.transform.position = Vector3.zero;
            displacementBC.transform.position = Vector3.zero;
        }


        if ((test || activeLateral)&!rainsingedge)
        {
            rainsingedge = true;
            beginning1 = Time.realtimeSinceStartup;
            grapchode.ActivateField(true);
            TimeCountClear11.text = (Time.realtimeSinceStartup - beginning1).ToString("0.00000000");
        }
        else if(!rainsingedge)
        {
            grapchode.ActivateField(false);
        }
    }

}
