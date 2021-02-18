using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCycleManager : Singleton<DayCycleManager>
{
    [Range(0.1f, 20.0f)]
    [Tooltip("Measured in min/s")]
    public float cycleSpeed = 1.0f;
    [Tooltip("Hours in the day")]
    public float cycleTime = 24.0f;
    [Min(0.0f)]
    [Tooltip("When the sun sets. Don't make higher than cycleTime")]
    public float cycleNight = 20.0f;
    [Min(0.0f)]
    [Tooltip("When the sun rises. Don't make higher than cycleNight")]
    public float cycleDay = 6.0f;
    [Range(0.1f, 1.0f)]
    [Tooltip("How many hours the days will shorten by each day")]
    public float cycleShortenStep = 0.2f;

    [SerializeField] float cycleCurrent = 400.0f;
    private float minTotal;
    private float minNight;
    private float minDay;
    private int day = 0;

    [SerializeField] bool isNight = false;
    [SerializeField] bool isDay = true; 
    public int Day { get { return day; } }
    
    public Light sunlight;
    public Light moonlight;

    public UnityEvent EnteringDay;
    public UnityEvent EnteringNight;

    private void Start()
    {
        if (sunlight.type != LightType.Directional)
        {
            Debug.LogError("Sun not found or not directional");
        }
        else if (moonlight.type != LightType.Directional)
        {
            Debug.LogError("Moon not found or not directional");
        }

        minTotal = cycleTime * 60.0f;
        minNight = cycleNight * 60.0f;
        minDay = cycleDay * 60.0f;

        StartCoroutine(Cycle());
    }

    override protected void OnDestroy()
    {
        StopAllCoroutines();

        base.OnDestroy();
    }

    // Cycle called once a second
    IEnumerator Cycle()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(1);
            _IncrementTime();
            _CheckDay();
            _CheckNight();
        }
    }

    /// <summary>
    /// Adds the cycle speed to current time in minutes
    /// At the max time, will increment a day and shorten day/night
    /// cycle by the cycleShortenStep
    /// </summary>
    void _IncrementTime()
    {
        //Add Time
        cycleCurrent += cycleSpeed;
        //Count Day
        if (cycleCurrent >= minTotal)
        {
            cycleCurrent = 0.0f;
            day++;
            //Shorten cycle;
            if (minDay != minNight)
            {// there is still some day time
                minDay += cycleShortenStep / 2.0f;
                minNight -= cycleShortenStep / 2.0f;
                if (minDay >= minNight)
                {// permanight begins
                    minDay = minNight;
                }
            }
        }
        //Debug.Log("Minute of the day: " + cycleCurrent);
    }

    /// <summary>
    /// Check if current minute is within day time
    /// During day, will turn on light and rotate as a ratio
    /// of the total day minutes
    /// Otherwise shuts off light and rotation
    /// </summary>
    void _CheckDay()
    {
        //Is Day
        if (cycleCurrent >= minDay && cycleCurrent <= minNight)
        {
            //calc daytime from 0 to 1
            float dayTime = (cycleCurrent - minDay) / (minNight - minDay);
            //Sun rotation
            float dayRot = (dayTime * 180.0f);
            sunlight.transform.localRotation = Quaternion.Euler(dayRot, 170, 0);
            if (sunlight.intensity == 0.0f) { sunlight.intensity = 1.0f; }
        }
        //Is Night
        else if (sunlight.intensity == 1.0f)
        {
            isNight = true;
            isDay = false; 
            sunlight.intensity = 0.0f;
            EnteringNight.Invoke();
        }
    }

    /// <summary>
    /// Check if current minute is within night time
    /// During day, will turn on light and rotate as a ratio
    /// of the total night minutes
    /// Otherwise shuts off light and rotation
    /// </summary>
    void _CheckNight()
    {
        //Is Night
        if (cycleCurrent >= minNight || cycleCurrent <= minDay)
        {
            //calc nightime from 0 to 1
            float nightTime = 0.0f;
            float nightLength = (minTotal - minNight) + minDay;
            if (cycleCurrent >= minNight)
            {//before midnight: nighttime = time - sunset
                nightTime = (cycleCurrent - minNight) / nightLength;
            }
            else if (cycleCurrent <= minDay)
            {//after midnight: nighttime = timeToMidnight + time
                nightTime = (minTotal - minNight + cycleCurrent) / nightLength;
            }
            //Moon rotation
            float nightRot = (nightTime * 180.0f);
            moonlight.transform.localRotation = Quaternion.Euler(nightRot, 170, 0);
            if (moonlight.intensity == 0.0f) { moonlight.intensity = 1.0f; }
        }
        //Is Day
        else if (moonlight.intensity == 1.0f)
        {
            isNight = false;
            isDay = true; 
            moonlight.intensity = 0.0f;
            EnteringDay.Invoke();
        }
    }
}
