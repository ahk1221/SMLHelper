﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using WorldStreaming;
using SMLHelper.V2.Assets.Biomes;
namespace SMLHelper.V2.Patchers
{
    internal class BatchOctreesPatcher
    {
            [HarmonyPatch(typeof(BatchOctrees), nameof(BatchOctrees.LoadOctrees))]
            [PatchUtils.Prefix]
            internal static bool BatchOctrees_LoadOctrees_Prefix(BatchOctrees __instance)
            {
                var shouldContinue = false;
                Biome containingBiome = null;
                for (var e = 0; e < BiomeAssetsVariables.Biomes.Count; e++)
                {
                    var biome = BiomeAssetsVariables.Biomes[e];
                    if (biome.BatchIds.Contains(__instance.id))
                    {
                        shouldContinue = true;
                        containingBiome = biome;
                        break;
                    }
                }
                if(shouldContinue)
                {
                    var instantiatedgo = UnityEngine.GameObject.Instantiate(containingBiome.BatchRoots[__instance.id]);
                instantiatedgo.layer = LayerID.TerrainCollider;
                    LargeWorldStreamer.main.OnBatchObjectsLoaded(__instance.id, instantiatedgo);
                    return false;
                }
                return true;
            }
        internal static void Patch(Harmony h)
        {
            PatchUtils.PatchClass(h);
            Logger.Log("Patched BatchOctrees");
        }
    }
}
