using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCFollowTrackPositionAndRotation : TPCFollow
{
    public TPCFollowTrackPositionAndRotation(Transform cameraTransform, Transform playerTransform)
        : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        Quaternion initialRotation = Quaternion.Euler(GameConstants.CameraAngleOffset);

        mCameraTransform.rotation = Quaternion.Lerp(mCameraTransform.rotation, 
            mPlayerTransform.rotation * initialRotation, 
            Time.deltaTime * GameConstants.Damping);

        base.Update();
    }
}
