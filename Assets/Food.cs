using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Food : MonoBehaviour
{
    public int energyValue;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Critter")
        {
            Critter critter = collision.GetComponent<Critter>();
            critter.EatFood(energyValue);
            gameObject.SetActive(false);
        }
        
    }
}
