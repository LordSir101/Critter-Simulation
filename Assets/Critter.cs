using UnityEngine;
using System.Collections.Generic;
using System;

public class Critter : MonoBehaviour
{
    private GameObject movementAnchor; // The game object used to dictate the center of the critter
    public int speed;
    public int sense;
    public int breed;

    private int speedScale = 3;
    private int senseScale = 5;

    public int energy;

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
        movementAnchor = gameObject.transform.parent.gameObject; // use this so the movment is relative to the body of the critter
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
       
        Move();
        DrawVision();
    }

    public void EatFood()
    {
        energy += 10;
    }

    private void ScanForFood(){
        timeOfLastScan = Time.time;
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2 (movementAnchor.transform.position.x, movementAnchor.transform.position.y), sense*(senseScale+1), new ContactFilter2D().NoFilter(), results);

        double smallestDist = Int32.MaxValue;
        int foodFound = 0;

        //TODO, find food that is the closest and has largest size
        foreach(Collider2D collider in results)
        {
            //Debug.Log(collider);
            if(collider.transform.tag == "Food"){
                foodFound++;
                float xCoord = collider.transform.position.x;
                float yCoord = collider.transform.position.y;

                double distSquared = Math.Pow((double)xCoord,2) + Math.Pow((double)yCoord, 2);
                if(distSquared < smallestDist)
                {
                    smallestDist = distSquared;
                    targetFood = new Vector3(xCoord,yCoord,0);
                }
            }
        }

        if(foodFound == 0){
            targetFood = new Vector3(UnityEngine.Random.Range(-10,10), UnityEngine.Random.Range(-10,10));
        }

        Debug.Log("Food found: " + foodFound);
    }

    private void Move()
    {
        float angle = Mathf.Atan2(targetFood.y - movementAnchor.transform.position.y, targetFood.x - movementAnchor.transform.position.x ) * Mathf.Rad2Deg;
        // Euler will get a rotation of the sprite's about the z axis towards the given angle
        // The rotation points the x axis to the target.  we add 270 degrees to the angle so the y axis points to the target instead
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
        movementAnchor.transform.rotation = Quaternion.RotateTowards(movementAnchor.transform.rotation, targetRotation, speed * (speedScale+1)* 50* Time.deltaTime);

        //TODO: implement drag to deaccelerate as you get close to food
        movementAnchor.transform.position = Vector3.MoveTowards(movementAnchor.transform.position, targetFood, speed*(speedScale +1)* Time.deltaTime);
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
        float radius = sense*(senseScale + 1);

        float angleStop = 2f * Mathf.PI / subdivisions;

        LineRenderer lineRenderer = movementAnchor.GetComponent<LineRenderer>();
        
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
