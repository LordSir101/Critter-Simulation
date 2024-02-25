using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used as a point for critters to navigate to if they can't find food
public class MovementTarget : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D collision)
    { 
        Destroy(gameObject);
    }
}
