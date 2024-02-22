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

    private void SwapPreviewSlots(int firstIdx, int secondIdx, List<GameObject> partSections)
    {
        (partSections[secondIdx], partSections[firstIdx]) = (partSections[firstIdx], partSections[secondIdx]);
    }

    public void CreateCritter(int speed, int sense, int breed, GameObject template)
    {

        // get all child components of the critter template that have the partPreview tag
        List<GameObject> partSections = new();
        Transform[] transform = template.GetComponentsInChildren<Transform>();

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

        int[] sizesToSwapFor = new int[]{4,5,7,8};
        
        if(sizesToSwapFor.Contains(size))
        {
            SwapPreviewSlots(size-1, size, partSections);
        }

        // reset all parts of the criter
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

        // Adjust hitbox size based on sprite size
        //double size = speed + sense + breed;
        if(template.GetComponent<BoxCollider2D>()){
            float y_hitbox = (float) Math.Floor((double)size/3);
            template.GetComponent<BoxCollider2D>().size = new Vector3(3, y_hitbox, 1);
            float offset = 0.5f*(3 - y_hitbox); //3 - y_hitbox
            template.GetComponent<BoxCollider2D>().offset = new Vector2(0, offset);

            Debug.Log(template.GetComponent<BoxCollider2D>().offset);
            Debug.Log(template.GetComponent<BoxCollider2D>().size);

        }
        
    }

    
}
