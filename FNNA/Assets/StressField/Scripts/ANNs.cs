using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using TMPro;
using System.IO;

/// <summary>
/// This Class implement the TensorFlowLite Library with the ANNs
/// </summary>
public class ANNs : MonoBehaviour
{
    [SerializeField] string fileNameNodesCloud = "CloudNode.tflite";
    [SerializeField] string fileNameVonMisses = "VonMisses.tflite";
    [SerializeField] string fileNameTresca = "Tresca.tflite";

    Interpreter nodesCloud_NN;
    Interpreter VonMisses_NN; 
    Interpreter Tresca_NN;

    void Start()
    {
        var options = new InterpreterOptions()
        {
            threads = 2,
        };

        string pathNodesCloud = Path.Combine(Application.streamingAssetsPath, fileNameNodesCloud);
        string pathVonMisses = Path.Combine(Application.streamingAssetsPath, fileNameVonMisses);
        string pathTresca = Path.Combine(Application.streamingAssetsPath, fileNameTresca);

        nodesCloud_NN = new Interpreter(FileUtil.LoadFile(pathNodesCloud), options);
        nodesCloud_NN.AllocateTensors();
        
        VonMisses_NN = new Interpreter(FileUtil.LoadFile(pathVonMisses), options);
        VonMisses_NN.AllocateTensors();

        Tresca_NN = new Interpreter(FileUtil.LoadFile(pathTresca), options);
        Tresca_NN.AllocateTensors();

    }

    private void OnDestroy()    
    {
        nodesCloud_NN?.Dispose();
        VonMisses_NN?.Dispose();
        Tresca_NN.Dispose();
    }

    public float[] nodesCloud_DoInference(string a, string b)
    {

        float[] input = new float[2];
        input[0] = float.Parse(a);
        input[1] = float.Parse(b);

        float[] output = new float[4050];

        nodesCloud_NN.SetInputTensorData(0, input);
        nodesCloud_NN.Invoke();
        nodesCloud_NN.GetOutputTensorData(0, output);

        return (output);
    }

    public float[] VonMisses_DoInference(string a, string b, string c)
    {
        float[] input = new float[3];
        input[0] = float.Parse(a);
        input[1] = float.Parse(b);
        input[2] = float.Parse(c);

        float[] output = new float[2025];
        VonMisses_NN.SetInputTensorData(0, input);
        VonMisses_NN.Invoke();
        VonMisses_NN.GetOutputTensorData(0, output);
        return (output);
    }

    public float[] Tresca_DoInference(string a, string b, string c)
    {
        float[] input = new float[3];
        input[0] = float.Parse(a);
        input[1] = float.Parse(b);
        input[2] = float.Parse(c);

        float[] output = new float[2025];
        Tresca_NN.SetInputTensorData(0, input);
        Tresca_NN.Invoke();
        Tresca_NN.GetOutputTensorData(0, output);
        return (output);
    }

}
