using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using brainflow;
using brainflow.math;



/*TO DO 
1. Arreglar el start_stream deberia arrancar cuando el juego comience o keyboard
2. Renombrar los scripts en lugar de openbci... brainflowConnection
3. Crear una clase que itere la cada canal y lo filtre y retorne la señal filtrada
4. Colocar una pausa, que saque un sample un segundo, pause y que retorne a streamear *un bucle*
5. 
*/






public class openbciConnection : MonoBehaviour
{

    public List<string> predicted;


    private BoardShim board_shim = null;
    private int sampling_rate = 0;

    public GameObject connect_btn;
    
    public GameObject disconnect_btn;



    //Saber si se presiono el boton de start Keyboard
    public Button start_btn;
    private bool flag_button =false;


    //poner referencias al subject y session
    public TMP_InputField subject;
    private string str_subject;
    public TMP_InputField session;
    private string str_session;
    public TMP_InputField frequency;
    private string str_frequency;

    private string file_name;
    public static double[,] data;

    private bool markerStop = false; //This variable will prevent the marker to be inserted all the time (we only need one when the scene begins and another one one the scene ends)
    // Start is called before the first frame update

    //number of samples
    private int number_of_data_points = 375;




    void startBoard()
    {
        try
        {
            
            BoardShim.set_log_file("brainflow_log.txt");
            BoardShim.enable_dev_board_logger();

            BrainFlowInputParams input_params = new BrainFlowInputParams();
            int board_id = staticPorts.boardIdSelected;
            // input_params.serial_port = "COM3";
            if(staticPorts.synthetic != true){
                input_params.serial_port = staticPorts.selected_port;
            }

            board_shim = new BoardShim(board_id, input_params);

            // Obtaining the text from the boxes
            str_subject = subject.text;
            str_session = session.text;
            str_frequency = frequency.text;
            staticPorts.freq = float.Parse(str_frequency);

            //CREATING A NEW DIRECTORY
            String path = Application.dataPath+"\\Recordings\\Subject"+str_subject+"\\";
            
            
            if (!Directory.Exists(path)){
                var folder = Directory.CreateDirectory(path);
                file_name = "file://"+folder.FullName+str_session+"_"+str_frequency+"Hz.csv:w";
            }else{
                print("ALREADY CREATED");
                file_name = "file://"+path+str_session+"_"+str_frequency+"Hz.csv:w";
            }
            
            
            //file_name = "file://Subject"+str_subject+"_Session"+str_session+"_"+str_frequency+"Hz.csv:w";
            

            print("FULL PATH: "+file_name);
            board_shim.prepare_session();
            board_shim.start_stream(376, file_name);
            //board_shim.insert_marker(1.0f);
            sampling_rate = BoardShim.get_sampling_rate(board_id);
            print("Sampling rate:"+sampling_rate);
            staticPorts.sampling_rate = BoardShim.get_sampling_rate(board_id);

            Debug.Log("Brainflow streaming was started");
            staticPorts.statusON = true;
            connect_btn.SetActive(false);
            disconnect_btn.SetActive(true);

            staticPorts.eeg_channels =  BoardShim.get_eeg_channels (board_id);


            BoardDescr board_descr = BoardShim.get_board_descr<BoardDescr>(board_id); 
            staticPorts.board_descr = board_descr; //This object will be used in the SignalProcessing script




            
            //DontDestroyOnLoad(connect_btn);
            //DontDestroyOnLoad(disconnect_btn);
            //SetConnectButton();
            DontDestroyOnLoad(this.gameObject);
            
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Why this toggle?
        
        

        if(staticPorts.connect == true){
            startBoard();
            staticPorts.connect = false;
        }

        //status = ON / OFF

        if(staticPorts.statusON == true){
            if (board_shim == null)
            {
                return;
            }

            // adding a delegate with no parameters
            start_btn.onClick.AddListener(NoParamaterOnclick);

            var current_raw_data = board_shim.get_current_board_data(number_of_data_points);
            
            //brainflow_matrix_shape  is channels per time 32x100
           if((current_raw_data.GetLength(1) > number_of_data_points-1) && flag_button==true){
                print("current_raw_data es del mismo tamaño que number_of_data_points");
                SignalFiltering.unprocessed_data= board_shim.get_board_data();
                // SignalFiltering.unprocessed_data = board_shim.get_board_data();
               //print(SignalFiltering.unprocessed_data.GetLength(0)+"   --  "+SignalFiltering.unprocessed_data.GetLength(1));
                var output = SignalFilt.Metodo(SignalFiltering.unprocessed_data);
                predicted = new List<string>();
                 foreach(var t in output) {
                    predicted.Add(t);
                    
                }

                

           }
           /* while(current_raw_data.GetLength(1)<(eeg_segment-1)){
                print("aun bajo el limite");
            }*/
            

            //Insert ONE marker when the game starts.
            if (staticPorts.gameStarted){

                if (markerStop == false){
                    board_shim.insert_marker(staticPorts.freq);
                    markerStop = true;
                }
                
                
            }


            /*
            else if(staticPorts.gameStarted == false){
                board_shim.insert_marker(-2.0f);   
            }
            */            

            //SignalFiltering.unprocessed_data = board_shim.get_board_data();
            // check https://brainflow.readthedocs.io/en/stable/index.html for api ref and more code samples
            //print(data[31,1]);
            /*
            Debug.Log("Num elements: " + data.GetLength(0));
            for (int i=0;i<data.GetLength(0);i++){
                print(i);
                //dt.Add((int)data[1,i]);    
            }
            */
            /*
            if (connect_btn == null){
                connect_btn = GameObject.FindWithTag("connect_btn");
                connect_btn.SetActive(false);
            }
            if (disconnect_btn == null){
                disconnect_btn = GameObject.FindWithTag("disconnect_btn");
                disconnect_btn.SetActive(true);
            }
            */
            
        }
        /*
        if (connect_btn == null){
            connect_btn = GameObject.FindWithTag("connect_btn");
        }
        if (disconnect_btn == null){
            disconnect_btn = GameObject.FindWithTag("disconnect_btn");
        }
        
        if(staticPorts.statusON){
            connect_btn.SetActive(false);
            disconnect_btn.SetActive(true);
        }
        */
    }
    // you need to call release_session and ensure that all resources correctly released
    public void OnDestroy()
    {
        
        print("DESTROYING BOARD");
        
        board_shim.insert_marker(1.0f);
        if (board_shim != null)
        {
            try
            {
                board_shim.release_session();
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
            }
            //staticPorts.connect = false;
            staticPorts.statusON = false;
            //connect_btn.SetActive(true);
            //disconnect_btn.SetActive(false);
            Debug.Log("Brainflow streaming was stopped");
        }
    }
    /*
    public void ToggleConnect(){
        if(PlayerPrefs.GetInt("Connected",0) == 0){
            PlayerPrefs.SetInt("Connected",1);
        }
        else{
            PlayerPrefs.SetInt("Connected", 0);
        }
        SetConnectButton();
    }
    private void SetConnectButton(){
        if(PlayerPrefs.GetInt("Connected", 0) == 0){
            connect_btn.SetActive(false);
            disconnect_btn.SetActive(true);
        }
        else{
            connect_btn.SetActive(true);
            disconnect_btn.SetActive(false);
        }
    }
    */
    /*Verificando si se activo el boton de True*/
    private void NoParamaterOnclick()
    {
        flag_button=true;
    }


    void m_shape()
    {

    }
}