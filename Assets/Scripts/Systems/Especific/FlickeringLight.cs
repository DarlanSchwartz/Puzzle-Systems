using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    private Light lightSource;
    public Vector2 randomIntensity;
    public Vector2 randomRange;
    public float stepVelocityChange = 10;

    private void Awake()
    {
        lightSource = GetComponent<Light>();
    }

    float currentRange;
    float currentInt;

    private void LateUpdate()
    {
        currentInt = Random.Range(randomIntensity.x, randomIntensity.y);
        currentRange = Random.Range(randomRange.x, randomRange.y);

        lightSource.intensity = Mathf.Lerp(lightSource.intensity, currentInt, stepVelocityChange * Time.deltaTime);
        lightSource.range = Mathf.Lerp(lightSource.range, currentRange, stepVelocityChange * Time.deltaTime);
    }
}
