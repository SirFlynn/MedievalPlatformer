using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControllerTest : MonoBehaviour
{
	public ParticleSystem Particles;

	void Awake()
	{
		Particles = GetComponent<ParticleSystem>();
	}

	public void Play(Vector2 point)
	{
		transform.position = point;
		Particles.Play();
	}
}
