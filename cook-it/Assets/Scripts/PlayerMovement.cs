using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acc = 20; //use acceleration to simulate the movement 
    Vector2 speed = new Vector2(0, 0);//new Vector3(0, 0, 0);
    Vector2 right = new Vector2(1, 0);//new Vector3(1, 0, 0);
    private GameObject player;
    private Rigidbody2D rbs;
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        rbs = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            speed = -right * acc;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            speed = right * acc;
        }
        else
        {
            resetSpeed();
        }
        //this.gameObject.transform.position += speed * Time.deltaTime;
        
        rbs.MovePosition(rbs.position + (speed * Time.deltaTime));
    }

    public void resetSpeed()
    {
        speed = new Vector2(0, 0);
    }
}
