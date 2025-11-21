using UnityEngine;

public class MovementTrailParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] fx;
    private ParticleSystem currentParticle;
    public PlayerMovement playerMovement;
    public static MovementTrailParticleManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    public void WalkTrail()
    {
        fx[0].Play();
        currentParticle = fx[0];
    }

    public void RunTrail()
    {
        if (playerMovement.movement.x != 0 && playerMovement.movement.y == 0)
        {
            fx[1].Play();
            currentParticle = fx[1];
        }
        else if (playerMovement.movement.x != 0 && playerMovement.movement.y > 0)
        {
            fx[2].Play();
            currentParticle = fx[2];
        }
        else if (playerMovement.movement.x != 0 && playerMovement.movement.y < 0)
        {
            fx[3].Play();
            currentParticle = fx[3];
        }
    }

    public void StopTrail()
    {
        currentParticle.Stop();
    }
}
