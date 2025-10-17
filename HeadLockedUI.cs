using UnityEngine;

public class HeadLockedUI : MonoBehaviour
{
    public Transform targetTransform;
    public float distanceInFront = 1.5f;
    public float horizontalOffset = 0f;
    public float verticalOffset = 0f; 

    void Start()
    {
        if (targetTransform == null)
        {
            targetTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        Vector3 targetPosition = targetTransform.position + targetTransform.forward * distanceInFront;

    
        Vector3 rightVector = targetTransform.right * horizontalOffset;
        Vector3 upVector = targetTransform.up * verticalOffset;

        transform.position = targetPosition + rightVector + upVector;

        transform.rotation = Quaternion.LookRotation(transform.position - targetTransform.position);
    }

}
