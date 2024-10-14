using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float maxX = 5;
    public float speed = 1;
    public float rotationSpeed = 100;

    void Update()
    {
        transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
        if (transform.position.x < -maxX){
            Destroy(gameObject);
        }
        transform.Rotate(new Vector3(1, 1, 1) * Time.deltaTime * rotationSpeed);  
    }
}
