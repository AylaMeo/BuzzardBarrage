using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlsekBuzzard
{
    public class Misc : BaseScript
    {
        public struct Buzzard { public uint HeliHash; public float MissilePositionOffset; }

        public static Dictionary<uint, Buzzard> Buzzards = new Dictionary<uint, Buzzard>()
        {
            [(uint)GetHashKey("buzzard")] = new Buzzard()
            {
                HeliHash = (uint)GetHashKey("buzzard"),
                MissilePositionOffset = 0.46f
            }
        };
    }
}