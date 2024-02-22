
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.iOS;
using UnityEngine;

public class Critter : MonoBehaviour
{
    public int speed;
    public int sense;
    public int breed;

    public int energy;
    public int move_speedX = 5;
    public int move_speedY = 5;

    public float timeOfCollsion;
    public bool inCollisionState = false;

    public Sprite surprisedEyes;
    public Sprite normalEyes;
    // Start is called before the first frame update
    void Start()
    {
        move_speedX = Random.Range(-11,11);
        move_speedY = Random.Range(-11,11);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(move_speedX,move_speedY,0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(move_speedX,move_speedY,0);
            inCollisionState = false;
            AnimateCollisionState();
        }
    }

    public void EatFood()
    {
        energy += 10;
        Debug.Log(energy);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        AnimateCollisionState();
        move_speedX = Random.Range(-11,11);
        move_speedY = Random.Range(-11,11);
        timeOfCollsion = Time.time;
        inCollisionState = true;
    }

    private void AnimateCollisionState(){
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
