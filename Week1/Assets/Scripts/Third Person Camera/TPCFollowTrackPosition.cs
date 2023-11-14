using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCFollowTrackPosition : TPCFollow
{
    public TPCFollowTrackPosition(Transform cameraTransform, Transform playerTransform) : base(cameraTransform, playerTransform)
    { 
    }

    public override void Update()
    {
        Quaternion initialRotation = Quaternion.Euler(GameConstants.CameraAngleOffset);

        mCameraTransform.rotation = Quaternion.RotateTowards(mCameraTransform.rotation,
            initialRotation, Time.deltaTime * GameConstants.Damping);

        base.Update();
    }
}
