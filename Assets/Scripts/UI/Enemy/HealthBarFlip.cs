using UnityEngine;

public class HealthBarFlip : MonoBehaviour
{
    private float lastParentXScale;
    private void Update()
    {
        if (transform.parent != null)
        {
            Transform parentTransform = transform.parent;
            Vector3 parentPos = transform.parent.position;
            float currentPXScale = parentTransform.localScale.x;
            Debug.Log(currentPXScale + "," + lastParentXScale);
            if (currentPXScale != lastParentXScale)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                lastParentXScale = currentPXScale;
            }
        }
    }
}
