using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShooter : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shotCooldown;
    [SerializeField] private float bulletForce;
    [SerializeField] private bool canFire = false;
    [SerializeField] private ProjectionV3 projectionV3;
    [SerializeField] private float bulletMass = 1f;
    private float currentCooldown;

    // Start is called before the first frame update
    void Start()
    {
        currentCooldown = shotCooldown;
    }

    // Update is called once per frame
    void Update()
    {

        if (canFire)
        {
            projectionV3.ShowTrajectoryLine(firePoint.position, firePoint.transform.forward * bulletForce / bulletMass);

            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.mass = bulletMass;
                rb.AddForce(firePoint.transform.forward * bulletForce, ForceMode.Impulse);
                currentCooldown = shotCooldown;
            }
        }
    }
}
