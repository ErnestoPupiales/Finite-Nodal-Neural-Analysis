using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TouchManager : MonoBehaviour
{
    private bool activeLateral = false;

    public GameObject support;
    public Support supportScript;

    public GameObject displacementBC;
    public DisplacementBC DisplacementBDScript;


    [SerializeField] TMP_Dropdown stressChoosen = null;
    public GameObject vonMissesDisplayed;
    public GameObject trescaDisplayed;
    public GameObject stressBar_Eqv;
    public GameObject stressBar_Int;

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
            }
        }
        else
        {
            support.transform.position = Vector3.zero;
            displacementBC.transform.position = Vector3.zero;
        }


        if (activeLateral)
        {
            beginning1 = Time.realtimeSinceStartup;
            ActivateStress(true);
            TimeCountClear11.text = (Time.realtimeSinceStartup - beginning1).ToString("0.00000000");
        }
        else
        {
            ActivateStress(false);
        }
    }

    public void ActivateStress(bool state)
    {
        if (stressChoosen.value == 0)
        {
            vonMissesDisplayed.SetActive(state);
            stressBar_Eqv.SetActive(state);
            trescaDisplayed.SetActive(false);
            stressBar_Int.SetActive(false);
        }

        else
        {
            trescaDisplayed.SetActive(state);
            stressBar_Int.SetActive(state);
            vonMissesDisplayed.SetActive(false);
            stressBar_Eqv.SetActive(false);
        }
    }

}
