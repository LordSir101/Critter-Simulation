using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.Universal;

public class Critter : MonoBehaviour
{
    //private GameObject movementAnchor; // The game object used to dictate the center of the critter
    public int speed;
    public int sense;
    public int breed;

    private float speedScale = 0.5f;
    private int senseScale = 3;

    public int energy;

    private Vector3 targetFood;

    public float timeOfCollsion;
    public bool inCollisionState = false;

    public float scanInterval = 2;
    private float timeOfLastScan;

    public Sprite surprisedEyes;
    public Sprite normalEyes;

    private LineRenderer lineRenderer;

    
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        //movementAnchor = gameObject.transform.parent.gameObject; // use this so the movment is relative to the body of the critter
        ScanForFood();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeOfCollsion >= 1 && inCollisionState){
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

    public void EatFood(int energyValue)
    {
        energy += energyValue;
    }

    private void ScanForFood(){
        timeOfLastScan = Time.time;
        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2 (transform.position.x, transform.position.y), (sense+1)*senseScale, new ContactFilter2D().NoFilter(), results);

        double mostEfficient = Int32.MinValue;
        int foodFound = 0;

        //TODO, find food that is the closest and has largest size
        foreach(Collider2D collider in results)
        {
            //collider.transform.gameObject.GetComponent<Light2D>().color = Color.red;
            //Debug.Log(collider);
            if(collider.transform.tag == "Food"){
                foodFound++;
                float xCoord = collider.transform.position.x;
                float yCoord = collider.transform.position.y;

                int energyValue = collider.transform.gameObject.GetComponent<FoodEatenDetection>().energyValue;
                double distSquared = Math.Pow((double)xCoord - transform.position.x ,2) + Math.Pow((double)yCoord - transform.position.y, 2);

                double effciency = energyValue / distSquared;
                if(effciency > mostEfficient)
                {
                    mostEfficient = effciency;
                    targetFood = new Vector3(xCoord,yCoord,0);
                }
                
            }
        }

        if(foodFound == 0){
            int[] directions = {-1,1};
            targetFood = new Vector3(UnityEngine.Random.Range(8,20) * directions[UnityEngine.Random.Range(0,2)], UnityEngine.Random.Range(8,20) * directions[UnityEngine.Random.Range(0,2)]);
        }

        Debug.Log("Food found: " + foodFound);
    }

    private void Move()
    {
        float angle = Mathf.Atan2(targetFood.y - transform.position.y, targetFood.x - transform.position.x ) * Mathf.Rad2Deg;
        // Euler will get a rotation of the sprite's about the z axis towards the given angle
        // The rotation points the x axis to the target.  we add 270 degrees to the angle so the y axis points to the target instead
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 270));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (speed+1) * speedScale* 50* Time.deltaTime);

        //TODO: implement drag to deaccelerate as you get close to food
        //movementAnchor.transform.position = Vector3.MoveTowards(movementAnchor.transform.position, targetFood, speed*(speedScale +1)* Time.deltaTime);
        Vector3 vel = targetFood - transform.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = vel * (speed+1) * speedScale;
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
        //float radius = sense*(senseScale + 1);

        float angleStop = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;

        for(int i = 0; i < subdivisions; i++)
        {
            float x = (sense+1)*senseScale * Mathf.Cos(angleStop*i);
            float y = (sense+1)*senseScale * Mathf.Sin(angleStop * i);

            Vector3 pointInCircle = new Vector3(gameObject.transform.position.x + x,gameObject.transform.position.y + y,0);

            lineRenderer.SetPosition(i,pointInCircle);
        }
    }
}
