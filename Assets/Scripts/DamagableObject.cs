using UnityEngine;
using System.Collections;
using System.Linq;

/* A component that should be applied to any meteors, aliens, bonuses, etc that should be shot down from the sky.
    Takes damage and has a score */

public enum DamageType { IMPACT , EXPLOSION }

public class DamagableObject : MonoBehaviour {
    // The total health of the object
    public float maxHP = 100;

    // Is the object dead?
    public bool takenCriticalDamage = false;

    // A flag to indicate bonusness. Bonuses are not enemies, so your kill count does not rise for killing one, for example.
    public bool isBonus = false;

    // How much health does it have now
    private float currentHP;

    // A list of particle systems to play while the object is alive.
    public ParticleSystem[] particleSystemsAlive;

    // Systems to play when damage is taken,
    public ParticleSystem[] takeDamageEffect;

    // Systems to play once when critical damage is taken
    public ParticleSystem[] deathEffect;

    // The score the player gets for blowing this thing up
    public int baseScore = 100;

    // How long after this object spawns can it not be damaged or destroyed?
    public float initialInvulnerableTime = 0.3f;

    // How much bonus score to apply? Bonus score adds cash to bank.
    public float bonusScore = 500;

    // When was this object last initalized?
    private float timeInited;

    // If true, the object will implode (die) after the implodeAfter time. Useful for bonuses with a timed life.
    public bool enableAutoImplosion = false;
    public float implodeAfter = 2.0f;

    // How long after destroying this object before it is removed from the field and can be used again by pools? (Be sure to let all effects finish!)
    public float timeAfterDestructionToRemove = 2.0f;

    // When was this object destroyed?
    private float timeDestroyed;

    // Sound to play when this object dies.
    public AudioSource dieSound;

	void Start ()
    {
        Init();
	}

    // Function to run every time the object is created from a pool. Object is "refreshed"
    public void Init()
    {
        // Reset HP
        currentHP = maxHP;
        takenCriticalDamage = false;
        // Note initialization time
        timeInited = Time.time;
        // Enable the collider
        GetComponent<Collider2D>().enabled = true;

        // Restart all particle systems
        foreach (ParticleSystem system in particleSystemsAlive)
        {
            system.EnableEmission(true);
        }
    }

    // A utility for destroying the object instantly (For example, with a bomb powerup)
    public void InstaKill(bool collectScore = true)
    {
        HandleCriticalDamage(collectScore);
    }
	
	void Update ()
    {
        // Automatically implode the object if its been around for too long
        if (enableAutoImplosion && Time.time - timeInited > implodeAfter)
        {
            HandleCriticalDamage(false, true);
        }
	}

    private void HandleCriticalDamage(bool collectScore, bool byImplosion = false)
    {
        // Don't die if already dead
        if (takenCriticalDamage)
        {
            return;
        }

        // Set flag for being dead
        takenCriticalDamage = true;

        // Note destruction time
        timeDestroyed = Time.time;

        // Disable the collider to prevent further collisions
        GetComponent<Collider2D>().enabled = false;

        // Stop particle systems
        foreach (ParticleSystem system in particleSystemsAlive)
        {
            system.EnableEmission(false);
        }

        // If this death wasn't caused by a timeout...
        if (!byImplosion)
        {
            // Draw explosion animations
            foreach (ParticleSystem system in deathEffect)
            {
                system.Play();
            }

            // Shake the camera
            Camera.main.GetComponent<DefendorCamera>().ShakeMed();
            
            // Notify the game manager of this objects destruction to collect points
            DefendorGame.main.HandleDamagableObjectDestroyed(this, collectScore);
        }
        
        // Wait until effects are done and then kill
        StartCoroutine(KillAfterEffectsFinish());

        // Play death sound if enabled
        if (dieSound)
        {
            dieSound.Play();
        }
    }

    // Wait for all the effects to be finished and then set the gameobject to false
    private IEnumerator KillAfterEffectsFinish()
    {
        while (deathEffect.Any(p => p.IsAlive()) || Time.time - timeDestroyed < timeAfterDestructionToRemove)
        {
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }

    // Public utility for damaging this object
    public void ApplyDamage(float dmg, DamageType type)
    {
        // If the object is in its initial invulnerable time, do nothing
        if (Time.time - timeInited < initialInvulnerableTime)
        {
            return;
        }

        // If object is dead, do nothing
        if (takenCriticalDamage)
        {
            return;
        }

        // Play all damage effects
        foreach (ParticleSystem system in takeDamageEffect)
        {
            system.Play();
        }

        // Subtract HP
        currentHP -= dmg;
        // If health is less than 0...
        if (currentHP <= 0)
        {
            // Lock HP at minimum 0
            currentHP = 0;
            // Cause explosion
            HandleCriticalDamage(true);
        }
    }
}
