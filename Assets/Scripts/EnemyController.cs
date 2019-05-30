using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // Range of movement
    public float rangeY = 2f;
    public float speed = 3f;

    public float direction = 1f; // Initial direction
    Vector3 initialPosition;    // To keep the initial position

    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position; // Initial location in Y
    }

    // Update is called once per frame
    void Update()
    {
        float movementY = direction * speed * Time.deltaTime; // How much we are moving
        float newY = transform.position.y + movementY;         // New position

        // Check whether the limit would be passed
        if (Mathf.Abs(newY - initialPosition.y) > rangeY)
        {
            direction *= -1; // Move the other way
        }
        else // If it can move further, move
        {
            transform.Translate(new Vector3(0, movementY, 0)); // Move the object
        }
    }
}