using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerV2 : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementMask;

    private void Update()
    {

    }

    public Vector3 GetHoveredMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;

        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, 100, placementMask) ? hit.point : Vector3.positiveInfinity;
    }
}
