using UnityEngine;

public class ClockwiseRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 20f;

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}
