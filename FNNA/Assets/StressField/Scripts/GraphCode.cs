using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This Class creates the stress fields by consulting the ANNs.
/// </summary>
public class GraphCode : MonoBehaviour
{
    public RectTransform workplane;

    public ANNs aNN;

    public RectTransform FieldContainer;
    public List<GameObject> FieldList;
    public List<RectTransform> fieldRectTransformList;

    [SerializeField] TMP_InputField SMayor = null;
    [SerializeField] TMP_InputField SMenor = null;
    [SerializeField] TMP_InputField Displacement = null;

    private float[] nodesCloudArray;
    private float[] stressArray;

    private float stressMin;
    private float stressMax;

    [SerializeField] private Sprite node = null;
    [SerializeField] private Sprite stressnode8 = null;
    [SerializeField] private Sprite stressnode7 = null;
    [SerializeField] private Sprite stressnode6 = null;
    [SerializeField] private Sprite stressnode5 = null;
    [SerializeField] private Sprite stressnode4 = null;
    [SerializeField] private Sprite stressnode3 = null;
    [SerializeField] private Sprite stressnode2 = null;
    [SerializeField] private Sprite stressnode1 = null;

    float Scale = 1e9f;

    [SerializeField] private TMP_Text TagNode8_Int = null;
    [SerializeField] private TMP_Text TagNode7_Int = null;
    [SerializeField] private TMP_Text TagNode6_Int = null;
    [SerializeField] private TMP_Text TagNode5_Int = null;
    [SerializeField] private TMP_Text TagNode4_Int = null;
    [SerializeField] private TMP_Text TagNode3_Int = null;
    [SerializeField] private TMP_Text TagNode2_Int = null;
    [SerializeField] private TMP_Text TagNode1_Int = null;
    [SerializeField] private TMP_Text TagNodeMin_Int = null;




    //Just for evaluate the procesing time
    [SerializeField] private TMP_Text TimeCountClear1 = null;
    [SerializeField] private TMP_Text TimeCountClear2 = null;
    [SerializeField] private TMP_Text TimeCountClear3 = null;
    [SerializeField] private TMP_Text TimeCountClear4 = null;
    [SerializeField] private TMP_Text TimeCountClear5 = null;
    [SerializeField] private TMP_Text TimeCountClear6 = null;
    [SerializeField] private TMP_Text TimeCountClear7 = null;
    [SerializeField] private TMP_Text TimeCountClear8 = null;
    [SerializeField] private TMP_Text TimeCountClear9 = null;
    [SerializeField] private TMP_Text TimeCountClear10 = null;

    

    //public double beginning;

    private void Awake()
    {
        nodesCloudArray = new float[4050];
        stressArray = new float[2025];

        workplane.sizeDelta = new Vector2(Screen.width * 0.52f, Screen.height * 0.98f);

        FieldList = new List<GameObject>();
        fieldRectTransformList = new List<RectTransform>();

        foreach (var item in aNN.Parameters)
        {
            GameObject field = new GameObject(item.name, typeof(RectTransform));
            field.transform.SetParent(FieldContainer, false);
            RectTransform fieldRectTransform = field.GetComponent<RectTransform>();
            fieldRectTransform.sizeDelta = new Vector2(256, 256);
            fieldRectTransform.anchorMin = new Vector2(1, 1);
            fieldRectTransform.anchorMax = new Vector2(1, 1);
            fieldRectTransform.pivot = new Vector2(1, 1);

            FieldList.Add(field);
            fieldRectTransformList.Add(fieldRectTransform);
        }
    }


    public void Calculate()
    {

        /*beginning = Time.realtimeSinceStartup; 
        TimeCountClear1.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear2.text = (Time.realtimeSinceStartup - beginning).ToString("0.0000000");

        TimeCountClear3.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear4.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear5.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear6.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear7.text = (Time.realtimeSinceStartup - beginning).ToString("0.0000000");

        TimeCountClear8.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear9.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");

        TimeCountClear10.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");*/

        ClearAll();

        nodesCloudArray = aNN.NN[0].NN_DoInference(SMayor.text, SMenor.text);
        NodesCloudDisplay(nodesCloudArray);
        Symmetry(fieldRectTransformList[0]);

        for (int i = 1; i < FieldList.Count; i++)
        {
            stressArray = aNN.NN[i].NN_DoInference(SMayor.text, SMenor.text, Displacement.text);
            StressDisplay(nodesCloudArray, stressArray, fieldRectTransformList[i]);
            Symmetry(fieldRectTransformList[i]);
        }
    }

