using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{
    public float asteroidSpeed = 0.9f;
    public CameraShake shaker;
    public GameObject dupa;

    public GameObject asteroidPrefab;
    public GameObject targetPrefab;
    
    private GameObject asteroid;
    private GameObject target;
    private float asteroidDegree;
    
    private float asteroidDistance;
    private float asteroidNewDistance;
    
    private float targetDegree;
    private float t = 0;
    private bool shouldLerp = false;

    private IEnumerator lerper = null;

    private bool stopkurwa = false;

    private void UpdateAsteroid()
    {
        if (asteroid == null) return;

        /*if (shouldLerp)
        {
            asteroidDistance = Mathf.Lerp(asteroidDistance, asteroidNewDistance, t / 10f);
            t += Time.deltaTime;
            Debug.Log(t);

            if (t >= 1)
            {
                shouldLerp = false;
                t = 0;
            }
        }*/

        if (asteroidDistance <= 155f)
        {
            stopkurwa = true;
            float diff = Mathf.Abs((asteroidDegree % (Mathf.PI * 2)) - targetDegree);
            if (diff >= 0.15f)
            {
                Destroy(asteroid);
                Destroy(target);
                StartCoroutine(SpawnAsteroid());
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
        
        asteroidDegree += Time.deltaTime * asteroidSpeed;
        asteroidDistance -= Time.deltaTime * 15f;
        asteroid.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            Mathf.Sin(asteroidDegree) * asteroidDistance,
            Mathf.Cos(asteroidDegree) * asteroidDistance
        );
        //asteroid.GetComponent<RectTransform>().rotation *= Quaternion.Inverse(Quaternion.Euler(0, 0, Time.deltaTime * Random.Range(50f, 75f)));
    }
    
    private void MoveAsteroid()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (lerper == null)
            {
                asteroidNewDistance = asteroidDistance + 30f;
                lerper = Lerp();
                StartCoroutine(lerper);
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (lerper == null) 
            {
                asteroidNewDistance = asteroidDistance - 30f;
                lerper = Lerp();
                StartCoroutine(lerper);
            }
        }
    }

    IEnumerator Lerp()
    {
        float timeElapsed = 0f;
        float duration = 1f;
        while (timeElapsed < duration)
        {
            asteroidDistance = Mathf.Lerp(asteroidDistance, asteroidNewDistance, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        lerper = null;
        asteroidDistance = asteroidNewDistance;
        
    }

    /*private void CheckForHit()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 crosshairPosition = crosshair.anchoredPosition;

            for (int i = 0; i < asteroids.Count; i++)
            {
                if (asteroids[i] == null) continue;
                
                Vector2 asteroidPosition = asteroids[i].GetComponent<RectTransform>().anchoredPosition;

                if (Mathf.Abs(crosshairPosition.x - asteroidPosition.x) <= 45f
                    && Mathf.Abs(crosshairPosition.y - asteroidPosition.y) <= 45f)
                {
                    shaker.Shake(0.2f);
                    Destroy(asteroids[i]);
                    destroyedAsteroids++;

                    if (destroyedAsteroids == asteroidsCount)
                    {
                        UnityEditor.EditorApplication.isPlaying = false;
                    }
                }
                else
                {
                    Debug.Log(asteroidPosition);
                    Debug.Log(crosshairPosition);
                }
            }
        }
    }*/
    
    private void Update()
    {
        if (stopkurwa) return;
        
        MoveAsteroid();
        UpdateAsteroid();
    }

    private void Start()
    {
        StartCoroutine(SpawnAsteroid());
    }

    IEnumerator SpawnAsteroid()
    {
        yield return new WaitForSeconds(2f);
        
        asteroid = Instantiate(asteroidPrefab);
        asteroid.transform.SetParent(dupa.transform);
        asteroidDegree = Random.Range(0, Mathf.PI);
        asteroidDistance = Random.Range(160, 430);
        asteroidNewDistance = asteroidDistance;

        target = Instantiate(targetPrefab);
        target.transform.SetParent(dupa.transform);
        targetDegree = Random.Range(0, Mathf.PI * 2);
        float targetDist = 150f;

        asteroid.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            Mathf.Sin(asteroidDegree) * asteroidDistance,
            Mathf.Cos(asteroidDegree) * asteroidDistance
        );

        asteroid.AddComponent<FaceTarget>();
        asteroid.GetComponent<FaceTarget>().target = dupa.transform;

        yield return null;
        
        target.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            Mathf.Sin(targetDegree) * targetDist,
            Mathf.Cos(targetDegree) * targetDist
        );

        stopkurwa = false;
    }
}
