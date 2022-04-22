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
        public string hola;
        public void SetPrediction(Tensor<string> predict){
           // predicted=t.AsFloats();
          //  predictedValue = System.Array.IndexOf(predicted,predicted.Max());
            foreach(var t in predict){
                print("pred "+t);
                hola +=t+"";
                
                //predicted.Add(t);
            }
        }
    }



    public Prediction predicti;



    private float[,] matriz;
    //var denseTensor = new DenseTensor<int>(new[] { 3, 5 });
    async void Start()
    {
   
        /*string path = "Assets/_karen/Models/knnn.onnx";
        InferenceSession session = new InferenceSession(path);
        DenseTensor<float> T1;

        float[,] Predict_input = new float[3,375];  
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
                  //  predicti.SetPrediction(prediction);
                   // print("prediciton"+prediction);
                }
            }

        }catch (Exception e){
            print("error"+e);
        }*/
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<string> predict(float[,] Predict_input)
        {
            string path = "Assets/_karen/Models/knnn.onnx";
            InferenceSession session = new InferenceSession(path);
            
            //Create Tensor
            DenseTensor<float> T1;

            T1 = Predict_input.ToTensor();
            var inputMeta = session.InputMetadata;
            var outputMeta = session.OutputMetadata;

            var inputs1 = new List<NamedOnnxValue>();

            //Add values of Tensor to List
            foreach (var name in inputMeta.Keys)
            {
                inputs1.Add(NamedOnnxValue.CreateFromTensor<float>(name, T1));

            }
            var output_variable = new List<string>();
            try
            {
                using(var results = session.Run(inputs1)){
                    //Revisando la prediccion
                    foreach(var r in results){
                        //.GetValue(0)
                        Tensor<string> prediction=r.AsTensor<string>();
                        
                         foreach(var t in prediction){
                            //print("pred "+t);
                            output_variable.Add(t);
                        }
                        //predicti.SetPrediction(prediction);
                        
                    // print("prediciton"+prediction);
                    }
                }
  
            }catch (Exception e){
                print("error"+e);

            }
            return output_variable;
        }
}
