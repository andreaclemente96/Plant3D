using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;

public class ParteInteractiva : MonoBehaviour, IMixedRealityPointerHandler
{
    public string nombreParte;
    public TextMeshPro textoUI;

    public void OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        if (eventData.Handedness == Handedness.Right)
        {
            MostrarNombre();
        }
    }

    public void MostrarNombre()
    {
        textoUI.text = nombreParte;
        textoUI.gameObject.SetActive(true);

        foreach (Transform sibling in transform.parent)
        {
            if (sibling != this.transform)
                sibling.gameObject.SetActive(false);
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData eventData) { }
    public void OnPointerDragged(MixedRealityPointerEventData eventData) { }
    public void OnPointerUp(MixedRealityPointerEventData eventData) { }
}
