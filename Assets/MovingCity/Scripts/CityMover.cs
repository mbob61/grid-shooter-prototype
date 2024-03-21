using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityMover : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector3 movementVector = new Vector3(0, 0, 1);
    [SerializeField] private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(transform.position + movementVector * Time.fixedDeltaTime * speed);
        }
    }
}
