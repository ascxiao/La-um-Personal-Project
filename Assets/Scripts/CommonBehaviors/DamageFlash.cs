using UnityEngine;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;
    private Coroutine damageFlashCoroutine;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }

    // private void Update()
    // {
    //     DebugCheck();
    // }

    private void Init()
    {
        materials = new Material[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }

    public void CallDamageFlash()
    {
        damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    private void SetFlashColor()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_FlashColor", flashColor);
        }
    }

    private void SetFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashAmount", amount);
        }
    }

    private void DebugCheck()
    {
        // Check spriteRenderers
        if (spriteRenderers == null)
            Debug.LogError("[NullDebugger] spriteRenderers ARRAY is NULL");
        else if (spriteRenderers.Length == 0)
            Debug.LogWarning("[NullDebugger] spriteRenderers ARRAY is EMPTY");

        // Check materials
        if (materials == null)
            Debug.LogError("[NullDebugger] materials ARRAY is NULL");
        else if (materials.Length == 0)
            Debug.LogWarning("[NullDebugger] materials ARRAY is EMPTY");

        // Check damageFlashCoroutine
        if (damageFlashCoroutine == null)
            Debug.Log("[NullDebugger] damageFlashCoroutine is NULL (this is normal until coroutine runs)");
        else
            Debug.Log("[NullDebugger] damageFlashCoroutine exists!");
    }
}
