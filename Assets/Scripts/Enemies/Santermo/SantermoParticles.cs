using UnityEngine;

public class SantermoParticles : MonoBehaviour
{
    [SerializeField] private GameObject gb;
    private ParticleSystem burstFlames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        burstFlames = gb.GetComponent<ParticleSystem>();
    }
    public void PlayFlames()
    {
        burstFlames.Play();
    }

    public void Stop()
    {
        burstFlames.Stop();
    }

}
