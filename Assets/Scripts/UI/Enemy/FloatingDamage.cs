using UnityEngine;
using TMPro;
using System.Collections;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private GameObject floatingNumbers;

    private float zoomInScale = 1.3f;
    private float zoomOutScale = 0.4f;
    private float zoomDuration = 0.12f;

    public void DamageNumber(Vector3 spawnPos, float damage)
    {
        Vector3 randomPoint = Random.insideUnitSphere * 0.35f;
        GameObject obj = Instantiate(floatingNumbers, spawnPos + randomPoint, Quaternion.identity);

        TextMeshPro tmp = obj.GetComponentInChildren<TextMeshPro>();
        tmp.text = damage.ToString();

        StartCoroutine(ZoomRoutine(obj));
        if (obj)
        {
            try
            {
                Destroy(obj, 0.3f);
            }
            catch { }
        }
    }

    IEnumerator ZoomRoutine(GameObject obj)
    {
        Transform t = obj.transform;
        Vector3 originalScale = t.localScale;

        yield return ScaleTo(originalScale * zoomInScale, zoomDuration, t);

        yield return ScaleTo(originalScale * zoomOutScale, zoomDuration, t);

        Destroy(obj, 0.3f);
    }

    IEnumerator ScaleTo(Vector3 targetScale, float duration, Transform t)
    {
        Vector3 start = t.localScale;
        float time = 0f;

        while (time < duration)
        {
            try
            {
                t.localScale = Vector3.Lerp(start, targetScale, time / duration);
                time += Time.deltaTime;
            }
            catch { }
            yield return null;
        }

        t.localScale = targetScale;
    }
}
