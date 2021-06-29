using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public ANNs ANNSet;
    public Toggle TogglePrefab;
    public RectTransform obj;
    Text label;

    List<Toggle> ToggleList = new List<Toggle>();
    public List<bool> fieldsetting = new List<bool>();




    private float fields;

    private void Awake()
    {
        fields = ANNSet.Parameters.Count - 2;
        obj.sizeDelta = new Vector2(15, fields*70);

        for(int i=1; i <= fields; i++)
        {

            Toggle field = Instantiate(TogglePrefab);
            field.transform.SetParent(obj, false);
            field.isOn = true;
            RectTransform fieldRectTransform = field.GetComponent<RectTransform>();
            fieldRectTransform.anchoredPosition = new Vector2(0, -70*i+70);
            label = field.GetComponentInChildren<Text>();
            label.text = ANNSet.Parameters[i + 1].name;
            ToggleList.Add(field);
            fieldsetting.Add(true);


        }
    }

    public void SetSetting()
    {
        for(int i=0; i<ToggleList.Count; i++)
        {
            fieldsetting[i] = ToggleList[i].isOn;
        }
    }
}
