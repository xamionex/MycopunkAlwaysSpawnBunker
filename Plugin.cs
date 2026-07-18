using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MycopunkAlwaysSpawnBunker
{
    [BepInPlugin(PluginInfo.PluginGuid, PluginInfo.PluginName, PluginInfo.PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = Logger;

            var harmony = new Harmony(PluginInfo.PluginGuid);
            harmony.PatchAll();

            Log.LogInfo($"{PluginInfo.PluginName} v{PluginInfo.PluginVersion} loaded - Bunker encounter will always spawn.");
        }
    }

    // How the game actually decides encounters (from decompiling Assembly-CSharp):
    //
    //   BunkerEncounter.CanSpawn()          -> already hardcoded "return true;"
    //   BunkerEncounter.CanSpawnRandomly()  -> inherited default, also hardcoded "return true;"
    //   IEncounter.GatherEncounters()       -> builds a candidate list of every encounter that
    //                                          passes the two checks above, then does a WEIGHTED
    //                                          RANDOM DRAW WITHOUT REPLACEMENT using each
    //                                          candidate's SpawnWeight, capped at a handful of
    //                                          slots per generated level.
    //
    // So the Bunker is already always *eligible* - it just has to win a lottery against every
    // other encounter type each time a level is generated. Patching CanSpawn is a no-op safety
    // net; the actual fix is making SpawnWeight so large that it wins that lottery basically
    // every time.

    [HarmonyPatch(typeof(BunkerEncounter), nameof(BunkerEncounter.CanSpawn))]
    internal static class BunkerEncounter_CanSpawn_Patch
    {
        // Defensive only - this already always returns true in vanilla. Kept in case a future
        // patch adds real gating logic here.
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(BunkerEncounter), "SpawnWeight", MethodType.Getter)]
    internal static class BunkerEncounter_SpawnWeight_Patch
    {
        // Every other encounter's weight is a small hand-tuned float (tens to low hundreds at
        // most). Returning something this much larger means the Bunker overwhelmingly wins the
        // weighted draw the moment it's in the candidate pool, for as long as at least one
        // random encounter slot is available on the level - which is always the case.
        private const float ForcedSpawnWeight = 999999f;

        private static void Postfix(ref float __result)
        {
            __result = ForcedSpawnWeight;
        }
    }
}