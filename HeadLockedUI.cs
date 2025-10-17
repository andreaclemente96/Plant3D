using UnityEngine;

public class HeadLockedUI : MonoBehaviour
{
    public Transform targetTransform;
    public float distanceInFront = 1.5f;
    public float horizontalOffset = 0f;
    public float verticalOffset = 0f; // Ahora controla la altura relativa a tu visión

    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Posición base frente a la cámara
        Vector3 targetPosition = targetTransform.position + targetTransform.forward * distanceInFront;

        // Mueve el objeto de forma horizontal y vertical respecto a la visión de la cámara
        Vector3 rightVector = targetTransform.right * horizontalOffset;
        Vector3 upVector = targetTransform.up * verticalOffset;

        transform.position = targetPosition + rightVector + upVector;

        // Asegura que el objeto siempre mire a la cámara
        transform.rotation = Quaternion.LookRotation(transform.position - targetTransform.position);
    }
}