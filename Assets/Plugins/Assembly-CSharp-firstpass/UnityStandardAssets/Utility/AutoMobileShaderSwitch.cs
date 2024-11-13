using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
	public class AutoMobileShaderSwitch : MonoBehaviour
	{
		[Serializable]
		public class ReplacementDefinition
		{
			public Shader original = null;

			public Shader replacement = null;
		}

		[Serializable]
		public class ReplacementList
		{
			public ReplacementDefinition[] items = new ReplacementDefinition[0];
		}

		[SerializeField]
		private ReplacementList m_ReplacementList;

		private void OnEnable()
		{
		}
	}
}
