using Cruciball;
using PeglinUI.PostBattle;
using Relics;
using System;
using TMPro;
using UnityEngine;

namespace PegTheStreamer.Behaviours {
    class STSPoolViewController : MonoBehaviour {
		private void OnEnable() {
			isOpen = true;
			UpgradeOption.OnUpgradeOptionClicked = (UpgradeOption.UpgradeOptionClicked)Delegate.Combine(UpgradeOption.OnUpgradeOptionClicked, new UpgradeOption.UpgradeOptionClicked(this.HandleUpgradeOptionClicked));
			upgradeConfirmationPanelInspect.Init(this.deckManager, this.relicManager, this.cruciballManager);
			UpdateText();
		}

		private void OnDisable() {
			isOpen = false;
			UpgradeOption.OnUpgradeOptionClicked = (UpgradeOption.UpgradeOptionClicked)Delegate.Remove(UpgradeOption.OnUpgradeOptionClicked, new UpgradeOption.UpgradeOptionClicked(this.HandleUpgradeOptionClicked));
		}

		private void HandleUpgradeOptionClicked(UpgradeOption.UpgradeType type, GameObject clickedButton = null, GameObject chosen = null) {
			if (isOpen && type == UpgradeOption.UpgradeType.INSPECT_NEW_ORB) {
				chosenOption = clickedButton;
				upgradeConfirmationPanelInspect.gameObject.SetActive(true);
				upgradeConfirmationPanelInspect.PopulateForNewOrb(chosen, 0);
				return;
			} else if (type == UpgradeOption.UpgradeType.NEW_ORB_CONFIRMED) {
				upgradeConfirmationPanelInspect.gameObject.SetActive(false);
				GameObject.Destroy(chosenOption);
				deckManager.AddOrbToDeck(chosen);
				orbsLeftToAdd--;
				UpdateText();
				if (orbsLeftToAdd == 0) {
					FinishChoosingStartingDeck();
				}
			}
		}

		private void FinishChoosingStartingDeck() {
			this.gameObject.SetActive(false);
			GameObject.FindWithTag("GameController").GetComponent(typeof(BattleController)).SendMessage("DestroyCurrentBall");
			deckManager.ShuffleCompleteDeck(true);
			STSStartManager.Instance.state = STSStartManager.STSState.FINISHED;
		}

		private void UpdateText() {
			if (chooseText == null) {
				SetupText();
			}
			chooseText.text = $"Choose your deck! {orbsLeftToAdd} orbs left";
		}

		private void SetupText() {
			TMP_Text text = Utils.CreateText(this.transform, "placeholder choose orbs text", new Vector3(0, 200, 0));
			chooseText = text;
			text.horizontalAlignment = TMPro.HorizontalAlignmentOptions.Center;
			text.fontSize = 45f;
			text.maxVisibleLines = 1;
			text.overflowMode = TMPro.TextOverflowModes.Truncate;

			Vector2 newSizeDelta = (text.gameObject.transform as RectTransform).sizeDelta;
			newSizeDelta.x = (this.transform as RectTransform).sizeDelta.x - 0.2f;
			(text.gameObject.transform as RectTransform).sizeDelta = newSizeDelta;
		}

		private bool isOpen = false;
		private GameObject chosenOption = null;

		public int orbsLeftToAdd;
		public DeckManager deckManager;
		public CruciballManager cruciballManager;
		public RelicManager relicManager;
		public UpgradeConfirmationOrbInspect upgradeConfirmationPanelInspect;
		public TMP_Text chooseText;
	}
}
