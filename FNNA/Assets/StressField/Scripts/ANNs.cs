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

    public List<ListNN> Parameters = new List<ListNN>();
    public List<ArtificialNeuralNetwork> NN = new List<ArtificialNeuralNetwork>();

    public class ArtificialNeuralNetwork
    {

        public string fileNamePath;
        public int inputNeurons;
        public int outputNeurons;

        private float[] inputArray;
        private float[] outputArray;

        Interpreter InterpreterNN;

        public ArtificialNeuralNetwork(string FileNamePath, int InputNeurons, int OutputNeurons)
        {
            fileNamePath = FileNamePath;
            inputNeurons = InputNeurons;
            outputNeurons = OutputNeurons;

            inputArray = new float[inputNeurons];
            outputArray = new float[outputNeurons];

            var options = new InterpreterOptions()
            {
                threads = 2,
            };

            string path = Path.Combine(Application.streamingAssetsPath, fileNamePath);

            InterpreterNN = new Interpreter(FileUtil.LoadFile(path), options);
            InterpreterNN.AllocateTensors();
        }

        public float[] NN_DoInference(string a, string b, string c)
        {
            inputArray[0] = float.Parse(a);
            inputArray[1] = float.Parse(b);
            inputArray[2] = float.Parse(c);

            InterpreterNN.SetInputTensorData(0, inputArray);
            InterpreterNN.Invoke();
            InterpreterNN.GetOutputTensorData(0, outputArray);

            return (outputArray);
        }

        public float[] NN_DoInference(string a, string b)
        {
            System.GC.Collect();
            inputArray[0] = float.Parse(a);
            inputArray[1] = float.Parse(b);

            InterpreterNN.SetInputTensorData(0, inputArray);
            InterpreterNN.Invoke();
            InterpreterNN.GetOutputTensorData(0, outputArray);
            
            return (outputArray);
        }

        public void OnDestroyNN()
        {
            InterpreterNN?.Dispose();
        }
    }

    [System.Serializable]
    public class ListNN
    {
        public string name;
        public int input;
        public int output;
        public float maxScale;
        public float minScale;
    }

    private void Awake()
    {
        foreach (var item in Parameters)
        {
            ArtificialNeuralNetwork Prueba = new ArtificialNeuralNetwork(item.name+".tflite", item.input, item.output);
            NN.Add(Prueba);
        }
    }

    private void OnDestroy()
    {
        foreach(var item in NN)
        {
            item.OnDestroyNN();
        }
    }

}
