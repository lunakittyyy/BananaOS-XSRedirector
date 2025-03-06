using System;
using HarmonyLib;

namespace BananaOS_XSRedirector.Patches
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class BananaPatch
    {
        private static void Postfix(GorillaLocomotion.Player __instance)
        {
            Console.WriteLine(__instance.maxJumpSpeed);
        }
    }
}