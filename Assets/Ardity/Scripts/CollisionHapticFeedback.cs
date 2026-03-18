using UnityEngine;

public class CollisionHapticFeedback : MonoBehaviour
{
    public SendToArduino sendToArduino;
    public string targetTag = "HapticTarget";
    public int idleAngle = 0;
    public int contactAngle = 90;

    void Awake()
    {
        if (sendToArduino == null)
        {
            sendToArduino = GetComponent<SendToArduino>();
        }
    }

    void Start()
    {
        if (sendToArduino != null)
        {
            sendToArduino.SendAngle(idleAngle);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (sendToArduino == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag(targetTag))
        {
            sendToArduino.SendAngle(contactAngle);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (sendToArduino == null)
        {
            return;
        }

        if (collision.gameObject.CompareTag(targetTag))
        {
            sendToArduino.SendAngle(idleAngle);
        }
    }
}
