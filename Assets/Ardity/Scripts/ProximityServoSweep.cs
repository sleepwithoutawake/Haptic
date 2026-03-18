using UnityEngine;

public class ProximityServoSweep : MonoBehaviour
{
    public SendToArduino sendToArduino;
    public Transform target;
    public float activationDistance = 4f;
    public int idleAngle = 0;
    public int minSweepAngle = 20;
    public int maxSweepAngle = 100;
    public float minSweepSpeed = 0.5f;
    public float maxSweepSpeed = 4f;

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

    void Update()
    {
        if (sendToArduino == null || target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > activationDistance)
        {
            sendToArduino.SendAngle(idleAngle);
            return;
        }

        float normalizedDistance = 1f - Mathf.Clamp01(distance / activationDistance);
        float sweepSpeed = Mathf.Lerp(minSweepSpeed, maxSweepSpeed, normalizedDistance);
        float t = Mathf.PingPong(Time.time * sweepSpeed, 1f);
        int angle = Mathf.RoundToInt(Mathf.Lerp(minSweepAngle, maxSweepAngle, t));

        sendToArduino.SendAngle(angle);
    }
}
