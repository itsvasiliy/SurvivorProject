using Unity.Netcode;
using UnityEngine;

public class DayNightCycle : NetworkBehaviour
{
    [SerializeField] private float dayDurationInSeconds;
    [SerializeField] private float nightDurationInSeconds;

    [SerializeField] private EnemySpawner[] nightSpawners;

    private float timeLeft;
    private float rotationSpeed;
    private bool isDay = true;

    private float halfOfDayDurationToDegrees = 180f;


    private void Start()
    {
        //the start point of time is day
        timeLeft = dayDurationInSeconds;
        rotationSpeed = halfOfDayDurationToDegrees / timeLeft;

        for (int i = 0; i < nightSpawners.Length; i++)
        {
            nightSpawners[i].StopSpawning();
        }
    }


    void Update()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            if (isDay)
                SetNightStatus();
            else
                SetDayStatus();
        }
    }

    private void ResetTimeLeft(float _timeLeft)
    {
        timeLeft = _timeLeft;
        isDay = !isDay;
        rotationSpeed = halfOfDayDurationToDegrees / timeLeft;
    }

    private void SetDayStatus()
    {
        ResetTimeLeft(dayDurationInSeconds);
       
        for(int i = 0; i < nightSpawners.Length; i++)
        {
            nightSpawners[i].StopSpawning();
        }
    }

    private void SetNightStatus()
    {
        ResetTimeLeft(nightDurationInSeconds);

        for (int i = 0; i < nightSpawners.Length; i++)
        {
            nightSpawners[i].StartSpawning();
        }
    }


}
