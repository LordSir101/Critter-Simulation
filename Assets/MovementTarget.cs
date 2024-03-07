using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Used as a point for critters to navigate to if they can't find food
public class MovementTarget : MonoBehaviour
{
    float timeSpawned;
    float timeToExist = 5;

    void Start()
    {
       
    }

    void Update()
    {
        // Movement target will destroy itself after 3 seconds no matter what in case it spawns out of bounds
        if(Time.time - timeSpawned >= timeToExist)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    { 
        // Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        timeSpawned = Time.time;
    }
}
