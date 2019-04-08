using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acc = 20; //use acceleration to simulate the movement 
    Vector3 speed = new Vector3(0, 0, 0);
    Vector3 right = new Vector3(1, 0, 0);
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            speed += -right * acc * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            speed += right * acc * Time.deltaTime;
        }
        this.gameObject.transform.position += speed * Time.deltaTime;
    }

    public void resetSpeed()
    {
        speed = new Vector3(0, 0, 0);
    }
}
