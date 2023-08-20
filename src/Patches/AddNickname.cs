using Battle.Enemies;
using HarmonyLib;
using PegTheStreamer.Behaviours;
using UnityEngine;

namespace PegTheStreamer.Patches {
    [HarmonyPatch(typeof(Enemy))]
    class AddNicknamePatch {
        public static bool IsBoss(Enemy.EnemyType type) {
            return (type & (Enemy.EnemyType.Boss | Enemy.EnemyType.Minion)) == Enemy.EnemyType.Boss;
        }

        [HarmonyPatch("Initialize")]
        public static void Postfix(Enemy __instance) {
            if (IsBoss(__instance.enemyTypes) && !__instance.name.Contains("Tree")) {
                __instance.HealthBarObj.AddComponent(typeof(BossTwitchNickname));
            } else {
                __instance.HealthBarObj.AddComponent(typeof(EnemyTwitchNickname));
            }
        }
    }
}
