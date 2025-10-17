using System.Diagnostics;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class PartSelector : MonoBehaviour, IMixedRealityPointerHandler
{
    public GameObject plant; // Arrastrar objeto padre 

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (eventData.Pointer.Controller?.ControllerHandedness != Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right)
            return; // Solo responde a la mano derecha

        GameObject clickedObject = eventData.Pointer.Result.Details.Object;

        if (clickedObject == null) return;

        string clickedName = clickedObject.name;

        foreach (Transform part in plant.transform)
        {
            bool isSelected = part.name == clickedName;
            part.gameObject.SetActive(isSelected);
        }

        UnityEngine.Debug.Log("Parte seleccionada: " + clickedName);
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) { }
    public void OnPointerUp(MixedRealityPointerEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
}
