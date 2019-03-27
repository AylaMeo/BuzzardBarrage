using CitizenFX.Core;
using static AlsekBuzzard.Misc;
using static CitizenFX.Core.Native.API;

namespace AlsekBuzzard
{
    public class Missile : BaseScript
    {
        public static async void DeployMissile(Vehicle vehicle)
        {
            // Stop if the vehicle doesn't exist or if it's dead.
            if (!vehicle.Exists() || vehicle.IsDead) return;

            // Make sure the doors are open, stop if they aren't.
            if (AlsekBuzzard.FireBarrage)
            {
                // Get the missile model.
                uint missile = 3473446624;

                // Stop if the model is invalid.
                if (!IsWeaponValid(missile)) return;

                // Load the model if it's not loaded yet.
                if (!HasWeaponAssetLoaded(missile))
                {
                    RequestWeaponAsset(missile, 31, 26);
                    while (!HasWeaponAssetLoaded(missile))
                    {
                        await BaseScript.Delay(0);
                    }
                }

                MissleShot(vehicle, missile);
                await BaseScript.Delay(4000);
                MissleShot(vehicle, missile);
            }
        }

        public static async void MissleShot(Vehicle vehicle, uint missile)
        {
            await BaseScript.Delay(0);
            
            var PlayerCoords = GetEntityCoords(PlayerPedId(), true);
            var PlayerCoordsOffsetRight = GetOffsetFromEntityInWorldCoords(PlayerPedId(), 0, 50, -3);

            ShootSingleBulletBetweenCoords(PlayerCoords.X, PlayerCoords.Y, PlayerCoords.Z, PlayerCoordsOffsetRight.X, PlayerCoordsOffsetRight.Y, PlayerCoordsOffsetRight.Z, 1, true, missile, PlayerPedId(), true, false, 0);
            
            //AlsekFlares.FlaresLeft -= 2;
            if (AlsekBuzzard.DebugMode)
            {
                //Debug.Write($"Flaresleft: {AlsekFlares.FlaresLeft}");
            }
        }
    }
}