using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid
{
    public Flocking flock;
    public float cohesionRadius = 10f; //radius for the cohesion rule
    public float separationRadius = 3f; //radius for the separation rule
    public float allignmentRadius = 5f; //radius for alignment rule
    public float fleeRadius = 5f; //radius for fleeing rule
    public float obstacleRadius = 5f; //radius for the obstacle avoidance rule
    GameObject predator;
    GameObject[] obstacles;

    //3 basic properties each boid must have
    public Vector2 velocity = Vector2.zero;
    public Vector2 position = Vector2.zero;
    public Vector2 acceleration = Vector2.zero;
    public GameObject enemy;
    public float size;

    public Boid(Vector2 velocity, Vector2 position, Flocking flock, float size, GameObject enemy)
    {
        this.velocity = velocity;
        this.position = position;
        this.flock = flock;
        this.enemy = enemy;
        this.size = size;
    }

    public Vector2 Cohesion()
    {
        Vector2 averagePosition = new Vector2(0,0);
        int boidAmount = 0;

        for (int i = 0; i < this.flock.boidList.Count; i++) //for every boid
        {
            Vector2 dist = this.position - this.flock.boidList[i].position; //calc distance
            if (dist.magnitude < cohesionRadius && this.flock.boidList[i] != this) //if it is close enough, and is NOT this one
            {
                averagePosition += this.flock.boidList[i].position; //add this boids pos to average pos
                boidAmount++; //add 1 to amount of boids nearby
            }
        }
        if (boidAmount > 0) //if any boids are nearby
        {
            averagePosition = averagePosition / boidAmount; //get the average position of the boids that are nearby only
            averagePosition = (averagePosition - this.position) / 100; //work out steering force

            //averagePosition.Normalize();
        }
        return averagePosition;
    }

    public Vector2 Separation()
    {

        Vector2 directionToMove = Vector2.zero;
        int boidAmount = 0;

        for (int i = 0; i < this.flock.boidList.Count; i++) //for every boid
        {
            if (this.flock.boidList[i] != this) 
            { 
                Vector2 dist = (this.position + new Vector2(size/2, size/2)) - (this.flock.boidList[i].position + new Vector2(this.flock.boidList[i].size/2, this.flock.boidList[i].size / 2));
                if (dist.magnitude < separationRadius) //if it is close enough, and is NOT this one
                {
                    boidAmount++;
                    directionToMove += (this.position - this.flock.boidList[i].position);
                }
            }
        }

        if (boidAmount > 0) //if any boids are nearby
        {
            directionToMove = directionToMove / boidAmount; //get the direction to move
            //directionToMove.Normalize();
        }

        return directionToMove;
    }

    //Setting the alignment of the boid
    public Vector2 Alignment()
    {
        Vector2 averageVelocity = Vector2.zero; //create empty average velocity
        int boidAmount = 0; //create empty value to store how many boids are nearby

        for (int i = 0; i < this.flock.boidList.Count; i++) //for every boid
        {
            float dist = Vector2.Distance(this.position, this.flock.boidList[i].position); //check it's distance
            if (dist < allignmentRadius && this.flock.boidList[i] != this) //if it is close enough, and is NOT this one
            {
                averageVelocity += this.flock.boidList[i].velocity; //add that boids velocity to the average
                boidAmount++; //add 1 to amount of boids nearby
            }
        }

        if (boidAmount > 0) //if any boids are nearby
        {
            averageVelocity = averageVelocity / boidAmount; //get the average velocity of the boids that are nearby only
        }
        else
        {
            averageVelocity = this.velocity; //else keep the same velocity
        }

        averageVelocity = (averageVelocity - this.velocity) / 50; //work out steering force
        //averageVelocity.Normalize();

        return averageVelocity;
    }

    public Vector2 Flee()
    {
        Vector2 newHeading = Vector2.zero;

        predator = GameObject.FindGameObjectWithTag("Player"); //get player obj
        Vector2 predPos = predator.transform.position; //get player pos

        float distToPred = Vector2.Distance(this.position, predPos); //check it's distance
        if(distToPred < fleeRadius) //if its too close
        {
            //go to the negative direction of the predator
            newHeading = (this.position - predPos);
        }

        return newHeading;
    }

    public Vector2 AvoidObstacle()
    {
        Vector2 newDir = Vector2.zero;
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            Vector2 obstaclePos = obstacle.transform.position;
            float distToWall = Vector2.Distance(this.position, obstaclePos);

            if (distToWall < obstacleRadius)
            {

            }

        }
        return newDir;
    }

}
