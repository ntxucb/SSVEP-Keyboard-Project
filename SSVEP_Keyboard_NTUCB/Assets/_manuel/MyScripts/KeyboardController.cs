using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KeyboardController : MonoBehaviour
{

    private int[] values;
    private bool[] keys;
        // Start is called before the first frame update
        

    string key;
    string text;
    public TextMeshProUGUI text_tmp;

    float holdTimer;
    float holdThreshold = 0.1f;



    private void Awake() {
        values = (int[])System.Enum.GetValues(typeof(KeyCode));
        keys = new bool[values.Length];
    }
 

    // Update is called once per frame
    void Update()
    {
        // for (int i = 0, n = values.Length; i < n; i++){

        // keys[i] = Input.GetKey((KeyCode)values[i]);
        // }
        // print(key.);
        if(Input.anyKey)
        {   if(Input.GetKey(KeyCode.Return)) return;
            if(Input.GetKey(KeyCode.Backspace))
            {
                // holdTimer -= Time.deltaTime;
                // if(holdTimer < 0)
                // {   
                //     StartCoroutine(BackspaceCoroutine());   
                // }
                holdTimer += Time.deltaTime;


                    if( holdTimer > holdThreshold)
                    {
                        print("BACKSPACE");
                        if (text.Length >= 1)
                        {
                            removeLastKey();
                            holdTimer = 0f;
                            // text = text.Remove(text.Length - 1);
                            // text_tmp.text = text;
                            // StartCoroutine(BackspaceCoroutine());
                        }
                        else return;

                    }
                

            }
            else {
                key = (string) Input.inputString;
                print(key);

                text += key;
                text_tmp.text = text;
                }
        }
        
        
    }


IEnumerator BackspaceCoroutine()
{
    yield return new WaitForSeconds(0.5f);
    print("Waited!");
    text = text.Remove(text.Length - 1);
    text_tmp.text = text;
}

void removeLastKey(){
    text = text.Remove(text.Length - 1);
    text_tmp.text = text;
}

}