    private void CreateNode(Vector2 anchoredPosition, RectTransform container, Sprite node)
    {
        GameObject gameObject = new GameObject("node", typeof(Image));
        gameObject.transform.SetParent(container, false);
        gameObject.GetComponent<Image>().sprite = node;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(15, 15);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    public void NodesCloudDisplay(float[] Nodes)
    {
        int j = 0;
        int k = 1;

        for (int i = 0; i < 2025; i++)
        {
            CreateNode(new Vector2(Nodes[j] * 256 / 0.64f, Nodes[k] * 256 / 0.64f), fieldRectTransformList[0], node);
            j = j + 2;
            k = k + 2;
        }
    }

    public void MaxMin(float[] Stress)
    {
        stressMin =  1e30f;
        stressMax = -1e30f;

        for (int i = 0; i < 2025; i++)
        {
            if (Stress[i] > stressMax)
            {
                stressMax = Stress[i];
            }
            else if (Stress[i] < stressMin)
            {
                stressMin = Stress[i];
            }
        }
    }

    public void StressDisplay(float[] coordinates, float[] stress, RectTransform container)
    {
        
        Sprite StressNode;
        MaxMin(stress);

        int j = 0;
        int k = 1;

        float delta = (stressMax - stressMin) / 8;
        
        TagNodeMin_Int.text = (stressMin * Scale).ToString(".000e+00");
        TagNode1_Int.text = ((stressMin + delta * 1) * Scale).ToString(".000e+00");
        TagNode2_Int.text = ((stressMin + delta * 2) * Scale).ToString(".000e+00");
        TagNode3_Int.text = ((stressMin + delta * 3) * Scale).ToString(".000e+00");
        TagNode4_Int.text = ((stressMin + delta * 4) * Scale).ToString(".000e+00");
        TagNode5_Int.text = ((stressMin + delta * 5) * Scale).ToString(".000e+00");
        TagNode6_Int.text = ((stressMin + delta * 6) * Scale).ToString(".000e+00");
        TagNode7_Int.text = ((stressMin + delta * 7) * Scale).ToString(".000e+00");
        TagNode8_Int.text = ((stressMin + delta * 8) * Scale).ToString(".000e+00");
        
        for (int i = 0; i < 2025; i++)
        {
            if (stressMin <= stress[i] && stress[i] < stressMin + delta * 1) StressNode = stressnode1;
            else if (stressMin + delta * 1 <= stress[i] && stress[i] < stressMin + delta * 2) StressNode = stressnode2;
            else if (stressMin + delta * 2 <= stress[i] && stress[i] < stressMin + delta * 3) StressNode = stressnode3;
            else if (stressMin + delta * 3 <= stress[i] && stress[i] < stressMin + delta * 4) StressNode = stressnode4;
            else if (stressMin + delta * 4 <= stress[i] && stress[i] < stressMin + delta * 5) StressNode = stressnode5;
            else if (stressMin + delta * 5 <= stress[i] && stress[i] < stressMin + delta * 6) StressNode = stressnode6;
            else if (stressMin + delta * 6 <= stress[i] && stress[i] < stressMin + delta * 7) StressNode = stressnode7;
            else if (stressMin + delta * 7 <= stress[i] && stress[i] < stressMin + delta * 8) StressNode = stressnode8;
            else StressNode = node;

            CreateNode(new Vector2(coordinates[j] * 256 / 0.64f, coordinates[k] * 256 / 0.64f), container, StressNode);
            j = j + 2;
            k = k + 2;
        }
    }

    public void Symmetry(RectTransform quarterModel)
    {
        RectTransform Mirror1 = Instantiate(quarterModel, quarterModel, false);
        Mirror1.pivot = new Vector2(0, 0);
        Mirror1.anchoredPosition = new Vector2(-256,-256);
        Mirror1.localScale = new Vector3(-1, 1, 1);

        RectTransform Mirror2 = Instantiate(quarterModel, quarterModel, false);
        Mirror2.pivot = new Vector2(0, 0);
        Mirror2.anchoredPosition = new Vector2(-256, -256);
        Mirror2.localScale = new Vector3(1, -1, 1);

    }

    public void ClearAll()
    {
        foreach(Transform children in fieldRectTransformList)
        {
            foreach (Transform subchildren in children)
            {
                Destroy(subchildren.gameObject);

                foreach(Transform subchildrenint in subchildren)
                {
                    Destroy(subchildrenint.gameObject);
                }
            }
        }


    }

    public void CloseApp()
    {
        Application.Quit();
    }


}
