using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStatus : MonoBehaviour
{
	[SerializeField]
	private int _currentHealth = 5;
	public int CurrentHealth
	{
		get { return _currentHealth; }
		private set { _currentHealth = value; }
	}
	public int MaxHealth = 5;

	[Tooltip("Called when a change in health is detected.")]
	public UnityEvent<int> OnHealthChanged;

	void Start()
	{
		SetHealth(MaxHealth);
	}

	void Update()
	{
		// Debug code, get rid of this!
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyDown(KeyCode.K))
				InflictDamage(1);
			if (Input.GetKeyDown(KeyCode.J))
				HealDamage(1);
		}
	}

	/// <summary>
	/// Removes value from health without going below 0
	/// </summary>
	public void InflictDamage(int value)
	{
		SetHealth(Mathf.Max(CurrentHealth - value, 0));
	}

	/// <summary>
	/// Adds value to health without going above MaxHealth
	/// </summary>
	public void HealDamage(int value)
	{
		SetHealth(Mathf.Min(CurrentHealth + value, MaxHealth));
	}

	/// <summary>
	/// Sets current health and triggers the UnityEvent if a change in health is detected
	/// </summary>
	public void SetHealth(int value)
	{
		if (CurrentHealth != value)
		{
			CurrentHealth = value;
			OnHealthChanged?.Invoke(CurrentHealth);
		}
	}

	/// <summary>
	/// Returns a 0-1 normalized representation of the player's health
	/// </summary>
	public float GetHealthPercentage()
	{
		return (float)CurrentHealth / (float)MaxHealth;
	}

	void OnValidate()
	{
		SetHealth(_currentHealth);
	}
}
