using UnityEngine;
using System;
using TMPro;

public class StarsHUD : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField] private TMP_Text tmpText;


	[Header("Formatting")]
	[SerializeField] private string prefix = ""; // e.g. "⭐ "
	[SerializeField] private string suffix = "";

	void Awake()
	{
		if (tmpText == null)
			tmpText = GetComponentInChildren<TMP_Text>();
	}

	void OnEnable()
	{
		UpdateView(InitScript.Instance != null ? InitScript.Instance.GetStars() : PlayerPrefs.GetInt("Stars", 0));
		InitScript.OnStarsChanged += OnStarsChanged;
	}

	void OnDisable()
	{
		InitScript.OnStarsChanged -= OnStarsChanged;
	}

	private void OnStarsChanged(int stars)
	{
		UpdateView(stars);
	}

	private void UpdateView(int stars)
	{
		string text = prefix + stars.ToString() + suffix;
		if (tmpText != null)
			tmpText.text = text;
	}
}


