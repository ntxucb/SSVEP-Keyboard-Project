using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Linq;
using System;




public class OnnxInference : MonoBehaviour
{
    Tensor<float> t1, t2;
    //float[] sourceData;  // assume your data is loaded into a flat float array
    //int[] dimensions;    // and the dimensions of the input is stored here
    // Start is called before the first frame update
    static float width = 8.0f;
    static float height = 8.0f;

    float[] sourceData = { width, height };
    int[] dimensions = { 1, 2 };


    [System.Serializable]
    public struct Prediction
    {
        public List<string> predicted;
        public void SetPrediction(Tensor<string> predict){
           // predicted=t.AsFloats();
          //  predictedValue = System.Array.IndexOf(predicted,predicted.Max());
            foreach(var t in predict){
                print("pred "+t);
                predicted.Add(t);
            }
        }
    }



    public Prediction predicti;



    private float[,] matriz;
    //var denseTensor = new DenseTensor<int>(new[] { 3, 5 });
    async void Start()
    {
       // t1= new DenseTensor<float>(new[] { 4, 8 });
        
    
       /* 
        //t1 = new DenseTensor<float>(sourceData, dimensions);
        t2 = new DenseTensor<float>(sourceData, dimensions);
        //Tensor<float> t1 = new DenseTensor<float>(sourceData, dimensions);
        string path = "Assets/Karen/Models/knn_test.onnx";
        var session = new InferenceSession(path);
        var inputs = new List<NamedOnnxValue>()
                {
                    NamedOnnxValue.CreateFromTensor<float>("float_input", t1)
                };
        using (var results = session.Run(inputs))
        {
            foreach (var r in results) {
            if(r.Name == "output_label") {
                print($"{r.Name}");
                print(r);
                print(r.AsTensor<float>().ToDenseTensor().Buffer.ToArray());
                print($"{r.AsTensor<float>().GetValue(0)}");
                
            }
            print("entre aqui");
        }
        }*/
        string path = "Assets/_karen/Models/knnn.onnx";
        InferenceSession session = new InferenceSession(path);
        DenseTensor<float> T1;

        float[,] Predict_input = new float[3,375];
      /* Predict_input[0, 0] = 6.0f;
        Predict_input[0, 1] = 148.0f;
        Predict_input[0, 2] = 72.0f;
        Predict_input[0, 3] = 35.0f;
        Predict_input[0, 4] = 0.0f;
        Predict_input[0, 5] = 33.6f;
        Predict_input[0, 6] = 0.627f;
        Predict_input[0, 7] = 50.0f;

        Predict_input[1, 0] = 0.0f;
        Predict_input[1, 1] = 0.0f;
        Predict_input[1, 2] = 0.0f;
        Predict_input[1, 3] = 0.0f;
        Predict_input[1, 4] = 0.0f;
        Predict_input[1, 5] = 0.6f;
        Predict_input[1, 6] = 0.627f;
        Predict_input[1, 7] = 0.0f;*/
        
        for (int i = 1; i< 3; i++)
            {
                for (int j = 1; j < 375; j++)
                {
                    Predict_input[i, j] = 2.3f;
                }
            }


        T1 = Predict_input.ToTensor();
        var inputMeta = session.InputMetadata;
        var outputMeta = session.OutputMetadata;

        var inputs1 = new List<NamedOnnxValue>();

        foreach (var name in inputMeta.Keys)
        {
            print("entre");
            print(name);
            inputs1.Add(NamedOnnxValue.CreateFromTensor<float>(name, T1));
        }
        try
        {
            using(var results = session.Run(inputs1)){
                foreach(var r in results){
                    //.GetValue(0)
                    Tensor<string> prediction=r.AsTensor<string>();
                    predicti.SetPrediction(prediction);
                   // print("prediciton"+prediction);
                }
            }

        }catch (Exception e){
            print("error"+e);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string predict(float[,] Predict_input)
        {
            string path = "Assets/_karen/Models/knnn.onnx";
            InferenceSession session = new InferenceSession(path);
            DenseTensor<float> T1;

            string output_string="";

            T1 = Predict_input.ToTensor();
            var inputMeta = session.InputMetadata;
            var outputMeta = session.OutputMetadata;

            var inputs1 = new List<NamedOnnxValue>();

            foreach (var name in inputMeta.Keys)
            {
                print("entre");
                print(name);
                inputs1.Add(NamedOnnxValue.CreateFromTensor<float>(name, T1));
            }
            try
            {
                using(var results = session.Run(inputs1)){
                    foreach(var r in results){
                        //.GetValue(0)
                        Tensor<string> prediction=r.AsTensor<string>();
                        foreach(var t in prediction){
                            //Prediction is done here
                            print("pred "+t);            
                        }
                    // print("prediciton"+prediction);
                    }
                }
            output_string="Prediccion correcta";
            }catch (Exception e){
                print("error"+e);
                output_string="error";
            }
            return output_string;
        }
}
