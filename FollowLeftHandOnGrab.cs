using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections.Generic;

public class FollowLeftHandOnGrab : MonoBehaviour, IMixedRealityInputHandler, IMixedRealityHandJointHandler
{
    public GameObject objectToMove; // El objeto (planta) que se moverá
    public Vector3 offset = Vector3.zero; // Desplazamiento respecto a la mano
    private bool isLeftHandGrabbing = false; // Indica si la mano izquierda está cerrada (agarrando)

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

    // Método para detectar si la mano izquierda está cerrada
    private bool IsLeftHandClosed(IDictionary<TrackedHandJoint, MixedRealityPose> handJoints)
    {
        if (handJoints.ContainsKey(TrackedHandJoint.ThumbTip) && handJoints.ContainsKey(TrackedHandJoint.IndexTip))
        {
            var thumb = handJoints[TrackedHandJoint.ThumbTip].Position;
            var index = handJoints[TrackedHandJoint.IndexTip].Position;

            // Si los dedos están lo suficientemente cerca, consideramos que la mano está cerrada.
            return Vector3.Distance(thumb, index) < 0.05f; // Ajusta el valor según lo que necesites.
        }

        return false;
    }

    // Método para manejar el inicio de la acción de input (cuando la mano izquierda realiza un gesto de "Select")
    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.Handedness == Handedness.Left && eventData.MixedRealityInputAction.Description == "Select")
        {
            isLeftHandGrabbing = true; // La mano izquierda está ahora "agarrando"
        }
    }

    // Método para manejar el fin de la acción de input (cuando la mano izquierda libera el gesto de "Select")
    public void OnInputUp(InputEventData eventData)
    {
        if (eventData.Handedness == Handedness.Left && eventData.MixedRealityInputAction.Description == "Select")
        {
            isLeftHandGrabbing = false; // La mano izquierda ya no está "agarrando"
        }
    }

    // Este método se llama cuando se actualizan las articulaciones de la mano.
    public void OnHandJointsUpdated(InputEventData<IDictionary<TrackedHandJoint, MixedRealityPose>> eventData)
    {
        // Verificamos si la mano izquierda está "agarrando" (cerrada) antes de mover el objeto
        if (isLeftHandGrabbing && eventData.Handedness == Handedness.Left)
        {
            // Verificamos si la mano está cerrada antes de mover el objeto
            if (IsLeftHandClosed(eventData.InputData))
            {
                // Si la mano está cerrada, movemos el objeto al lugar de la palma
                if (eventData.InputData.TryGetValue(TrackedHandJoint.Palm, out MixedRealityPose palmPose))
                {
                    objectToMove.transform.position = palmPose.Position + offset; // Actualizamos la posición del objeto
                }
            }
        }
    }
}
