using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRotator : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(Vector3.right * Input.GetAxis("Vertical") * speed * Time.deltaTime, Space.Self);

        float localX = transform.localRotation.eulerAngles.x;

        if (localX > 180)
        {
            localX = localX - 360;
        }

        if (localX < 0)
        {
            transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }

        if (localX > 45)
        {
            transform.localRotation = Quaternion.Euler(45, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
    }
}
