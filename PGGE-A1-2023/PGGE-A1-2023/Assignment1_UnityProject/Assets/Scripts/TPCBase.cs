using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PGGE
{
    // The base class for all third-person camera controllers
    public abstract class TPCBase
    {
        protected Transform mCameraTransform;
        protected Transform mPlayerTransform;

        public Transform CameraTransform
        {
            get
            {
                return mCameraTransform;
            }
        }
        public Transform PlayerTransform
        {
            get
            {
                return mPlayerTransform;
            }
        }

        public TPCBase(Transform cameraTransform, Transform playerTransform)
        {
            mCameraTransform = cameraTransform;
            mPlayerTransform = playerTransform;
        }

        public void RepositionCamera()
        {
            //-------------------------------------------------------------------
            // Implement here.
            //-------------------------------------------------------------------
            //-------------------------------------------------------------------
            // Hints:
            //-------------------------------------------------------------------
            // check collision between camera and the player.
            // find the nearest collision point to the player
            // shift the camera position to the nearest intersected point
            //-------------------------------------------------------------------
            int layerMask = 1 << 8;

            layerMask = ~layerMask;

            RaycastHit hit;

            Vector3 newCameraTransform = mCameraTransform.position;

            float playerDistance = Vector3.Distance(mCameraTransform.position * 0.4f, mPlayerTransform.position * 0.1f);

            if (Physics.Raycast(mCameraTransform.position, mCameraTransform.TransformDirection(Vector3.forward), 
                out hit, playerDistance, layerMask))
            {
                // New camera position will be at the collision point.
                newCameraTransform = hit.point;
                mCameraTransform.position = newCameraTransform;
            }
            else
            {
                mCameraTransform.position = newCameraTransform;
            }
        }

        public abstract void Update();
    }
}
