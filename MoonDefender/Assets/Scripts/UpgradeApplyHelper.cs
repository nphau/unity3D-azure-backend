/* Helper class for applying upgrades to game objects at the beginning of a level when it has just been loaded */

using UnityEngine;


public class UpgradeApplyHelper  {
    public static void ApplyProfiles(GenericUpgradeProfile[] profiles, GunTurret mainGun, DefendorGame gameController, PlayerCity[] cities)
    {
        foreach (GenericUpgradeProfile profile in profiles)
        {
            ApplyProfile(profile, mainGun, gameController, cities);
        }
    }
    public static void ApplyProfile(GenericUpgradeProfile profile, GunTurret mainGun, DefendorGame gameController, PlayerCity[] cities)
    {
        mainGun.cooldownBetweenShots += profile.reduceShotCooldown;
        mainGun.rotationAcceleration += profile.increaseRotationAcceleration;
        mainGun.maxRotationSpeed += profile.increaseRotationSpeed;
        mainGun.bullet.directHitDamage += profile.increaseMainHitDamage;
        mainGun.bullet.splashDamage += profile.increaseSplashDamage;
        mainGun.bullet.explosionBlastRadius += profile.increaseExplosionBlastRadius;
        foreach (PlayerCity city in cities)
        {
            city.cityMaxHP += profile.increaseCityHP;
            city.Init();
        }

        gameController.reduceMissileAccuracyBy += profile.reduceIncomingProjectileAccuracy;

        mainGun.maxBulletsActiveAtATime += profile.increaseActiveBulletsAtATime;

        if (profile.enableAutomaticFire)
        {
            mainGun.automaticFire = true;
        }

        if (profile.enableLaserSights)
        {
            mainGun.useLaserSight = true;
        }

        mainGun.bulletSpeed += profile.increaseBulletSpeed;
        mainGun.bullet.acceleratePerSecond += profile.increaseBulletAccelerationPerSecond;
    }

    /*
    public static void Apply(DefendorGameStateKeeper keeper, GunTurret mainGun, DefendorGame gameController, PlayerCity[] cities)
    {
        if (keeper.hasLithiumHeatSinks)
        {
            mainGun.cooldownBetweenShots *= 0.6f;
        }

        mainGun.useLaserSight = keeper.hasLaserSights;

        if (keeper.hasOrmTargeting)
        {
            mainGun.rotationAcceleration *= 2.5f;
            mainGun.maxRotationSpeed *= 1.8f;
        }
        if (keeper.hasFriendshipDrive)
        {
            mainGun.bullet.directHitDamage *= 1.8f;
            mainGun.bullet.splashDamage *= 1.8f;
            mainGun.bullet.explosionBlastRadius *= 1.8f;
        }

        if (keeper.hasAdBlocker)
        {
            foreach (PlayerCity city in cities)
            {
                city.cityMaxHP *= 1.5f;
                city.Init();
            }
        }

        if (keeper.hasReaganCameo)
        {
            mainGun.maxBulletsActiveAtATime += 5;
            mainGun.automaticFire = true;
        }

        if (keeper.hasAshtray)
        {
            gameController.reduceMissileAccuracyBy = 5;
        }

        if (keeper.hasMissileTips)
        {
            mainGun.bulletSpeed *= 3;
            mainGun.bullet.acceleratePerSecond *= 2;
        }
    }
    */
}

