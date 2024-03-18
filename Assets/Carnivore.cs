using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Critter
{
    private bool isEating = false;
    private float eatSpeed = 0.5f;
    private float timeStartedEating;

    private int attackRange = 1;

    private Critter prey;

    private int energyTaken = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<BoxCollider2D>().size += new Vector2(attackRange, attackRange); // make it so that the carnivore can catch critters from a small distance away
        Setup();
        UseEnergy();
        ScanForFood();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEating )
        {
            ScanForFood();
        }

        if(Time.time - timeOfLastEnergyConsumption >= energyUsageInterval){
            AttemptBreed();
            UseEnergy();   
        }

        if(Time.time - timeStartedEating >= eatSpeed && isEating)
        {
            timeStartedEating = Time.time;
            EatPrey();
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
            if(collider.transform.tag == "Critter")
            {
                Critter prey = collider.transform.gameObject.GetComponent<Critter>();
                if(prey.size > size)
                {
                    continue;
                }
                foodFound++;
                float xCoord = collider.transform.position.x;
                float yCoord = collider.transform.position.y;

                // The critter will target the food that has the most energy per distance traveled
                int energyValue = prey.energy;
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

         if(foodFound == 0 && (!targetFood.activeInHierarchy || !goToMovmentTarget)){
            targetFood = FoodSpawner.SharedInstance.getMovementTarget(gameObject, transform.position);
            targetFoodPos = targetFood.transform.position;
            //foundFood = false;
            goToMovmentTarget = true;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if((collision.gameObject == targetFood) && !isEating)
        {
            isEating = true;
            timeStartedEating = Time.time;
            prey = targetFood.GetComponent<Critter>();
            // prevents stuttering when the carnivore is attatched to the prey
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
        }
    }

    private void Move()
    {
        targetFoodPos = targetFood.transform.position;
        float angle = Mathf.Atan2(targetFoodPos.y - transform.position.y, targetFoodPos.x - transform.position.x ) * Mathf.Rad2Deg;
        // Euler will get a rotation of the sprite's about the z axis towards the given angle
        // The rotation points the x axis to the target.  we add 270 degrees to the angle so the y axis points to the target instead
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, (speed+baseSpeed) * speedScale * 20 * Time.deltaTime);

        if(isEating)
        {
            if(prey.gameObject.activeInHierarchy)
            {
                gameObject.transform.position = prey.gameObject.transform.position + new Vector3(1f,1f,0);
            }
        }
        else
        {
            // normalize then scale the vector so that the critter always moves with a constant speed towards the target
            Vector3 directionToMove = targetFoodPos - transform.position;
            directionToMove.Normalize();
            gameObject.GetComponent<Rigidbody2D>().velocity = directionToMove * (speed+baseSpeed) * speedScale;
        }
       
    }

    private void EatPrey()
    {
        // When a carnivore is eating, the prey and carnivore will drain energy from eachother until one dies based on size diff
        if(isEating)
        {
            int preyEnergyLost = (int) Math.Floor(Math.Pow(2, (size - prey.size + 1) * 0.4) * 12);
            int energyUsed = (int) Math.Floor(Math.Pow(2, 0.3) * 5 / (size - prey.size +1));

            // Store the energy we take from the prey.  The carnivore gets it when the prey is dead
            energyTaken += prey.energy - preyEnergyLost <= 0 ? prey.energy : preyEnergyLost;
            prey.energy -= preyEnergyLost;
            energy -= energyUsed;

            if(energy <= 0)
            {
                CritterManager.SharedInstance.CritterDeath(gameObject);
            }

            if(prey.energy <= 0)
            {
                if(prey.gameObject.activeInHierarchy) //in case two carnivores attack the same target
                {
                    CritterManager.SharedInstance.CritterDeath(prey.gameObject);
                }
                energy += energyTaken;
                isEating = false;
                energyTaken = 0;
                gameObject.GetComponent<Rigidbody2D>().simulated = true;
                
            }
        }
        
        
    }
    private void AttemptBreed()
    {
        float chance = UnityEngine.Random.Range(0,100) - (breed+baseBreed) * breedScale;
        
        // Carnivores do not evolve
        if (chance < 1) {
            energy -= energy/2;
            CritterManager.SharedInstance.CritterBirth(speed, sense, breed, speciesNum, gameObject.GetComponent<CritterInformationDisplay>().color, gameObject, energy);
        }
    }

    // private void UseEnergy()
    // {
    //     energy -= (int) Math.Floor(Math.Pow(size, 1.5f) * 1.5 + 9-size);
    //     timeOfLastEnergyConsumption = Time.time;

    //     if(energy <= 0)
    //     {
    //         critterManager.GetComponent<CritterManager>().CritterDeath(gameObject);
    //     }
    // }

    

}
