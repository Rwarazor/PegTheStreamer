using PeglinUI;
using UnityEngine;
using UnityEngine.UI;

namespace PegTheStreamer.Behaviours {
    class STSStartManager : Singleton<STSStartManager> {
        public STSState state = STSState.DISABLED;

        private void SetupViewController(GameObject view) {
            var compOld = view.GetComponent(typeof(InventoryViewController)) as InventoryViewController;
            var compNew = view.AddComponent(typeof(STSPoolViewController)) as STSPoolViewController;
            compNew.deckManager = compOld.deckManager;
            compNew.relicManager = compOld.relicManager;
            compNew.cruciballManager = compOld.cruciballManager;
            compNew.upgradeConfirmationPanelInspect = compOld.upgradeConfirmationPanelInspect;
            compNew.orbsLeftToAdd = deckSize;
            Component.Destroy(compOld);
        }

        private void SetupGridPopulator(GameObject grid) {
            var compOld1 = grid.GetComponent(typeof(InventoryGridPopulator)) as InventoryGridPopulator;
            var compNew1 = grid.AddComponent(typeof(STSPoolPopulator)) as STSPoolPopulator;
            compNew1.deckManager = compOld1.deckManager;
            compNew1.relicManager = compOld1.relicManager;
            compNew1.cruciballManager = compOld1.cruciballManager;
            compNew1.upgradeOptionPrefab = compOld1.upgradeOptionPrefab;
            compNew1.poolSize = poolSize;
            Component.Destroy(compOld1);
        }

        private GameObject CreateWindow() {
            GameObject inventoryView = GameObject.Find("InventoryView");
            GameObject clone = GameObject.Instantiate(inventoryView);
            clone.SetActive(false);
            Component.Destroy(clone.transform.Find("Container").GetComponent(typeof(Button)));
            Component.Destroy(clone.transform.Find("Container/Frame").GetComponent(typeof(Button)));
            GameObject.Destroy(clone.transform.Find("Container/Frame/CloseButton").gameObject);

            SetupViewController(clone);
            SetupGridPopulator(clone.transform.Find("Container/Frame/TileFill/BallInventoryView/Scroll View/Viewport/OrbGrid").gameObject);

            clone.transform.Find("Container").gameObject.SetActive(true);
            return clone;
        }

        public void SetupStartingDeckChoice() {
            state = STSState.STARTED;
            var window = CreateWindow();
            window.SetActive(true);
        }

        public static int deckSize = 4;
        public static int poolSize { get { return PTSSettingsManager.STSStartPoolSize; } }

        public enum STSState {
            DISABLED,
            SHOULD_START,
            STARTED,
            FINISHED
        }
    }
}
