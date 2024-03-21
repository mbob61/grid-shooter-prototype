using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionV2 : MonoBehaviour
{
    [SerializeField] private int segmentCount = 50;
    [SerializeField] private Transform spawnPoint;

    private Vector3[] segments;
    [SerializeField] private LineRenderer line;
    [SerializeField] private float curveLength = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        segments = new Vector3[segmentCount];
        line.positionCount = segmentCount;
    }

    public void Simulate(Vector3 velocity)
    {
        Vector3 startPos = spawnPoint.transform.position;
        segments[0] = startPos;
        line.SetPosition(0, startPos);

        for (int i = 1; i < segmentCount; i++)
        {
            float timeOfset = Time.fixedDeltaTime * curveLength * i;
            Vector3 gravityOfset = 0.5f * Physics.gravity * Mathf.Pow(timeOfset, 2);

            segments[i] = segments[0] + velocity * timeOfset + gravityOfset;
            line.SetPosition(i, segments[i]);
        }
    }
}
