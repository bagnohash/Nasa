using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AsteroidController : MonoBehaviour
{
    public RectTransform crosshair;
    public float crosshairSpeed = 30f;
    public float asteroidSpeed = 0.9f;
    public CameraShake shaker;
    public GameObject dupa;

    public int asteroidsCount = 5;
    public int destroyedAsteroids = 0;
    private List<GameObject> asteroids = new List<GameObject>();
    private List<float> distance = new List<float>();
    private List<float> degress = new List<float>();
    
    public GameObject asteroidPrefab;

    private IEnumerator SpawnAsteroids()
    {
        for (int i = 0; i < asteroidsCount; i++)
        {
            yield return new WaitForSeconds(Random.Range(3, 5));
            
            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.transform.SetParent(dupa.transform);
            float degree = Random.Range(0, Mathf.PI);
            float dist = Random.Range(160, 430);
            Debug.Log(Mathf.Sin(degree) * dist);
            asteroid.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                Mathf.Sin(degree) * dist,
                Mathf.Cos(degree) * dist
            );

            asteroids.Add(asteroid);
            distance.Add(dist);
            degress.Add(degree);
        }

        yield return null;
    }

    private void UpdateAsteroids()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            if (asteroids[i] == null) continue;
            
            degress[i] += Time.deltaTime * asteroidSpeed;
            asteroids[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(
                Mathf.Sin(degress[i]) * distance[i],
                Mathf.Cos(degress[i]) * distance[i]
            );
        }
    }
    
    private void MoveCrosshair()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (crosshair.anchoredPosition.y <= 430f) crosshair.position += new Vector3(0f, crosshairSpeed * Time.deltaTime, 0f);
        } else if (Input.GetKey(KeyCode.DownArrow) && crosshair.position.y >= 0)
        {
            if (crosshair.anchoredPosition.y >= 160f) crosshair.position -= new Vector3(0f, crosshairSpeed * Time.deltaTime, 0f);
        }
    }

    private void CheckForHit()
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
    }
    
    private void Update()
    {
        MoveCrosshair();
        UpdateAsteroids();
        CheckForHit();
    }

    private void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }
}
