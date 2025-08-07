
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace Spine.Unity {
	public class EquipButton : MonoBehaviour {
		public EquipAsset asset;
		public EquipSystem equipSystem;
		public Image inventoryImage;

		void OnValidate () {
			MatchImage();
		}

		void MatchImage () {
			if (inventoryImage != null)
				inventoryImage.sprite = asset.sprite;
		}

		void Start () {
			MatchImage();

			var button = GetComponent<Button>();
			button.onClick.AddListener(
				delegate { equipSystem.Equip(asset); }
			);
		}
	}
}
