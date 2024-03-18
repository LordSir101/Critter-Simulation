using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivoreAnimation : Animation
{
    void Update()
    {
         if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            inCollisionState = false;
            AnimateCollisionState();
        }
        
    }
}
