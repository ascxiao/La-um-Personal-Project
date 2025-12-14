using UnityEngine;

using System;

public class CombatManager : MonoBehaviour
{

    private System.Random rng = new System.Random();
    public float DamageCalculator(float atk, float def, float critR, float critD, float luck, float atkPower)
    {
        float damage = (float)Math.Round(((Math.Pow(atk, 2) / def) - def) * atkPower + (rng.Next(-1, 5) * 0.10f));

        if (damage < 0)
        {
            damage = 1;
        }

        if (rng.Next(1, 100) <= critR || rng.Next(1, 100) <= luck)
        {
            damage *= critD / 100;
            damage = (float)Math.Round(damage);
        }

        return damage;
    }
}
