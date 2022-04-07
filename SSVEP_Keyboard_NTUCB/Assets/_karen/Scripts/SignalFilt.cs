using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System;
using TMPro;


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
                Console.WriteLine ("Alpha/Beta Ratio:" + (band_power_alpha/ band_power_beta));
                */
                print("Llegue aca");
              //  if (unprocessed_data.GetLength(1) % (staticPorts.sampling_rate*2) == 0){
                    print("Llegue aca 2222222");
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

                    //filtered_EEG[i]=(float)filtered;
                    print("leeeennnnnnnnggggggg "+filtered.Length);
                    double[,] matrix = ConvertMatrix(filtered, 1, 255);
                    print("Prueba   "+matrix[0,0]);
                    /*
                    for (int f = 0; f < 1; f++)
                    {
                        for (int j = 0; j < 255; j++)
                        {
                            Console.WriteLine("matrix[{0},{1}] = {2}", f, j, matrix[f, j]);
                        }
                    }*/

                    print("tipo de datooo "+filtered.GetType());
                    print("tipo de datooo 2222"+matrix.GetType());

                    float[,] matrix_float = new float[1, 255];

                    for (int f = 0; f< matrix.GetLength(0); f++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            matrix_float[f, j] = (float)matrix[f, j];
                        }
                    }
                    print("tipo de datooo matrix float"+matrix_float.GetType());
                    print("dato de la matrix "+matrix_float[0,0]);
                  /* for (int f = 0; f < filtered.Length; f++)
                    {
                        for (int j = 0; j < filtered[f].Length; j++)
                        {
                            filtered_EEG[f,j] = filtered[f, j];
                        }
                    }*/
                    print ("Filtered channel " + eeg_channels[i]);
                    print ("[{0}]"+ string.Join (", ", filtered));  



                }
         


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
    }















