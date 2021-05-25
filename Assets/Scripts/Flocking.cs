using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : MonoBehaviour
{
    public GameObject enemy;
    public GameObject parent;
    public Boid boid;
    public float vlim;
    public int boidAmount;
    public float speed;
    public GameObject player;
    //public float scatterRange;
    public int edgeDirChange;

    [SerializeField]
    public List<Boid> boidList = new List<Boid>();

    int width = 100;
    int height = 100;

    int state1 = 1;
    int state2 = -20;
    int m1 = 1;
    int m2 = 1;
    int m3 = 1;
    int m4 = 1;
    int m5 = 1;

    public void Start()
    {
        for (int i=0; i<boidAmount; i++)
        {
            //apply random values
            Vector2 velocity = new Vector2(Random.Range(-vlim, vlim), Random.Range(-vlim, vlim)); //set a random velocity
            Vector2 position = new Vector2(Random.Range((-width/2)+1, (width/2)-1), Random.Range((-height/2)+1, (height/2)-1)); //set a random position

            float size = Random.Range(0.5f, 2f); //give a random size

            GameObject newBoidObj = Instantiate(enemy, position, Quaternion.identity); //create an object for the boid
            newBoidObj.gameObject.transform.localScale = new Vector3(size, size, size); //set the objects size
            Boid newBoid = new Boid(velocity, position, this, size, newBoidObj); //create a boid with a reference to the object created
            boidList.Add(newBoid); //add that boid to a list
        }
    }


    public void Update()
    {
        //rules
        Vector2 cohesion = Vector2.zero;
        Vector2 separation = Vector2.zero;
        Vector2 alignment = Vector2.zero;
        Vector2 flee = Vector2.zero;
        Vector2 avoidObstacle = Vector2.zero;
        int xMin = -width / 2;
        int xMax = width / 2;
        int yMin = -height / 2;
        int yMax = height / 2;

        if (Input.GetKey("space"))
        {
            m1 = state2;
        }
        else
        {
            m1 = state1;
        }
        
        for (int i = 0; i < boidList.Count; i++) //for every boid
        {
            Vector2 newPos = Vector2.zero;
            
            //get each value for each boid
            cohesion = m1 * boidList[i].Cohesion();  
            separation = m2 * boidList[i].Separation();
            alignment = m3 * boidList[i].Alignment();
            flee = m4 * boidList[i].Flee();

            boidList[i].velocity = boidList[i].velocity + cohesion + separation + alignment + flee;
            //limit the speed
            boidList[i].velocity = LimitVelocity(boidList[i].velocity);

            newPos = boidList[i].position += (boidList[i].velocity.normalized * speed) * Time.deltaTime;

            //Keep it scrolling on x axis
            if ((boidList[i].position).x > xMax)
            {
                boidList[i].position.x = -width / 2;
            }
            else if ((boidList[i].position).x < xMin)
            {
                boidList[i].position.x = width / 2;
            }
            if(newPos.y > yMax)
            {
                boidList[i].velocity.y -= edgeDirChange;
            }
            else if (newPos.y < yMin)
            {
                boidList[i].velocity.y += edgeDirChange;
            }

            boidList[i].position += (boidList[i].velocity.normalized * speed) * Time.deltaTime;
            boidList[i].enemy.transform.position = boidList[i].position;
            boidList[i].enemy.transform.rotation = Quaternion.LookRotation(boidList[i].velocity);
                
        }
    } 

    public Vector2 LimitVelocity(Vector2 velocity)
    {

        if (velocity.magnitude > vlim)
        {
            Vector2 clamped = Vector2.ClampMagnitude(velocity, vlim);
            velocity = clamped;
        }
        return velocity;
    }

}


