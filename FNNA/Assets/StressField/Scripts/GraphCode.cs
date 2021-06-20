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

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public Sprite prefab;
        public int size;
    }

    public List<Pool> pools;


    public RectTransform workplane;

    public ANNs aNN;

    public RectTransform FieldContainer;
    private List<GameObject> FieldList;
    private List<RectTransform> fieldRectTransformList;


    private List<Text> LabelsLists;
    public Transform LabelContainer;
    private List<GameObject> LabelObject;
    public Font LabelFont;

    [SerializeField] TMP_InputField SMayor = null;
    [SerializeField] TMP_InputField SMenor = null;
    [SerializeField] TMP_InputField Displacement = null;

    private float[] nodesCloudArray;
    private float[] stressArray;

    private float stressMin;
    private float stressMax;

    [SerializeField] private Sprite node = null;

    float Scale = 1e9f;

    //Just for evaluate the procesing time
    [SerializeField] private TMP_Text TimeCountClear1 = null;
    [SerializeField] private TMP_Text TimeCountClear2 = null;
    [SerializeField] private TMP_Text TimeCountClear3 = null;
    [SerializeField] private TMP_Text TimeCountClear4 = null;
    [SerializeField] private TMP_Text TimeCountClear5 = null;
    [SerializeField] private TMP_Text TimeCountClear6 = null;
  

    public double beginning;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    

    private void Awake()
    {
        nodesCloudArray = new float[4050];
        stressArray = new float[2025];

        workplane.sizeDelta = new Vector2(Screen.width * 0.52f, Screen.height * 0.98f);

        FieldList = new List<GameObject>();
        fieldRectTransformList = new List<RectTransform>();

        LabelsLists = new List<Text>();
        LabelObject = new List<GameObject>();

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

            GameObject label = new GameObject(item.name + "label", typeof(Text));
            label.transform.SetParent(LabelContainer, false);
            label.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 580);
            Text labeltext = label.GetComponent<Text>();
            labeltext.fontSize = 32;
            labeltext.font = LabelFont;

            LabelsLists.Add(labeltext);
            LabelObject.Add(label);
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        Queue<GameObject> objectpoolnode0 = new Queue<GameObject>();
        for (int i = 0; i < 2025 * 4; i++)
        {
            GameObject obj = new GameObject("node0", typeof(Image));
            obj.GetComponent<Image>().sprite = node;
            obj.SetActive(false);
            objectpoolnode0.Enqueue(obj);
        }

        poolDictionary.Add("node0", objectpoolnode0);

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectpool = new Queue<GameObject>();

            for (int i = 0; i < pool.size * 4; i++)
            {
                GameObject obj = new GameObject(pool.tag, typeof(Image));
                obj.GetComponent<Image>().sprite = pool.prefab;
                obj.SetActive(false);
                objectpool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectpool);
        }
    }


public void Calculate()
    {
        beginning = Time.realtimeSinceStartup;
        ClearAll();
        TimeCountClear1.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        nodesCloudArray = aNN.NN[0].NN_DoInference(SMayor.text, SMenor.text);
        TimeCountClear2.text = (Time.realtimeSinceStartup - beginning).ToString("0.0000000");
        NodesCloudDisplay(nodesCloudArray);
        TimeCountClear3.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
       
        for (int i = 1; i < FieldList.Count; i++)
        {
            
            stressArray = aNN.NN[i].NN_DoInference(SMayor.text, SMenor.text, Displacement.text);
            TimeCountClear4.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
            StressDisplay(nodesCloudArray, stressArray, fieldRectTransformList[i],i);
            TimeCountClear5.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        }
        TimeCountClear6.text = (Time.realtimeSinceStartup - beginning).ToString("0.00000000");
        
    }

    private void CreateNode(Vector2 anchoredPosition, RectTransform container, string tag /*Sprite node*/)
    {
        GameObject gameObject = poolDictionary[tag].Dequeue();
        gameObject.transform.SetParent(container, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(15, 15);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        gameObject.SetActive(true);
        poolDictionary[tag].Enqueue(gameObject);

    }

    public void NodesCloudDisplay(float[] Nodes)
    {
        int j = 0;
        int k = 1;

        for (int i = 0; i < 2025; i++)
        {
            CreateNode(new Vector2(Nodes[j] * 256 / 0.64f, Nodes[k] * 256 / 0.64f), fieldRectTransformList[0], "node0");
            CreateNode(new Vector2(Nodes[j] * -256 / 0.64f, Nodes[k] * 256 / 0.64f), fieldRectTransformList[0], "node0");
            CreateNode(new Vector2(Nodes[j] * 256 / 0.64f, Nodes[k] * -256 / 0.64f), fieldRectTransformList[0], "node0");
            CreateNode(new Vector2(Nodes[j] * -256 / 0.64f, Nodes[k] * -256 / 0.64f), fieldRectTransformList[0], "node0");
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

    public void StressDisplay(float[] coordinates, float[] stress, RectTransform container, int labelindex)
    {    
        string stressnode;
        MaxMin(stress);

        int j = 0;
        int k = 1;

        float delta = (stressMax - stressMin) / 8;

        LabelsLists[labelindex].text =      (stressMin * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 1) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 2) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 3) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 4) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 5) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 6) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 7) * Scale).ToString(".000e+00") + "\n\n" +
                                            ((stressMin + delta * 8) * Scale).ToString(".000e+00");
        
        for (int i = 0; i < 2025; i++)
        {

            if (stressMin <= stress[i] && stress[i] < stressMin + delta * 1) stressnode = "stressnode1";
            else if (stressMin + delta * 1 <= stress[i] && stress[i] < stressMin + delta * 2) stressnode = "stressnode2";
            else if (stressMin + delta * 2 <= stress[i] && stress[i] < stressMin + delta * 3) stressnode = "stressnode3";
            else if (stressMin + delta * 3 <= stress[i] && stress[i] < stressMin + delta * 4) stressnode = "stressnode4";
            else if (stressMin + delta * 4 <= stress[i] && stress[i] < stressMin + delta * 5) stressnode = "stressnode5";
            else if (stressMin + delta * 5 <= stress[i] && stress[i] < stressMin + delta * 6) stressnode = "stressnode6";
            else if (stressMin + delta * 6 <= stress[i] && stress[i] < stressMin + delta * 7) stressnode = "stressnode7";
            else if (stressMin + delta * 7 <= stress[i] && stress[i] < stressMin + delta * 8) stressnode = "stressnode8";
            else stressnode = "stressnode8";

            CreateNode(new Vector2(coordinates[j] * 256 / 0.64f, coordinates[k] * 256 / 0.64f), container, stressnode);
            CreateNode(new Vector2(coordinates[j] * -256 / 0.64f, coordinates[k] * 256 / 0.64f), container, stressnode);
            CreateNode(new Vector2(coordinates[j] * 256 / 0.64f, coordinates[k] * -256 / 0.64f), container, stressnode);
            CreateNode(new Vector2(coordinates[j] * -256 / 0.64f, coordinates[k] * -256 / 0.64f), container, stressnode);
            j = j + 2;
            k = k + 2;
        }
    }

    public void ClearAll()
    {
        foreach(Transform children in fieldRectTransformList)
        {
            foreach (Transform subchildren in children)
            {
                subchildren.gameObject.SetActive(false);

                foreach (Transform subchildrenint in subchildren)
                {
                    subchildren.gameObject.SetActive(false);
                }
            }
        }

    }

    public void CloseApp()
    {
        Application.Quit();
    }


}
