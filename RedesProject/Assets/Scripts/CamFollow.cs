using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform targetobject;
    public Vector3 cameraOffset;
    private float smoothFactor = 0.5f;
    private bool lookAtTraget = false;
    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - targetobject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = targetobject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);
        if (lookAtTraget)
        {
            transform.LookAt(targetobject);
        }
    }


}
