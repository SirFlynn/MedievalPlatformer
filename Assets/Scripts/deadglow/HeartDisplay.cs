using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
	public RectTransform HeartDisplayTransform;
	public float DefaultWidth = 50;

	public void ChangeWidthFromHealth(int currentHealth)
	{
		Vector2 size = HeartDisplayTransform.sizeDelta;
		size.x = DefaultWidth * currentHealth;
		HeartDisplayTransform.sizeDelta = size;
	}
}
