using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {

        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * speed * Time.deltaTime);
    }
}
