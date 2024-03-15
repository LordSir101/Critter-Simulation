using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivoreAnimation : Animation
{
    // [SerializeField] private Sprite normalEyes;
    // [SerializeField] private Sprite surprisedEyes;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
         if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            inCollisionState = false;
            AnimateCollisionState();
        }
        
    }
}
