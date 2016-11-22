using UnityEngine;

public static class ParticleSystemExtension
{
    public static void EnableEmission(this ParticleSystem particleSystem, bool enabled)
    {
        var emission = particleSystem.emission;
        emission.enabled = enabled;
    }

    public static float GetEmissionRate(this ParticleSystem particleSystem)
    {
        return particleSystem.emission.rate.constantMax;
    }

    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        var emission = particleSystem.emission;
        var rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }

    public static void EnableEmissionAll(this ParticleSystem[] systems, bool enable = true)
    {
        foreach (ParticleSystem system in systems)
        {
            system.EnableEmission(enable);
            if (enable) system.Play();
        }
    }
}
