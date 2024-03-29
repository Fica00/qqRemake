using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    // Exposed properties for shake duration and intensity
    [SerializeField]
    private float shakeDuration = 0.5f;

    [SerializeField]
    private float shakeIntensity = 0.05f;

    // Reference to the camera you want to shake
    public Camera cam;

    // Variables for controlling the shake effect
    private float shakeTimer = 0f;

    void Update()
    {
        if (shakeTimer > 0)
        {
            // Shake the camera
            cam.transform.localPosition = Random.insideUnitSphere * shakeIntensity;

            // Decrease shake timer
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the camera position when shaking is done
            shakeTimer = 0f;
            cam.transform.localPosition = Vector3.zero;
        }
    }

    // Function to trigger camera shake
    public void ShakeCamera()
    {
        shakeTimer = shakeDuration;
    }

    // Property to get or set shake intensity
    public float ShakeIntensity
    {
        get { return shakeIntensity; }
        set { shakeIntensity = value; }
    }
}
