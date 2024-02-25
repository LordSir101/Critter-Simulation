using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FoodEatenDetection : MonoBehaviour
{
    public int energyValue;
    void Start()
    {
        Dictionary<int,Color> colors = new Dictionary<int, Color>(){
            {0, new Color32(219, 59, 55, 255 )}, //red
            {1, new Color32(26, 184, 217, 255 )}, //blue
            {2, new Color32(46, 154, 34, 255 )},
            {3, new Color32(255, 230, 52, 255 )}
        };
        
        int num = UnityEngine.Random.Range(0,4);
        energyValue = num * 10;
        gameObject.GetComponent<Light2D>().color = colors[num];
        gameObject.GetComponent<Light2D>().pointLightOuterRadius = 1+(0.1f*num);
        gameObject.transform.localScale = new Vector3(1+(0.2f*num),1+(0.2f*num),0);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        Critter critter = collision.GetComponent<Critter>();
        critter.EatFood(energyValue);
        Destroy(gameObject);
    }
}
