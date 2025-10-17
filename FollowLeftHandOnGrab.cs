using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections.Generic;

public class FollowLeftHandOnGrab : MonoBehaviour, IMixedRealityInputHandler, IMixedRealityHandJointHandler
{
    public GameObject objectToMove; 
    public Vector3 offset = Vector3.zero; 
    private bool isLeftHandGrabbing = false; 

    private void OnEnable()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityHandJointHandler>(this);
    }

    private void OnDisable()
    {
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler>(this);
        CoreServices.InputSystem?.UnregisterHandler<IMixedRealityHandJointHandler>(this);
    }

    private bool IsLeftHandClosed(IDictionary<TrackedHandJoint, MixedRealityPose> handJoints)
    {
        if (handJoints.ContainsKey(TrackedHandJoint.ThumbTip) && handJoints.ContainsKey(TrackedHandJoint.IndexTip))
        {
            var thumb = handJoints[TrackedHandJoint.ThumbTip].Position;
            var index = handJoints[TrackedHandJoint.IndexTip].Position;

            
            return Vector3.Distance(thumb, index) < 0.05f; // distancia dedos
        }

        return false;
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.Handedness == Handedness.Left && eventData.MixedRealityInputAction.Description == "Select")
        {
            isLeftHandGrabbing = true; 
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (eventData.Handedness == Handedness.Left && eventData.MixedRealityInputAction.Description == "Select")
        {
            isLeftHandGrabbing = false; 
        }
    }

    public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
    {

        if (isLeftHandGrabbing && eventData.Handedness == Handedness.Left)
        {
    
            if (IsLeftHandClosed(eventData.InputData))
            {
                if (eventData.InputData.TryGetValue(TrackedHandJoint.Palm, out MixedRealityPose palmPose))
                {
                    objectToMove.transform.position = palmPose.Position + offset; 
                }
            }
        }
    }
}

