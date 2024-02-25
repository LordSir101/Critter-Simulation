using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CritterSpawner : MonoBehaviour
{
    public critterBuilder critterBuilder;
    public GameObject critterTemplate;

    private int numCrittersToSpawn = 8;

    private Vector3 mapSize;
    void Start()
    {
        mapSize = GameObject.FindAnyObjectByType<EnvironmentManager>().GetComponent<EnvironmentManager>().mapSize;
        int buffer = 5;
        int width = (int)mapSize[0] / 2;
        int height = (int)mapSize[1] / 2;
        for(int i = 0; i < numCrittersToSpawn; i++){
            int xoffset = Random.Range(-width + buffer, width - buffer);
            int yoffset = Random.Range(-height + buffer, height - buffer);

            // The parent object of template is an empty object used to control the pivot point of the sprite
            // We need to get the child object for the actual critter template
            GameObject critter = Instantiate(critterTemplate, new Vector3(transform.position.x + xoffset, transform.position.y + yoffset, 0), transform.rotation);
            //GameObject critter = critterParent.transform.GetChild(0).gameObject;

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
