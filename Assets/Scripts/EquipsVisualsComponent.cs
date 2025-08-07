

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine.Unity.AttachmentTools;

namespace Spine.Unity {
	public class EquipsVisualsComponent : MonoBehaviour {

		public SkeletonAnimation skeletonAnimation;

		[SpineSkin]
		public string templateSkinName;

		Spine.Skin equipsSkin;
		Spine.Skin collectedSkin;

		public Material runtimeMaterial;
		public Texture2D runtimeAtlas;

		void Start () {
			equipsSkin = new Skin("Equips");

			// OPTIONAL: Add all the attachments from the template skin.
			var templateSkin = skeletonAnimation.Skeleton.Data.FindSkin(templateSkinName);
			if (templateSkin != null)
				equipsSkin.AddAttachments(templateSkin);

			skeletonAnimation.Skeleton.Skin = equipsSkin;
			RefreshSkeletonAttachments();
		}

		public void Equip (int slotIndex, string attachmentName, Attachment attachment) {
			equipsSkin.SetAttachment(slotIndex, attachmentName, attachment);
			skeletonAnimation.Skeleton.SetSkin(equipsSkin);
			RefreshSkeletonAttachments();
		}

		public void OptimizeSkin () {
			// 1. Collect all the attachments of all active skins.
			collectedSkin = collectedSkin ?? new Skin("Collected skin");
			collectedSkin.Clear();
			collectedSkin.AddAttachments(skeletonAnimation.Skeleton.Data.DefaultSkin);
			collectedSkin.AddAttachments(equipsSkin);

			// 2. Create a repacked skin.
			// Note: materials and textures returned by GetRepackedSkin() behave like 'new Texture2D()' and need to be destroyed
			if (runtimeMaterial)
				Destroy(runtimeMaterial);
			if (runtimeAtlas)
				Destroy(runtimeAtlas);
			var repackedSkin = collectedSkin.GetRepackedSkin("Repacked skin", skeletonAnimation.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial,
				out runtimeMaterial, out runtimeAtlas, maxAtlasSize : 1024, clearCache: false);
			collectedSkin.Clear();

			// You can optionally clear the textures cache after each ore multiple repack operations are done.
			//AtlasUtilities.ClearCache();
			//Resources.UnloadUnusedAssets();

			// 3. Use the repacked skin.
			skeletonAnimation.Skeleton.Skin = repackedSkin;
			RefreshSkeletonAttachments();
		}

		void RefreshSkeletonAttachments () {
			skeletonAnimation.Skeleton.SetSlotsToSetupPose();
			skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); //skeletonAnimation.Update(0);
		}

	}

}
