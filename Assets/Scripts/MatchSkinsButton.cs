

using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;

namespace Spine {
	public class MatchSkinsButton : MonoBehaviour {

		public SkeletonDataAsset skeletonDataAsset;
		public MatchSkins skinsSystem;

		[SpineSkin(dataField:"skeletonDataAsset")] public string itemSkin;
		public MatchSkins.ItemType itemType;

		void Start () {
			var button = GetComponent<Button>();
			button.onClick.AddListener(
				delegate { skinsSystem.Equip(itemSkin, itemType); }
			);
		}
	}
}
