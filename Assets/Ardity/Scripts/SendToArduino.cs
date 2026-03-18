using UnityEngine;

public class SendToArduino : MonoBehaviour
{
    public SerialController serialController;
    public int valueServo = 0;
    public int stepServo = 30;
    public bool allowKeyboardTest = true;
    private int lastSentAngle = -1;

    void Update()
    {
        if (allowKeyboardTest && Input.GetKeyDown(KeyCode.Space))
        {
            SendAngle(valueServo + stepServo);
        }
    }

    public void SendAngle(int angle)
    {
        valueServo = Mathf.Clamp(angle, 0, 180);
        if (serialController == null || valueServo == lastSentAngle)
        {
            return;
        }

        serialController.SendSerialMessage("b" + valueServo.ToString() + "e");
        lastSentAngle = valueServo;
    }
}
