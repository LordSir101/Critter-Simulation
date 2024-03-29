using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Critter : MonoBehaviour
{
    // Stats
    public int speed;
    public int baseSpeed = 1;
    protected float speedScale = 2f;

    public int sense;
    public int baseSense = 3;
    public float senseScale = 4f;

    public int breed;
    public int baseBreed = 1;
    protected float breedScale = 2f;
    protected float maxChildren = 1;

    public int size;
    public int speciesNum;
    
    
    // Lifespan
    public int energy = 80;
    protected float timeOfLastEnergyConsumption;
    protected float energyUsageInterval = 6;
    protected float energyStealRate = 2;
    protected float timeLastEnergyStolen;
    protected float amountOfEnergyToSteal = 0;

    protected float timeOfLastBreed;
    protected float breedInterval = 6;

    // Food
    protected Vector3 targetFoodPos;
    protected GameObject targetFood;
    //protected bool foundFood = false;
    public bool goToMovmentTarget = true;
    protected float timeOfLastScan;

    // access stats with dictionary syntax so upgrades can change stats easily
    public float this[String index]
    {
        get => index switch {
            "speedScale" => speedScale,
            "senseScale" => senseScale,
            "breedScale" => breedScale,
            "energyUsageInterval" => energyUsageInterval,
            "breedInterval" => breedInterval,
            "maxChildren" => maxChildren,
            "amountOfEnergyToSteal" => amountOfEnergyToSteal,
            _ => throw new IndexOutOfRangeException()
        };
        set {
            switch (index) {
                case "speedScale": speedScale = value; break;
                case "senseScale": senseScale = value; break;
                case "breedScale": breedScale = value; break;
                case "energyUsageInterval": energyUsageInterval = value; break;
                case "breedInterval": breedInterval = value; break;
                case "maxChildren": maxChildren = value; break;
                case "amountOfEnergyToSteal": amountOfEnergyToSteal = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }
    }


    void Start()
    {
        Setup();
        UseEnergy();
        ScanForFood();
    }

    void Update()
    {
        ScanForFood();
        
        if(Time.time - timeOfLastEnergyConsumption >= energyUsageInterval){
            UseEnergy();
        }

        if(Time.time - timeOfLastBreed >= breedInterval){
            AttemptBreed();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void ScanForFood(){
        timeOfLastScan = Time.time;
        
        // Find nearby food based on sense stat
        List<Collider2D> nearbyFood = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2 (transform.position.x, transform.position.y), (sense+baseSense)*senseScale, new ContactFilter2D().NoFilter(), nearbyFood);

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
                    //foundFood = true;
                    goToMovmentTarget = false;
                }
                
            }
        }

        // if there is no food, create an invisible target to move towards at a random location near the critter;
        if(foodFound == 0 && (!targetFood.activeInHierarchy || !goToMovmentTarget)){
            
            targetFood = FoodSpawner.SharedInstance.getMovementTarget(gameObject, transform.position);
            targetFoodPos = targetFood.transform.position;
            //foundFood = false;
            goToMovmentTarget = true;
        }
        
    }

    private void Move()
    {
        float angle = Mathf.Atan2(targetFoodPos.y - transform.position.y, targetFoodPos.x - transform.position.x ) * Mathf.Rad2Deg;
        // Euler will get a rotation of the sprite's about the z axis towards the given angle
        // The rotation points the x axis to the target.  we add 270 degrees to the angle so the y axis points to the target instead
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (speed+baseSpeed) * speedScale * 20 * Time.deltaTime);

        // normalize then scale the vector so that the critter always moves with a constant speed towards the target
        Vector3 directionToMove = targetFoodPos - transform.position;
        directionToMove.Normalize();
        gameObject.GetComponent<Rigidbody2D>().velocity = directionToMove * (speed+baseSpeed) * speedScale;
    }

    public void EatFood(int energyValue)
    {
        energy += energyValue;
    }

    protected void UseEnergy()
    {
        // energy cost is exponentially higher the bigger the critter is
        energy -=  (int) Math.Floor(Math.Pow(size, 1.5f) * 1.5 + 9-size);
        timeOfLastEnergyConsumption = Time.time;

        if(energy <= 0)
        {
            CritterManager.SharedInstance.CritterDeath(gameObject);
        }
    }

    private void AttemptBreed()
    {
        //each point in breed gives approx "breed scale" % extra chance to breed
        float chance = UnityEngine.Random.Range(0,101) - ((breed+baseBreed) * breedScale);

        if (chance < 1) {
            int evolveChance = UnityEngine.Random.Range(0,101);
            if(evolveChance <= 3)
            {
                CritterManager.SharedInstance.EvolveFromCritter(gameObject);
            }
            energy -= energy/2;

            CritterManager.SharedInstance.CritterBirth(speed, sense, breed, speciesNum, gameObject.GetComponent<CritterInformationDisplay>().color, gameObject, energy);

            // For every child after the first, it has a 10% chance of being created increased by breed stat
            for(int i = 0; i < maxChildren - 1; i++)
            {
                chance = UnityEngine.Random.Range(0,11) - (breed+baseBreed);
                if (chance < 1)
                {
                    CritterManager.SharedInstance.CritterBirth(speed, sense, breed, speciesNum, gameObject.GetComponent<CritterInformationDisplay>().color, gameObject, energy);
                }
            }
        }

        timeOfLastBreed = Time.time;
    }

    void StealEnergy(Collision2D collision)
    {
        if(amountOfEnergyToSteal > 0 && Time.time - timeLastEnergyStolen >= energyStealRate)
        {
            Critter target = collision.gameObject.GetComponent<Critter>();
            target.energy -= (int) amountOfEnergyToSteal;
            energy += (int) amountOfEnergyToSteal;
            Debug.Log("EnergyStolen:" + amountOfEnergyToSteal);
            timeLastEnergyStolen = Time.time;
        }
    }
    
    protected void Setup()
    {
        size = speed + sense + breed;
        targetFood = FoodSpawner.SharedInstance.getMovementTarget(gameObject, transform.position);
        targetFoodPos = targetFood.transform.position;
        timeLastEnergyStolen = Time.time;
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        gameObject.GetComponent<Animation>().AnimateCollisionState();

        // A critter can steal energy with certain upgrades
        StealEnergy(collision);
        
    }

}
