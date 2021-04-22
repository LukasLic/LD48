using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmeraldEffects : MonoBehaviour
{
#pragma warning disable 0649

    [SerializeField]
    private float rotationSpeed = 90f;
    [SerializeField]
    private AnimationCurve floatingCurve;
    [SerializeField]
    private float xCorrection = 2f;
    [SerializeField]
    private float yCorrection = 0.2f;
    [SerializeField]
    private float maxFlashInterval = 2f;
    [SerializeField]
    private float flashIntensity = 4f;

#pragma warning restore 0649

    private Vector3 origin;

    private float floatingTime;

    private float nextFlash;
    private Renderer objectRenderer;
    private MaterialPropertyBlock _propBlock;
    private Color originalEmissionColor;

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        objectRenderer = GetComponent<Renderer>();

        originalEmissionColor = objectRenderer.material.GetColor("_EmissionColor");
        SetEmission(0f);
        origin = transform.position;

        // Add random offset
        floatingTime = Random.value;
        nextFlash = Random.value * maxFlashInterval;
    }

    private void Update()
    {
        // Rotation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Floating
        floatingTime += Time.deltaTime / xCorrection;
        if(floatingTime > 1f)
        {
            floatingTime -= 1f;
        }

        var height = floatingCurve.Evaluate(floatingTime);
        transform.position = origin + new Vector3(0f, height * yCorrection, 0f);

        // Lightup
        nextFlash -= Time.deltaTime;
        if(nextFlash <= 0f)
        {
            nextFlash = 1f + Random.value * maxFlashInterval;

            StopAllCoroutines();
            StartCoroutine(Flash());
        }
    }

    /// <summary>
    /// One second flash.
    /// </summary>
    /// <returns></returns>
    IEnumerator Flash()
    {
        var steps = 24;
        var flashTime = 0.34f; // Flash time in seconds.

        // Light up
        for (int i = 1; i <= steps / 2; i++)
        {
            float emission = 2 * i * (1f / steps);
            SetEmission(emission);

            yield return new WaitForSeconds(flashTime / steps);
        }

        SetEmission(1f);

        // Light down
        for (int i = 1; i <= steps / 2; i++)
        {
            float emission = 1f - (2 * i * (1f / steps));
            SetEmission(emission);

            yield return new WaitForSeconds(flashTime / steps);
        }

        SetEmission(0f);
    }

    private void SetEmission(float linearSpaceEmission)
    {
        objectRenderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_EmissionColor", originalEmissionColor * Mathf.LinearToGammaSpace(linearSpaceEmission * flashIntensity));
        objectRenderer.SetPropertyBlock(_propBlock);
    }
}
