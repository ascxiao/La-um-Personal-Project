using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private float lastParentXScale;
    private void Update()
    {
        ////FIX THISSSSSSSSSSSS DOESNT WORK
        if (transform.parent != null)
        {
            Transform parentTransform = transform.parent;
            Vector3 parentPos = transform.parent.position;
            float currentPXScale = parentTransform.localScale.x;
            if (currentPXScale != lastParentXScale)
            {
                slider.transform.localScale = new Vector3(slider.transform.localScale.x * -1, slider.transform.localScale.y, slider.transform.localScale.z);
                lastParentXScale = currentPXScale;
            }
        }
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

}
