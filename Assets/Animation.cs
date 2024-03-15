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
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
         if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            inCollisionState = false;
            AnimateCollisionState();
        }
        
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        AnimateCollisionState();
        timeOfCollsion = Time.time;
        inCollisionState = true;
    }

    protected void AnimateCollisionState(){
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
