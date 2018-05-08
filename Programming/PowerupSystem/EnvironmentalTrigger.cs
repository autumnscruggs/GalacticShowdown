using UnityEngine;
using System.Collections;

public class EnvironmentalTrigger : Powerup
{
    [SerializeField] private PowerupManager.TriggerTypes tType;

    public void SetUpTrigger(PowerupManager.TriggerTypes tType)
    {
        this.GetReferences();
        this.powerupActivationLife = powerupManager.pUpActivationLife;
        this.powerupActivationTimer = powerupActivationLife;
        this.powerupShelfLife = powerupManager.pUpShelfLife;
        this.powerupShelfTimer = powerupShelfLife;
        this.type = PowerupManager.PowerupTypes.NONE; //hacks
        this.tType = tType;
        this.GetPowerupValues();
        this.SpawnIcon();
        this.startShelfTimer = true;
    }

    public override void GetPowerupValues()
    {
        this.SetPowerupSpawn();

        switch (tType)
        {
            case PowerupManager.TriggerTypes.LASER_BEAM_ASSAULT:
                icon = powerupManager.powerupIcons[PowerupManager.LaserBeamAssault];
                powerupPrefab = powerupManager.powerupPrefabs[PowerupManager.LaserBeamAssault];
                break;
            case PowerupManager.TriggerTypes.METEOR_SHOWER:
                icon = powerupManager.powerupIcons[PowerupManager.MeteorShower];
                powerupPrefab = powerupManager.powerupPrefabs[PowerupManager.MeteorShower];
                break;
        }
    }

    public override void ActivatePowerup()
    {
        base.ActivatePowerup();
    }

    protected override void PowerupFunctionality()
    {
        switch (tType)
        {
            case PowerupManager.TriggerTypes.LASER_BEAM_ASSAULT:
                LaserBeamAssault laserAssault = this.gameObject.AddComponent<LaserBeamAssault>();
                laserAssault.SetUpLaserAssault(playerWhoEarnedPowerup, powerupManager.laserSpeed, powerupManager.lbaRotationTime,
                    powerupManager.lbaTopInitialRotation, powerupManager.lbaBottomInitialRotation);
                laserAssault.GetLaserSpawnPoints(powerupManager.p1LaserSpawns, powerupManager.p2LaserSpawns);
                laserAssault.SpawnLasers();
                break;
            case PowerupManager.TriggerTypes.METEOR_SHOWER:
                if(playerWhoEarnedPowerup == Player.PlayerIdentity.PLAYER_1)
                { powerupManager.p2Strike.StartMeteorStrike(); }
                else
                { powerupManager.p1Strike.StartMeteorStrike(); }
                break;
        }
    }
}
