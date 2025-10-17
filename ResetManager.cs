using UnityEngine;
using TMPro;

public class ResetManager : MonoBehaviour
{
    public GameObject[] partesPlanta;
    public TextMeshPro textoUI; 

    public void RestaurarPlanta()
    {
        foreach (GameObject parte in partesPlanta)
        {
            parte.SetActive(true);
        }

        textoUI.gameObject.SetActive(false);
    }

}
