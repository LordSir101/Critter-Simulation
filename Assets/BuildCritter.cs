using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class critterBuilder : MonoBehaviour
{
    public Sprite speedSprite;
    public Sprite senseSprite;
    public Sprite breedSprite;

    private void SwapPartSlots(int firstIdx, int secondIdx, List<GameObject> partSections)
    {
        (partSections[secondIdx], partSections[firstIdx]) = (partSections[firstIdx], partSections[secondIdx]);
    }

    // Set the sprites and hitboxes for the critter based on its stats
    public void CreateCritter(int speed, int sense, int breed, GameObject template)
    {

        // The components that hold the part sprites of the critter are in the child object
        Transform[] transform = template.transform.GetChild(0).gameObject.GetComponentsInChildren<Transform>();
        //Transform[] transform = {};

        // foreach(Transform component in template.transform)
        // {
        //     if (component.tag == "BodyParts")
        //     {
        //         transform = template.transform.GetChild(0).gameObject.GetComponentsInChildren<Transform>();
        //     }
        // }
        // get all child components of the critter template that have the part tag
        List<GameObject> partSections = new();
        foreach(Transform part in transform){
            if (part.tag == "Part")
            {
                partSections.Add(part.gameObject);
            }

        }
        
        int speedParts = 0;
        int senseParts = 0;
        int breedParts = 0;
        int size = speed + sense + breed;

        // Swap the order of the images of the critter's parts to get different sprite designs based on size
        // The critter is layed out in a 3x3 grid
        // 1 2 3
        // 4 5 6
        // 7 8 9

        // A critter with size 4 should be this. The fourth part is in position 5
        // 1 2 3
        //   5

        // and 5 should be this. The fifth part is in position 6
        // 1 2 3
        // 4   6

        // So we can essentially move the part at index "size" one slot forward in the parts array

        int[] sizesToSwapFor = new int[]{4,5,7,8};
        
        if(sizesToSwapFor.Contains(size))
        {
            SwapPartSlots(size-1, size, partSections);
        }

        // reset all parts of the criter. 
        for (int i = 0; i < partSections.Count; i++)
        {
            SpriteRenderer partImage = partSections[i].GetComponent<SpriteRenderer>();
            partImage.sprite = null;
        }

        // add the approprate image for each part based on the critter's stats
        for (int i = 0; i < partSections.Count; i++)
        {
            SpriteRenderer partImage = partSections[i].GetComponent<SpriteRenderer>();

            if(speed - speedParts > 0)
            {
                partImage.sprite = speedSprite;
                speedParts++;
            }
            else if(sense - senseParts > 0)
            {
                partImage.sprite = senseSprite;
                senseParts++;
            }
            else if(breed - breedParts > 0)
            {
                partImage.sprite = breedSprite;
                breedParts++;
            }
        }

        if(template.GetComponent<BoxCollider2D>()){
            // Adjust hitbox size based on sprite size
            float y_hitbox = (float) Math.Ceiling((double)size/3);
            template.GetComponent<BoxCollider2D>().size = new Vector3(3, y_hitbox, 1);

            // By default, the hitbox is centered on a 3x3 critter so we translate the sprites downward if the hitbox is smaller so that the center of the hitbox is in the center of the sprite
            float offset = 0.5f*(3 - y_hitbox);
            template.transform.GetChild(0).localPosition = new Vector3 (0,-offset,0);

        }
        
    }

    
}
