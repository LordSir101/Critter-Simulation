using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public float timeOfCollsion;
    public bool inCollisionState = false;

    // Animation
    [SerializeField] private Sprite surprisedEyes;
    [SerializeField] private Sprite normalEyes;

    void Update()
    {
         if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            inCollisionState = false;
            AnimateCollisionState();
        }
        
    }


    public void AnimateCollisionState(){
        timeOfCollsion = Time.time;
        inCollisionState = true;

        Transform[] transform = gameObject.GetComponentsInChildren<Transform>();

        foreach(Transform part in transform){
            if (part.tag == "Eyes")
            {
                if(inCollisionState)
                {
                    part.GetComponent<SpriteRenderer>().sprite = surprisedEyes;
                }
                else{
                    part.GetComponent<SpriteRenderer>().sprite = normalEyes;
                }
            }
        }
    }
}
