using UnityEngine;

public class SimpleKeyboardMover : MonoBehaviour
{
    public float moveSpeed = 3f;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
