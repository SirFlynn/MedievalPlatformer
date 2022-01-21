using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStatus : MonoBehaviour
{
	public int CurrentHealth { get; private set; }
	public int MaxHealth = 5;

	public UnityEvent<int> OnHealthChanged;

	void Start()
	{
		SetHealth(MaxHealth);
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyDown(KeyCode.K))
				InflictDamage(1);
			if (Input.GetKeyDown(KeyCode.J))
				HealDamage(1);
		}
	}


	public void InflictDamage(int value)
	{
		SetHealth(Mathf.Max(CurrentHealth - value, 0));
	}

	public void HealDamage(int value)
	{
		SetHealth(Mathf.Min(CurrentHealth + value, MaxHealth));
	}

	public void SetHealth(int value)
	{
		CurrentHealth = value;
		OnHealthChanged?.Invoke(CurrentHealth);
	}
}
