using Cruciball;
using PeglinUI.PostBattle;
using Relics;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
    class STSPoolPopulator : MonoBehaviour {
        private List<GameObject> GetRandomOrbPool() {
            float value = UnityEngine.Random.value;
            if (value <= 0.6f) {
                return deckManager.CommonOrbPool;
            }
            if (value <= 0.9f) {
                return deckManager.UncommonOrbPool;
            }
            return deckManager.RareOrbPool;
        }

        private GameObject GetRandomOrb() {
            List<GameObject> randomOrbPool = GetRandomOrbPool();
            return randomOrbPool[UnityEngine.Random.Range(0, randomOrbPool.Count)];
        }

        private void OnEnable() {
            for (int i = 0; i < poolSize; ++i) {
                GameObject optionObject = GameObject.Instantiate(upgradeOptionPrefab, this.transform);
                GameObject orbPrefab = GetRandomOrb();

                optionObject.transform.localScale = Vector3.one;
                UpgradeOption optionComp = optionObject.GetComponent<UpgradeOption>();
                optionComp.SpecifiedOrb = orbPrefab;
                optionComp.upgradeType = UpgradeOption.UpgradeType.INSPECT_NEW_ORB;
                optionObject.GetComponent<ButtonHandleHover>().noteOffset = 10;
            }
        }

        public int poolSize;
        public DeckManager deckManager;
        public RelicManager relicManager;
        public CruciballManager cruciballManager;
        public GameObject upgradeOptionPrefab;
    }
}
