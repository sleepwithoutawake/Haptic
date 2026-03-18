using UnityEngine;

public class SoftShapeHapticFeedback : MonoBehaviour
{
    public SendToArduino sendToArduino;
    public Transform target;
    public Collider targetCollider;
    public float activationDistance = 2.5f;
    public int idleAngle = 0;
    public int proximityMinAngle = 10;
    public int proximityMaxAngle = 45;
    public int pressMinAngle = 60;
    public int pressMaxAngle = 120;
    public float shapeStrength = 0.35f;

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
        if (sendToArduino == null || target == null || targetCollider == null)
        {
            return;
        }

        Vector3 closestPoint = targetCollider.ClosestPoint(transform.position);
        float distanceToSurface = Vector3.Distance(transform.position, closestPoint);

        if (distanceToSurface > activationDistance)
        {
            sendToArduino.SendAngle(idleAngle);
            return;
        }

        Bounds bounds = targetCollider.bounds;
        bool insideBounds = bounds.Contains(transform.position);

        if (!insideBounds)
        {
            float proximityFactor = 1f - Mathf.Clamp01(distanceToSurface / activationDistance);
            int angle = Mathf.RoundToInt(Mathf.Lerp(proximityMinAngle, proximityMaxAngle, proximityFactor));
            sendToArduino.SendAngle(angle);
            return;
        }

        float topY = bounds.max.y;
        float bottomY = bounds.min.y;
        float depthRange = Mathf.Max(0.001f, topY - bottomY);
        float pressDepth = Mathf.Clamp(topY - transform.position.y, 0f, depthRange);
        float pressFactor = Mathf.Clamp01(pressDepth / depthRange);

        float horizontalRange = Mathf.Max(0.001f, bounds.extents.x);
        float offsetFromCenter = Mathf.Abs(transform.position.x - bounds.center.x);
        float shapeFactor = 1f - Mathf.Clamp01(offsetFromCenter / horizontalRange);
        float combinedFactor = Mathf.Clamp01(pressFactor + (shapeFactor * shapeStrength));

        int pressAngle = Mathf.RoundToInt(Mathf.Lerp(pressMinAngle, pressMaxAngle, combinedFactor));
        sendToArduino.SendAngle(pressAngle);
    }
}
