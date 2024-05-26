using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DayNightCycle : NetworkBehaviour
{
    [SerializeField] private float dayDurationInSeconds;
    [SerializeField] private float nightDurationInSeconds;

    [SerializeField] private Light sun;
    [SerializeField] private Light moon;

    [SerializeField] private EnemySpawner[] nightSpawners;

    private float timeLeft;
    private float rotationSpeed;
    private bool isDay = true;

    private float halfOfDayDurationToDegrees = 180f;
    private float degreesToStartShadowFading = 120f;
    private float totalRotated = 0f;

    private bool isShadowsFaded = false;

    private void Start()
    {
        //the start point of time is day
        timeLeft = dayDurationInSeconds;
        rotationSpeed = halfOfDayDurationToDegrees / timeLeft;

        for (int i = 0; i < nightSpawners.Length; i++)
        {
            nightSpawners[i].StopSpawning();
        }

        sun.shadowStrength = 0f;
        moon.shadowStrength = 0f;

        StartCoroutine(RaiseTheShadowStrange(sun));

        isShadowsFaded = false;

    }


    void Update()
    {
        totalRotated += Time.deltaTime * rotationSpeed;
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            if (isDay)
                SetNightStatus();
            else
                SetDayStatus();
        }

        if (totalRotated > degreesToStartShadowFading && isShadowsFaded == false)
        {
            if (isDay)
                StartCoroutine(LowerTheShadowStrange(sun));
            else
                StartCoroutine(LowerTheShadowStrange(moon));
        }
    }

    private IEnumerator LowerTheShadowStrange(Light light)
    {
        Debug.Log($"Lowering the {light.name}");

        isShadowsFaded = true;
        while (light.shadowStrength >= 0.03f)
        {
            light.shadowStrength -= rotationSpeed / 1000;
            yield return new WaitForSeconds(0.05f);
        }
        light.shadowStrength = 0f;
        yield break;
    }

    private IEnumerator RaiseTheShadowStrange(Light light)
    {
        isShadowsFaded = true;

        yield return new WaitForSeconds(timeLeft / 10); //skip the peroid where light is to low to cast shadows
        Debug.Log($"Raising the {light.name}");

        while (light.shadowStrength <= 0.825f)
        {
            light.shadowStrength += rotationSpeed / 1200;
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }

    private void ResetCycle(float _timeLeft)
    {
        totalRotated = 0f;
        timeLeft = _timeLeft;
        isDay = !isDay;
        rotationSpeed = halfOfDayDurationToDegrees / timeLeft;

        if (isDay)
            StartCoroutine(RaiseTheShadowStrange(sun));
        else
            StartCoroutine(RaiseTheShadowStrange(moon));

    //   transform.Rotate(Vector3.right * 35); //just need

        isShadowsFaded = false;
    }

    private void SetDayStatus()
    {
        ResetCycle(dayDurationInSeconds);

        for (int i = 0; i < nightSpawners.Length; i++)
        {
            nightSpawners[i].StopSpawning();
        }
    }

    private void SetNightStatus()
    {
        ResetCycle(nightDurationInSeconds);

        for (int i = 0; i < nightSpawners.Length; i++)
        {
            nightSpawners[i].StartSpawning();
        }
    }


}
