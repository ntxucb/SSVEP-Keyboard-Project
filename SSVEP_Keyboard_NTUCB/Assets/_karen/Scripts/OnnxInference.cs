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
                    var r = results.First();
                    Tensor<string> prediction=r.AsTensor<string>();
                        
                         foreach(var t in prediction){
                            //print("pred "+t);
                            output_variable.Add(t);
                        }

                 
                }
  
            }catch (Exception e){
                print("error"+e);

            }
            return output_variable;
        }
}
