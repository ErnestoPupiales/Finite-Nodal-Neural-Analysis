using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphCode : MonoBehaviour
{
    [SerializeField] private Sprite node = null;

    [SerializeField] private Sprite stressnode8 = null;
    [SerializeField] private Sprite stressnode7 = null;
    [SerializeField] private Sprite stressnode6 = null;
    [SerializeField] private Sprite stressnode5 = null;
    [SerializeField] private Sprite stressnode4 = null;
    [SerializeField] private Sprite stressnode3 = null;
    [SerializeField] private Sprite stressnode2 = null;
    [SerializeField] private Sprite stressnode1 = null;

    [SerializeField] private TMP_Text TagNode8_Eqv = null;
    [SerializeField] private TMP_Text TagNode7_Eqv = null;
    [SerializeField] private TMP_Text TagNode6_Eqv = null;
    [SerializeField] private TMP_Text TagNode5_Eqv = null;
    [SerializeField] private TMP_Text TagNode4_Eqv = null;
    [SerializeField] private TMP_Text TagNode3_Eqv = null;
    [SerializeField] private TMP_Text TagNode2_Eqv = null;
    [SerializeField] private TMP_Text TagNode1_Eqv = null;
    [SerializeField] private TMP_Text TagNodeMin_Eqv = null;

    [SerializeField] private TMP_Text TagNode8_Int = null;
    [SerializeField] private TMP_Text TagNode7_Int = null;
    [SerializeField] private TMP_Text TagNode6_Int = null;
    [SerializeField] private TMP_Text TagNode5_Int = null;
    [SerializeField] private TMP_Text TagNode4_Int = null;
    [SerializeField] private TMP_Text TagNode3_Int = null;
    [SerializeField] private TMP_Text TagNode2_Int = null;
    [SerializeField] private TMP_Text TagNode1_Int = null;
    [SerializeField] private TMP_Text TagNodeMin_Int = null;

    

    [SerializeField] TMP_InputField SMayor = null;
    [SerializeField] TMP_InputField SMenor = null;
    [SerializeField] TMP_InputField Displacement = null;

    public ANNs ANN;

    private float stressMin = 1e30f;
    private float stressMax = 0;

    private float[] nodesCloudArray;
    private float[] vonMissesArray;
    private float[] trescaArray;

    public GameObject NodeCloudContainer;
    private RectTransform nodeCloudContainer;
    public GameObject SymmetryNodeCloud;
    private RectTransform symmetryNodeCloud;

    public GameObject VonMissesContainer;
    private RectTransform vonMissesContainer;
    public GameObject SymmetryVonMisses;
    private RectTransform symmetryVonMisses;
    

    public GameObject TrescaContainer;
    private RectTransform trescaContainer;
    public GameObject SymmetryTresca;
    private RectTransform symmetryTresca;
    
    public RectTransform workplane;

    float Scale = 1e9f;

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

    public double beginning;

    private void Awake()
    {
        workplane.sizeDelta = new Vector2(Screen.width * 0.52f, Screen.height * 0.98f);

        nodeCloudContainer = NodeCloudContainer.GetComponent<RectTransform>();
        symmetryNodeCloud = SymmetryNodeCloud.GetComponent<RectTransform>();

        vonMissesContainer = VonMissesContainer.GetComponent<RectTransform>();
        symmetryVonMisses = SymmetryVonMisses.GetComponent<RectTransform>();

        trescaContainer = TrescaContainer.GetComponent<RectTransform>();
        symmetryTresca = SymmetryTresca.GetComponent<RectTransform>();
    }


    public void Calculate()
    {
        
        beginning = Time.realtimeSinceStartup;
        ClearAll();
        TimeCountClear1.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        nodesCloudArray = ANN.nodesCloud_DoInference(SMayor.text, SMenor.text);
        TimeCountClear2.text = (Time.realtimeSinceStartup - beginning).ToString("0.0000000");
        NodesCloudDisplay(nodesCloudArray);
        TimeCountClear3.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        Symmetry(nodeCloudContainer, symmetryNodeCloud);
        TimeCountClear4.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        vonMissesArray = ANN.VonMisses_DoInference(SMayor.text, SMenor.text, Displacement.text);
        TimeCountClear5.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        StressDisplay(nodesCloudArray, vonMissesArray, vonMissesContainer, 0);
        TimeCountClear6.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        Symmetry(vonMissesContainer, symmetryVonMisses);
        TimeCountClear7.text = (Time.realtimeSinceStartup - beginning).ToString("0.0000000");
        trescaArray = ANN.Tresca_DoInference(SMayor.text, SMenor.text, Displacement.text);
        TimeCountClear8.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        StressDisplay(nodesCloudArray, trescaArray, trescaContainer, 1);
        TimeCountClear9.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        Symmetry(trescaContainer, symmetryTresca);
        TimeCountClear10.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        

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
            CreateNode(new Vector2(Nodes[j] * 256 / 0.64f, Nodes[k] * 256 / 0.64f), nodeCloudContainer, node);
            j = j + 2;
            k = k + 2;
        }
    }

    public void MaxMin(float[] Stress)
    {
        stressMin = 1e30f;
        stressMax = 0;

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

    public void StressDisplay(float[] coordinates, float[] stress, RectTransform container, int tag)
    {
        
        Sprite StressNode;
        MaxMin(stress);

        int j = 0;
        int k = 1;

        float delta = (stressMax - stressMin) / 8;

        if (tag ==0)
        {
            TagNodeMin_Eqv.text = (stressMin * Scale).ToString(".000e+00");
            TagNode1_Eqv.text = ((stressMin + delta * 1) * Scale).ToString(".000e+00");
            TagNode2_Eqv.text = ((stressMin + delta * 2) * Scale).ToString(".000e+00");
            TagNode3_Eqv.text = ((stressMin + delta * 3) * Scale).ToString(".000e+00");
            TagNode4_Eqv.text = ((stressMin + delta * 4) * Scale).ToString(".000e+00");
            TagNode5_Eqv.text = ((stressMin + delta * 5) * Scale).ToString(".000e+00");
            TagNode6_Eqv.text = ((stressMin + delta * 6) * Scale).ToString(".000e+00");
            TagNode7_Eqv.text = ((stressMin + delta * 7) * Scale).ToString(".000e+00");
            TagNode8_Eqv.text = ((stressMin + delta * 8) * Scale).ToString(".000e+00");
        }
        else
        {
            TagNodeMin_Int.text = (stressMin * Scale).ToString(".000e+00");
            TagNode1_Int.text = ((stressMin + delta * 1) * Scale).ToString(".000e+00");
            TagNode2_Int.text = ((stressMin + delta * 2) * Scale).ToString(".000e+00");
            TagNode3_Int.text = ((stressMin + delta * 3) * Scale).ToString(".000e+00");
            TagNode4_Int.text = ((stressMin + delta * 4) * Scale).ToString(".000e+00");
            TagNode5_Int.text = ((stressMin + delta * 5) * Scale).ToString(".000e+00");
            TagNode6_Int.text = ((stressMin + delta * 6) * Scale).ToString(".000e+00");
            TagNode7_Int.text = ((stressMin + delta * 7) * Scale).ToString(".000e+00");
            TagNode8_Int.text = ((stressMin + delta * 8) * Scale).ToString(".000e+00");
        }


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

    public void Symmetry(RectTransform quarterModel, RectTransform Container)
    {
        RectTransform Mirror1 = Instantiate(quarterModel, Container, false);
        Mirror1.anchoredPosition = new Vector3(128, -128, 0);
        Mirror1.anchorMin = new Vector2(0, 1);
        Mirror1.anchorMax = new Vector2(0, 1);
        Mirror1.transform.Rotate(0, 180, 0, Space.Self);

        RectTransform Mirror2 = Instantiate(quarterModel, Container, false);
        Mirror2.anchoredPosition = new Vector3(-128, 128, 0);
        Mirror2.anchorMin = new Vector2(1, 0);
        Mirror2.anchorMax = new Vector2(1, 0);
        Mirror2.transform.Rotate(180, 0, 0, Space.Self);

        RectTransform Mirror3 = Instantiate(quarterModel, Container, false);
        Mirror3.anchoredPosition = new Vector3(128, 128, 0);
        Mirror3.anchorMin = new Vector2(0, 0);
        Mirror3.anchorMax = new Vector2(0, 0);
        Mirror3.transform.Rotate(0, 0, 180, Space.Self);

    }

    public void ClearAll()
    {
        foreach (Transform children in nodeCloudContainer)
        {
            Destroy(children.gameObject);
        }

        foreach (Transform children in symmetryNodeCloud)
        {
            Destroy(children.gameObject);
        }

        foreach (Transform children in vonMissesContainer)
        {
            Destroy(children.gameObject);
        }

        foreach (Transform children in symmetryVonMisses)
        {
            Destroy(children.gameObject);
        }

        foreach (Transform children in trescaContainer)
        {
            Destroy(children.gameObject);
        }

        foreach (Transform children in symmetryTresca)
        {
            Destroy(children.gameObject);
        }

    }

    public void CloseApp()
    {
        Application.Quit();
        Debug.Log("Exit");
    }


}
