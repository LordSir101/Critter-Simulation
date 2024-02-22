using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEatenDetection : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Critter critter = collision.GetComponent<Critter>();
        critter.EatFood();
        Destroy(gameObject);
    }
}
