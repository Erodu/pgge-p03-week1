using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCTopDown : TPCBase
{
    public TPCTopDown(Transform cameraTransform, Transform playerTransform) : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        Vector3 targetPos = mPlayerTransform.position;
        targetPos.y += GameConstants.CameraPositionOffset.y;
        Vector3 position = Vector3.Lerp(mCameraTransform.position, targetPos, Time.deltaTime
        * GameConstants.Damping);
        mCameraTransform.position = position;

        Quaternion topDownRotation = Quaternion.Euler(GameConstants.CameraAngleOffset);
        mCameraTransform.rotation = topDownRotation;
    }
}
