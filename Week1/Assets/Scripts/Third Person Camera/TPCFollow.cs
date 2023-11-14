using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TPCFollow : TPCBase
{
    public TPCFollow(Transform cameraTransform, Transform playerTransform) : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        Vector3 forward = mCameraTransform.forward;
        Vector3 right = mCameraTransform.right;
        Vector3 up = mCameraTransform.up;

        Vector3 targetPos = mPlayerTransform.position;

        Vector3 desiredPosition = targetPos + (forward * GameConstants.CameraPositionOffset.z)
            + (right * GameConstants.CameraPositionOffset.x)
            + (up * GameConstants.CameraPositionOffset.y);

        Vector3 position = Vector3.Lerp(mCameraTransform.position, desiredPosition, Time.deltaTime
            * GameConstants.Damping);
        mCameraTransform.position = position;
    }
}
