using UnityEngine;
using System.Collections.Generic;
using System;

public class Critter : MonoBehaviour
{
    public int speed;
    public int sense;
    public int breed;

    public int energy;
    public int move_speedX = 5;
    public int move_speedY = 5;
    private Vector3 targetFood;

    public float timeOfCollsion;
    public bool inCollisionState = false;

    public float scanInterval = 2;
    private float timeOfLastScan;

    public Sprite surprisedEyes;
    public Sprite normalEyes;

    
    // Start is called before the first frame update
    void Start()
    {
        ScanForFood();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(move_speedX,move_speedY,0);
            inCollisionState = false;
            AnimateCollisionState();
        }
        if(Time.time - timeOfLastScan >= scanInterval)
        {
            ScanForFood();
        }

        transform.position = Vector3.MoveTowards(transform.position, targetFood, speed*3* Time.deltaTime);
        DrawVision();
        
    }

    public void EatFood()
    {
        energy += 10;
    }

    private void ScanForFood(){
        timeOfLastScan = Time.time;
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2 (transform.position.x, transform.position.y), sense*5, new ContactFilter2D().NoFilter(), results);

        double smallestDist = Int32.MaxValue;

        //TODO, find food that is the closest and has largest size
        foreach(Collider2D collider in results)
        {
            Debug.Log(collider);
            if(collider.transform.tag == "Food"){
                float xCoord = collider.transform.position.x;
                float yCoord = collider.transform.position.y;

                double distSquared = Math.Pow((double)xCoord,2) + Math.Pow((double)yCoord, 2);
                if(distSquared < smallestDist)
                {
                    smallestDist = distSquared;
                    targetFood = new Vector3(xCoord,yCoord,0);
                }
                // Debug.Log("Food");
                // Debug.Log(xCoord);
                // Debug.Log(yCoord);
            }
            //get distance
           
        }
        Debug.Log(targetFood);
    } 

    private void OnCollisionEnter2D(Collision2D collision) {
        AnimateCollisionState();
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

    private void DrawVision()
    {
        // draw a circle based on radius and subdivision
        // the line renderer is attatched to the critter and the circle will automatically move with it
        int subdivisions = 15;
        float radius = sense*5;

        float angleStop = 2f * Mathf.PI / subdivisions;

        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        
        lineRenderer.positionCount = subdivisions;

        for(int i = 0; i < subdivisions; i++)
        {
            float x = radius * Mathf.Cos(angleStop*i);
            float y = radius * Mathf.Sin(angleStop * i);

            Vector3 pointInCircle = new Vector3(x,y,0);

            lineRenderer.SetPosition(i,pointInCircle);
        }


    }
}
