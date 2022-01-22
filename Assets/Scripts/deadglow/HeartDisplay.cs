using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
	public RectTransform HeartDisplayTransform;
	[Tooltip("Width of the image when one heart is visible. This needs to be 1:1 with the width of the sprite.")]
	public float DefaultWidth = 50;

	
	public void ChangeWidthFromHealth(int currentHealth)
	{
		Vector2 size = HeartDisplayTransform.sizeDelta;
		size.x = DefaultWidth * currentHealth;
		HeartDisplayTransform.sizeDelta = size;
	}
}
