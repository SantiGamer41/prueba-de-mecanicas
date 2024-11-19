using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraShake : MonoBehaviour
{
    public Camera mainCamera;
    [Header("Shake Settings")]
    public float shakeDuration = 0.5f; // Duración predeterminada del sacudido
    public float shakeIntensity = 0.5f; // Intensidad del sacudido
    public float dampingSpeed = 1.0f; // Velocidad con la que se detiene el sacudido

    private Vector3 initialPosition;
    private float shakeTimeRemaining;

    private void OnEnable()
    {
        initialPosition = mainCamera.transform.localPosition;
    }

    public void Shake(float duration, float intensity)
    {
        shakeDuration = duration;
        shakeIntensity = intensity;
        shakeTimeRemaining = duration;
    }

    public void Shake()
    {
        shakeTimeRemaining = shakeDuration;
    }

    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            mainCamera.transform.localPosition = initialPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;
            shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeTimeRemaining = 0f;
            mainCamera.transform.localPosition = initialPosition;
        }
    }
}
