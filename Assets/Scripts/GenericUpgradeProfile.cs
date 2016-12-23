using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// Used to define the paramaters of a new kind of upgrade
[CreateAssetMenu(fileName = "Moon Defender Upgrade Profile", menuName = "Moon Defense/Upgrade Profile")]
public class GenericUpgradeProfile : ScriptableObject {
    public string slug;
    public float reduceShotCooldown;
    public float increaseRotationAcceleration;
    public float increaseRotationSpeed;
    public float increaseMainHitDamage;
    public float increaseSplashDamage;
    public float increaseExplosionBlastRadius;
    public float reduceIncomingProjectileAccuracy;
    public float increaseBulletSpeed;
    public float increaseBulletAccelerationPerSecond;
    public int increaseCityHP;
    public int increaseActiveBulletsAtATime;
    public bool enableAutomaticFire;
    public bool enableLaserSights;
}

