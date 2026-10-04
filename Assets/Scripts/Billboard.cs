using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool lockYAxis = true;
    public bool isUnityPlane = false;

    Transform cam;

    void LateUpdate()
    {
        if (cam == null)
        {
            if (Camera.main == null) return;
            cam = Camera.main.transform;
        }

        Vector3 forward = cam.forward;
        if (lockYAxis)
        {
            forward.y = 0f;
            if (forward.sqrMagnitude < 0.0001f) return;
            forward.Normalize();
        }

        Quaternion rot = Quaternion.LookRotation(forward, Vector3.up);

        if (isUnityPlane) rot *= Quaternion.Euler(-90f, 0f, 0f) * Quaternion.Euler(0f, 180f, 0f);

        transform.rotation = rot;
    }
}