using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.Native;

namespace AlsekBuzzard
{
    public class AlsekBuzzard : BaseScript
    {
        private static int playerVehicleInt;
        private static Vehicle playerVehicle;
        private static bool firstTick = true;
        public static int MissileLeft = 30;
        public static bool DebugMode = false;
        public static bool FireBarrage = false;
        
        public AlsekBuzzard()
        {
            API.RegisterCommand("BuzzDebug", new Action<int, List<object>, string>((source, arguments, raw) =>
            {
                if (DebugMode)
                {
                    DebugMode = false;
                }
                else
                {
                    DebugMode = true;
                }
            }), false);

            Tick += BuzzMainTask;
            //Tick += MissileInventory;
        }
        
        private static async Task MissileInventory()
        {
            await BaseScript.Delay(10000);

        
            if (MissileLeft > 29)
            {
                return;
            }
            else
            {
                MissileLeft += 1;

                if (DebugMode)
                {
                    Debug.Write($"BombsLeft: {MissileLeft}");
                }
            }
        }
        
        private static async Task BuzzMainTask()
        {
            if (firstTick)
            {
                firstTick = false;

                if (GetResourceMetadata(GetCurrentResourceName(), "client_debug_mode", 0).ToLower() == "true")
                {
                    DebugMode = true;
                }
                else
                {
                    DebugMode = false;
                }
            }

            if (Game.PlayerPed.IsInVehicle())
            {
                playerVehicle = Game.PlayerPed.CurrentVehicle;
                if (playerVehicle != null && playerVehicle.Exists())
                {
                    if (Misc.Buzzards.ContainsKey((uint)playerVehicle.Model.Hash))
                    {
                        int timer = GetGameTimer();
                        bool toggle = false;
                        while (Game.IsControlPressed(0, (Control)355)) // INPUT_VEH_FLY_BOMB_BAY (not in the api set so have to cast it to Control manually)
                        {
                            if (GetGameTimer() - timer > 500)
                            {
                                toggle = true;
                                break;
                            }
                            await Delay(0);
                        }
                        if (toggle)
                        {
                            playerVehicleInt = GetPlayersLastVehicle();
                            if (FireBarrage)
                            {
                                FireBarrage = false;
                            }
                            else
                            {
                                FireBarrage = true;
                            }
                        }
                        while (Game.IsControlPressed(0, (Control)355)) // INPUT_VEH_FLY_BOMB_BAY (not in the api set so have to cast it to Control manually)
                        {
                            await Delay(0);
                            Game.DisableControlThisFrame(0, Control.VehicleFlyAttack); // disable vehicle weapons from being fired. (114)
                        }
                        if (FireBarrage)
                        {
                            int timerFire = GetGameTimer();
                            bool toggleFire = false;
                            
                            Game.DisableControlThisFrame(0, Control.VehicleFlyAttack); // disable vehicle weapons from being fired. (114)
                            while (Game.IsDisabledControlPressed(0, Control.VehicleFlyAttack)) // INPUT_VEH_FLY_BOMB_BAY (not in the api set so have to cast it to Control manually)
                            {
                                Game.DisableControlThisFrame(0, Control.VehicleFlyAttack); // disable vehicle weapons from being fired. (114)
                                if (GetGameTimer() - timerFire > 500)
                                {
                                    toggleFire = true;
                                    break;
                                }
                                await Delay(0);
                            }
                            if (toggleFire)
                            {
                                Missile.DeployMissile(playerVehicle);
                            }
                        }
                        else // bomb bay doors are closed
                        {
                        }
                    }
                }
            }
        }
    }
}