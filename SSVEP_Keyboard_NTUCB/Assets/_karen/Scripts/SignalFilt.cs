using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;
using TMPro;
using System.Text;
using System.IO;


using brainflow;
using brainflow.math;


public class SignalFilt: MonoBehaviour
    {
        
        public static double[,] empty_data;
        public static double[] filtered;

        public static String enemySelected;

        

        //public GameObject textObject;

        //public TextMeshProUGUI enSel;
        void Awake(){

            
            DontDestroyOnLoad(this.gameObject);
            //enSel = textObject.GetComponent<TextMeshProUGUI> ();
        }


        void Update ()
        {
            
                    
            
        
        }
        public static void Metodo(double[,] unprocessed_data)
        {
             if (staticPorts.statusON == true){
                SignalFilt signalFi = new SignalFilt();

                //THIS FUNCTION WAS MAINLY TESTED WITH THE SYNTHETIC BOARD
                print("SignalFiltering Started");
                int board_id = staticPorts.boardIdSelected;
                //double[,] unprocessed_data = openbciConnection.data;
    //            Debug.Log("Num elements SF unprocessed data: " + unprocessed_data.GetLength(1));
                int[] eeg_channels = staticPorts.eeg_channels;
                
                BoardDescr board_descr = staticPorts.board_descr;
                int sampling_rate = board_descr.sampling_rate;
                /*
                int nfft = DataFilter.get_nearest_power_of_two(sampling_rate);
                // use second channel of synthetic board to see 'alpha'
                int channel = eeg_channels[1];
            
                double[] detrend = DataFilter.detrend(unprocessed_data.GetRow(channel), (int)DetrendOperations.LINEAR);
                Tuple<double[], double[]> psd = DataFilter.get_psd_welch (detrend, nfft, nfft / 2, sampling_rate, (int)WindowFunctions.HANNING);
                double band_power_alpha = DataFilter.get_band_power (psd, 7.0, 13.0);
                double band_power_beta = DataFilter.get_band_power (psd, 14.0, 30.0);
                Console.WriteLine ("Alpha/Beta Ratio:" + (band_power_alpha/ band_power_beta));*/

              //  if (unprocessed_data.GetLength(1) % (staticPorts.sampling_rate*2) == 0){
                //print("Entering the segmentation loop");
                //print((unprocessed_data.GetRow (eeg_channels[0]), staticPorts.sampling_rate, 15.0, 5.0, 2, (int)FilterTypes.BUTTERWORTH, 0.0));


                //print("Before processing:");
                /*print("[{0}] "+string.Join (", ", unprocessed_data));
                    //frecuencia de 3 a 100 es 97
                filtered = DataFilter.perform_bandpass (unprocessed_data, BoardShim.get_sampling_rate (board_id), 15.0, 97.0, 2, (int)FilterTypes.BUTTERWORTH, 0.0);
                    //frecuencia de 50Hz
                filtered = DataFilter.perform_bandstop (filtered, BoardShim.get_sampling_rate (board_id), 50.0, 50.0, 6, (int)FilterTypes.CHEBYSHEV_TYPE_1, 1.0);
                print ("Filtered channel " );
                print ("[{0}]"+ string.Join (", ", filtered));  */
                float[,] filtered_EEG = new float[16, 250];


                float[,] matrix_float=new float[16, 375];
                print("ENTERED THE IF");
               for (int i = 0; i < eeg_channels.Length; i++){
                    print("Before processing:");
                    print("[{0}] "+string.Join (", ", unprocessed_data.GetRow (eeg_channels[i])));
                    //frecuencia de 3 a 100 es 97
                    filtered = DataFilter.perform_bandpass (unprocessed_data.GetRow (eeg_channels[i]), BoardShim.get_sampling_rate (board_id), 15.0, 97.0, 2, (int)FilterTypes.BUTTERWORTH, 0.0);
                    //frecuencia de 50Hz
                    filtered = DataFilter.perform_bandstop (filtered, BoardShim.get_sampling_rate (board_id), 50.0, 50.0, 6, (int)FilterTypes.CHEBYSHEV_TYPE_1, 1.0);
                    
                   // filtered.CopyTo(filtered_EEG[i]);
                    //print("filas "+filtered_EEG.GetLength(0)+" col "+filtered_EEG.GetLength(1));

                    //Creando la matriz general
                    //-------------------
                    print("leeeennnnnnnnggggggg "+filtered.Length);
                    
                    if(filtered.Length==375){
                        //filtered_EEG[i]=(float)filtered;

                        print("leeeennnnnnnnggggggg "+filtered.Length);
                        double[,] matrix = ConvertMatrix(filtered, 1, 375);
                        print("Prueba   "+matrix[0,0]);
                        
                        for (int q = 0; q < 1; q++)
                        {
                            for (int u = 0; u < 375; u++)
                            {
                                matrix_float[i, u] =(float)matrix[q, u];
                                
                            }
                        }

                        print("tipo de datooo "+filtered.GetType());
                        print("tipo de datooo 2222"+matrix.GetType());

                        
                        //Aqui se llena el csv para luego analizar los datos
                     /*   if(i>10){
                            signalFi.save_csv(i+"; "+ string.Join ("; ", filtered)+";0");
                        }else{
                            signalFi.save_csv(i+"; "+ string.Join ("; ", filtered)+";1");
                        }*/

                      


                        print ("Filtered channel " + eeg_channels[i]);
                        print ("[{0}]"+ string.Join ("; ", filtered));  




                    }
                }
                //Prediccion
                string output_salida=OnnxInference.predict(matrix_float);
                print("--------------------"+output_salida+"------------");

         


               // unprocessed_data = empty_data;
         //   }

            }  
        }

        static double[,] ConvertMatrix(double[] flat, int m, int n)
        {
            if (flat.Length != m * n)
            {
                throw new ArgumentException("Invalid length");
            }
            double[,] ret = new double[m, n];
            // BlockCopy uses byte lengths: a double is 8 bytes
            Buffer.BlockCopy(flat, 0, ret, 0, flat.Length * sizeof(double));
            return ret;
        }


        void save_csv(string dato){
            string path="Assets/_karen/CSV/canales.csv";
            
            StringBuilder salida=new StringBuilder();

            List<string> lista=new List<string>();
            lista.Add(dato);

            for(int i=0; i<lista.Count; i++)
            {
                salida.AppendLine(string.Join("//////////",lista[i]));
                File.AppendAllText(path,salida.ToString());
            }
        }
    }















