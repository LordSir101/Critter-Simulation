using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawner : MonoBehaviour
{
    public critterBuilder critterBuilder;
    public GameObject critterTemplate;

    private int numCrittersToSpawn = 1;
    void Start()
    {
        for(int i = 0; i < numCrittersToSpawn; i++){
            int xoffset = Random.Range(-40, 40);
            int yoffset = Random.Range(-30, 30);
            GameObject critter = Instantiate(critterTemplate, new Vector3(transform.position.x + xoffset, transform.position.y + yoffset, 0), transform.rotation);
            critter.GetComponent<Critter>().speed = MenuInput.speed;
            critter.GetComponent<Critter>().sense = MenuInput.sense;
            critter.GetComponent<Critter>().breed = MenuInput.breed;
            critterBuilder.CreateCritter(MenuInput.speed, MenuInput.sense, MenuInput.breed, critter);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
