using UnityEngine;

public class HeadLockedUI : MonoBehaviour
{
    public Transform targetTransform;
    public float distanceInFront = 1.5f;
    public float horizontalOffset = 0f;
    public float verticalOffset = 0f; // Ahora controla la altura relativa a tu visi�n

    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Posici�n base frente a la c�mara
        Vector3 targetPosition = targetTransform.position + targetTransform.forward * distanceInFront;

        // Mueve el objeto de forma horizontal y vertical respecto a la visi�n de la c�mara
        Vector3 rightVector = targetTransform.right * horizontalOffset;
        Vector3 upVector = targetTransform.up * verticalOffset;

        transform.position = targetPosition + rightVector + upVector;

        // Asegura que el objeto siempre mire a la c�mara
        transform.rotation = Quaternion.LookRotation(transform.position - targetTransform.position);
    }
}