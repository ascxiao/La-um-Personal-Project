using UnityEngine;

[CreateAssetMenu(menuName = "Stats/CharacterStats")]
public class StatsSO : ScriptableObject
{
    public float maxHealth;
    public float baseAtk;
    public float baseDef;
    public float baseSpeed;
    public float baseCritR;
    public float baseCritD;
    public float baseLuck;
}
