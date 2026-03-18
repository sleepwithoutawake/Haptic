/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;


/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class SampleUserPolling_ReadWrite : MonoBehaviour
{
    public SerialController serialController;
    private string dataArduino;
    private bool isStreaming = false;
    private string finalData;
    public float force;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

       // Debug.Log("Press A or Z to execute some actions");
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------

        // If you press one of these keys send it to the serial device. A
        // sample serial device that accepts this input is given in the README.
        /*        if (Input.GetKeyDown(KeyCode.A))
                {
                    Debug.Log("Sending A");
                    serialController.SendSerialMessage("A");
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    Debug.Log("Sending Z");
                    serialController.SendSerialMessage("Z");
                }*/


        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------


        // Get the brut message
        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;
       
        

        // rebuilt the message to be between "b" and "e"
        for(int i=0; i < message.Length; i++)
        {
            if (message[i] == 'e') // end of the message
            {
                isStreaming = false;
                finalData =  dataArduino;
               
                dataArduino = ""; //reset the data
            }
            else if (message[i] == 'b') // begin of the message
            {
      
                isStreaming = true;
            }
            else if (message[i] != '\0'  && isStreaming) // the actual message (normally) but not empty
            {
                dataArduino = dataArduino + message[i];
          


            }
        }

        if(string.IsNullOrWhiteSpace(finalData))
            return;


        int dataForce;
        int.TryParse(finalData, out dataForce);

        float voltage = dataForce * 5f / 1023f;
        force = ((95.7f * Mathf.Pow(voltage, 2)) - (106.7f * voltage) - 5.7f) * 0.0097f;
        Debug.Log(force);
        if (force < 0)
        {
            force = 0;
        }

       // Debug.Log(force);


    }
}
