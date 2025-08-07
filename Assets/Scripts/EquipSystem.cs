

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine.Unity.AttachmentTools;

namespace Spine.Unity {
	public class EquipSystem : MonoBehaviour, IHasSkeletonDataAsset {

		// Implementing IHasSkeletonDataAsset allows Spine attribute drawers to automatically detect this component as a skeleton data source.
		public SkeletonDataAsset skeletonDataAsset;
		SkeletonDataAsset IHasSkeletonDataAsset.SkeletonDataAsset { get { return this.skeletonDataAsset; } }

		public Material sourceMaterial;
		public bool applyPMA = true;
		public List<EquipHook> equippables = new List<EquipHook>();

		public EquipsVisualsComponent target;
		public Dictionary<EquipAsset, Attachment> cachedAttachments = new Dictionary<EquipAsset, Attachment>();

		[System.Serializable]
		public class EquipHook {
			public EquipType type;
			[SpineSlot]
			public string slot;
			[SpineSkin]
			public string templateSkin;
			[SpineAttachment(skinField:"templateSkin")]
			public string templateAttachment;
		}

		public enum EquipType {
			Gun,
			Goggles
		}

		public void Equip (EquipAsset asset) {
			var equipType = asset.equipType;
			EquipHook howToEquip = equippables.Find(x => x.type == equipType);

			var skeletonData = skeletonDataAsset.GetSkeletonData(true);
			int slotIndex = skeletonData.FindSlotIndex(howToEquip.slot);
			var attachment = GenerateAttachmentFromEquipAsset(asset, slotIndex, howToEquip.templateSkin, howToEquip.templateAttachment);
			target.Equip(slotIndex, howToEquip.templateAttachment, attachment);
		}

		Attachment GenerateAttachmentFromEquipAsset (EquipAsset asset, int slotIndex, string templateSkinName, string templateAttachmentName) {
			Attachment attachment;
			cachedAttachments.TryGetValue(asset, out attachment);

			if (attachment == null) {
				var skeletonData = skeletonDataAsset.GetSkeletonData(true);
				var templateSkin = skeletonData.FindSkin(templateSkinName);
				Attachment templateAttachment = templateSkin.GetAttachment(slotIndex, templateAttachmentName);
				attachment = templateAttachment.GetRemappedClone(asset.sprite, sourceMaterial, premultiplyAlpha: this.applyPMA);
				// Note: Each call to `GetRemappedClone()` with parameter `premultiplyAlpha` set to `true` creates
				// a cached Texture copy which can be cleared by calling AtlasUtilities.ClearCache() as shown in the method below.

				cachedAttachments.Add(asset, attachment); // Cache this value for next time this asset is used.
			}

			return attachment;
		}

		public void Done () {
			target.OptimizeSkin();
			// `GetRepackedSkin()` and each call to `GetRemappedClone()` with parameter `premultiplyAlpha` set to `true`
			// creates cached Texture copies which can be cleared by calling AtlasUtilities.ClearCache().
			// You can optionally clear the textures cache after multiple repack operations.
			// Just be aware that while this cleanup frees up memory, it is also a costly operation
			// and will likely cause a spike in the framerate.

			//AtlasUtilities.ClearCache();
			//Resources.UnloadUnusedAssets();
		}

	}

}
