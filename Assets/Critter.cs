using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Critter : MonoBehaviour
{
    // Stats
    public int speed;
    public int sense;
    public int breed;
    public int size;
    public int speciesNum;
    public Color color = new Color32(0, 0, 0, 255 );

    protected float speedScale = 2f;
    protected int senseScale = 4;
    protected int breedScale = 2;
    public bool dead = false;

    // Lifespan
    public int energy = 80;
    protected float timeOfLastEnergyConsumption;
    protected int energyUsageInterval = 6;

    //[SerializeField] protected GameObject critterManager;
    protected GameObject critterManager;

    // Movement
    public GameObject movementTarget;

    protected Vector3 targetFoodPos;
    protected GameObject targetFood;
    protected bool foundFood = false;

    public float timeOfCollsion;
    public bool inCollisionState = false;

    // Animation
    public Sprite surprisedEyes;
    public Sprite normalEyes;

    protected LineRenderer lineRenderer;

    protected Button visionToggle;

    void Start()
    {
        Setup();
        UseEnergy();
        ScanForFood();
    }

    void Update()
    {
        if(dead){return;}
        if(Time.time - timeOfCollsion >= 1 && inCollisionState){
            inCollisionState = false;
            AnimateCollisionState();
        }
        if(!targetFood || !foundFood)
        {
            ScanForFood();
        }

        if(lineRenderer.enabled){
            DrawVision();
        }
        

        if(Time.time - timeOfLastEnergyConsumption >= energyUsageInterval){
            AttemptBreed();
            UseEnergy();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void ScanForFood(){
        // Find nearby food based on sense stat
        List<Collider2D> nearbyFood = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2 (transform.position.x, transform.position.y), (sense+3)*senseScale, new ContactFilter2D().NoFilter(), nearbyFood);

        double mostEfficient = Int32.MinValue;
        int foodFound = 0;

        foreach(Collider2D collider in nearbyFood)
        {
            // only check the colliders in the circle that are food
            if(collider.transform.tag == "Food"){
                foodFound++;
                float xCoord = collider.transform.position.x;
                float yCoord = collider.transform.position.y;

                // The critter will target the food that has the most energy per distance traveled
                int energyValue = collider.transform.gameObject.GetComponent<Food>().energyValue;
                double distSquared = Math.Pow((double)xCoord - transform.position.x ,2) + Math.Pow((double)yCoord - transform.position.y, 2);

                double effciency = energyValue / distSquared;
                if(effciency > mostEfficient)
                {
                    mostEfficient = effciency;
                    targetFoodPos = new Vector3(xCoord,yCoord,0);
                    targetFood = collider.transform.gameObject;
                    foundFood = true;
                }
                
            }
        }

        // if there is no food, create an invisible target to move towards at a random location near the critter;
        if(foodFound == 0 && !targetFood){
            //Vector3 mapSize = GameObject.FindAnyObjectByType<EnvironmentManager>().GetComponent<EnvironmentManager>().mapSize;
            int[] directions = {-1,1};
            float xCoord = UnityEngine.Random.Range(8,20) * directions[UnityEngine.Random.Range(0,2)];
            float yCoord = UnityEngine.Random.Range(8,20) * directions[UnityEngine.Random.Range(0,2)];
            Vector3 targetPos = transform.position + new Vector3(xCoord, yCoord,0);
            targetFood = Instantiate(movementTarget, targetPos, transform.rotation);
            targetFoodPos = targetPos;
            foundFood = false;
        }
        
    }

    private void Move()
    {
        float angle = Mathf.Atan2(targetFoodPos.y - transform.position.y, targetFoodPos.x - transform.position.x ) * Mathf.Rad2Deg;
        // Euler will get a rotation of the sprite's about the z axis towards the given angle
        // The rotation points the x axis to the target.  we add 270 degrees to the angle so the y axis points to the target instead
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (speed+1) * speedScale * 20 * Time.deltaTime);

        // normalize then scale the vector so that the critter always moves with a constant speed towards the target
        Vector3 directionToMove = targetFoodPos - transform.position;
        directionToMove.Normalize();
        gameObject.GetComponent<Rigidbody2D>().velocity = directionToMove * (speed+2) * speedScale;
    }

    protected void DrawVision()
    {
        // draw a circle based on radius and subdivision
        // the line renderer is attatched to the critter and the circle will automatically move with it
        int subdivisions = 15;
        float radius = (sense+3)*senseScale;

        float angleStop = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;

        for(int i = 0; i < subdivisions; i++)
        {
            float x = radius * Mathf.Cos(angleStop*i);
            float y = radius * Mathf.Sin(angleStop * i);

            Vector3 pointInCircle = new Vector3(gameObject.transform.position.x + x,gameObject.transform.position.y + y,0);

            lineRenderer.SetPosition(i,pointInCircle);
        }
    }

    public void EatFood(int energyValue)
    {
        energy += energyValue;
    }

    protected void UseEnergy()
    {
        energy -=  (int) Math.Floor(Math.Pow(size, 1.5f) * 1.5 + 9-size);
        timeOfLastEnergyConsumption = Time.time;

        if(energy <= 0)
        {
            critterManager.GetComponent<CritterManager>().CritterDeath(gameObject);
        }

        Debug.Log(speciesNum + ": " + energy);
    }

    private void AttemptBreed()
    {
        //each point in breed gives approx 1% extra chance to breed
        int chance = UnityEngine.Random.Range(0,1000) - (15 * (breed+1) * breedScale);

       
        //TODO add a chance of evolving the species when a new one is born
        if (chance < 10) {
            int evolveChance = UnityEngine.Random.Range(0,100);
            if(evolveChance <= 3)
            {
                critterManager.GetComponent<CritterManager>().EvolveFromCritter(gameObject);
            }
            energy -= energy/2;
            critterManager.GetComponent<CritterManager>().CritterBirth(speed, sense, breed, speciesNum, color, gameObject, energy);
            
        }
        
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

    public void ToggleLineRenderer()
    {
        lineRenderer.enabled = !lineRenderer.enabled;
    }

    protected void Setup()
    {
        size = speed + sense + breed;
        // set up vision circle
        gameObject.GetComponent<Light2D>().color = color;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        critterManager = GameObject.FindGameObjectWithTag("CritterManager");

        visionToggle = GameObject.FindGameObjectWithTag("VisionToggle").GetComponent<Button>();
        visionToggle.onClick.AddListener(ToggleLineRenderer);

        // toggle the initial state of line renderer to avoid desync when critters spawn
        EnvironmentManager manager = GameObject.FindGameObjectWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
        lineRenderer.enabled = manager.showLines;
    }

}
