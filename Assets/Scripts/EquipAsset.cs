
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity {
	public class EquipAsset : ScriptableObject {
		public EquipSystem.EquipType equipType;
		public Sprite sprite;
		public string description;
		public int yourStats;
	}
}
