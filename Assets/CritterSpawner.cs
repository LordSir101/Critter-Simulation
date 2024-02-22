using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner : MonoBehaviour
{
    public critterBuilder critterBuilder;
    public GameObject critterTemplate;
    void Start()
    {
        GameObject critter = Instantiate(critterTemplate, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
        critter.GetComponent<Critter>().speed = MenuInput.speed;
        critter.GetComponent<Critter>().sense = MenuInput.sense;
        critter.GetComponent<Critter>().breed = MenuInput.breed;
        critterBuilder.CreateCritter(MenuInput.speed, MenuInput.sense, MenuInput.breed, critter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
