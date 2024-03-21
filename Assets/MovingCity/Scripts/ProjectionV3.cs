using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionV3 : MonoBehaviour
{

    [SerializeField] private LineRenderer line;
    [SerializeField, Min(3)] private int lineSegments = 60;
    [SerializeField, Min(1)] private float projectionTimeInSeconds = 5;

    private Vector3[] linePoints;
    private Vector3[] lineRendererPoints;

    public void ShowTrajectoryLine(Vector3 startPosition, Vector3 startVelocity)
    {
        // The more points we add, the smoother the line will be.
        float timeStep = projectionTimeInSeconds / lineSegments;

        linePoints = CalculateTrajectoryLine(startPosition, startVelocity, timeStep);

        line.positionCount = lineSegments;
        line.SetPositions(linePoints);

    }

    private Vector3[] CalculateTrajectoryLine(Vector3 startPosition, Vector3 startVelocity, float timeStep)
    {

        lineRendererPoints = new Vector3[lineSegments];
        lineRendererPoints[0] = startPosition;

        // Equation below comes from https://www.youtube.com/watch?v=U3hovyIWBLk
        for (int i = 1; i < lineSegments; i++)
        {
            float timeOffset = timeStep * i;

            Vector3 progressBeforeGravity = startVelocity * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * Mathf.Pow(timeOffset, 2);
            Vector3 newPosition = startPosition + progressBeforeGravity - gravityOffset;
            lineRendererPoints[i] = newPosition;
        }
        return lineRendererPoints;

    }
}
