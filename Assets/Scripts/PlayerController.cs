using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0f;
    public Light light;
    public Vector3 startSize = new Vector3(1f, 1f, 1f);
    public Flocking flock;

    int width = 100;
    int height = 100;

    private void Start()
    {
        transform.localScale = startSize;
        flock = GameObject.FindGameObjectWithTag("Controller").GetComponent<Flocking>();
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(flock.boidList.Count);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        int xMin = -width / 2;
        int xMax = width / 2;
        int yMin = -height / 2;
        int yMax = height / 2;

        Vector3 pos = transform.position;

        Vector3 movement = new Vector3(horizontal, vertical, 0f) * speed * Time.deltaTime;

        Vector3 nextPos = pos += movement * speed;

        if (nextPos.x > xMax)
        {
            pos.x = xMin;
        }
        else if (nextPos.x < xMin)
        {
            pos.x = xMax;
        }
        else if (nextPos.y > yMax - transform.localScale.y)
        {
            pos.y = yMax - 1;
        }
        else if (nextPos.y < (yMin + transform.localScale.y))
        {
            pos.y = yMin + 1;
        }

        transform.position = pos;
        transform.position += movement * speed;
        light.transform.position = new Vector3(transform.position.x, transform.position.y, -30f);

        Vector2 dirToBoid = Vector2.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        GameObject collidedBoid = collision.gameObject;
        for(int i=0; i < flock.boidList.Count; i++)
        {
            if(flock.boidList[i].enemy == collidedBoid)
            {
                Destroy(flock.boidList[i].enemy);
                flock.boidList.RemoveAt(i);
            }
        }

    }

}
