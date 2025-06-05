using UnityEngine;

public class particulesLauncher : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ParticleSystem Particles;

    public void startParticles()
    {
        Debug.Log("startParticles");
        Particles.Play();
    }
}
