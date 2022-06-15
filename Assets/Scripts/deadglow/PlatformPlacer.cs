using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlatformPlacer : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject PlatformPrefab;
	public GameObject PlatformGhostPrefab;
	
	[Header("References")]
	[Tooltip("Doesn't actually have a use yet.")]
	public Transform Player;
	[Tooltip("Which layers will the platform placement collision check consider?")]
	public LayerMask PlatformCollisionLayers = new LayerMask();
	[Tooltip("Extents of placement collision check box.")]
	public Vector2 CollisionBoxSize = Vector2.one;

	[Header("Lifetime")]
	[Tooltip("How long the platform is active after placing it. Keep it at 0 for indefinite.")]
	public float LifetimeDuration = 0;
	public float CurrentActiveTime = 0;
	public float PlacementCooldownDuration = 0;
	public float CurrentPlacementCooldownTime = 0;

	[Header("Input")]
	public KeyCode PlacementInputKey = KeyCode.F;
	public KeyCode RemovalInputKey = KeyCode.G;
	[Range(0, 4)]
	public int PlacementMouseButton = 0;

	private Transform _platform;
	private Transform _ghost;
	private Transform _platformParent;
	private bool _isPlaced = false;


	[Header("Events")]
	public UnityEvent<Vector2> OnPlaceEvent;
	public UnityEvent<Vector2> OnRemoveEvent;
	public UnityEvent<Vector2> OnInvalidPlaceEvent;

	void Awake()
	{
		_platformParent = new GameObject(gameObject.name + "'s Platform Parent").transform;
		_platformParent.position = Vector3.zero;
		
		_platform = Instantiate<GameObject>(PlatformPrefab, _platformParent).transform;
		_ghost = Instantiate<GameObject>(PlatformGhostPrefab, _platformParent).transform;

		RemovePlatform();
	}

	void Update()
	{
		// Enter placement mode
		if (Input.GetKey(PlacementInputKey))
		{
			// Set the ghost platform's position
			Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_ghost.position = point;

			// Activate the ghost if it isn't active
			if (!_ghost.gameObject.activeSelf)
				_ghost.gameObject.SetActive(true);

			// Tick down cooldown
			if (CurrentPlacementCooldownTime > 0)
				CurrentPlacementCooldownTime -= Time.deltaTime;
			else
			{
				// Try place the platform if left mouse is clicked
				if (Input.GetMouseButtonDown(PlacementMouseButton))
					TryPlacePlatform(point);
			}
		}
		else
		{
			// Deactivate the ghost if it is active
			if (_ghost.gameObject.activeSelf)
				_ghost.gameObject.SetActive(false);
		}

		if (_isPlaced)
		{
			// Remove platform
			if (Input.GetKeyDown(RemovalInputKey))
				RemovePlatform();
			else
			{
				// Increments by delta time and removes the platform if it exceeds the lifetime duration
				CurrentActiveTime += Time.deltaTime;
				if (LifetimeDuration > 0 && CurrentActiveTime > LifetimeDuration)
					RemovePlatform();
			}
		}
	}

	/// <summary>
    /// Activates the platform at the defined position
    /// </summary>
    /// <param name="point">Point in world space</param>
	public void PlacePlatform(Vector2 point)
	{
		if (_isPlaced)
			RemovePlatform();
		
		_platform.position = point;
		_platform.gameObject.SetActive(true);

		CurrentActiveTime = 0;

		_isPlaced = true;

		// Start placement cooldown
		if (PlacementCooldownDuration > 0)
			CurrentPlacementCooldownTime = PlacementCooldownDuration;

		// Platform placed!!
		OnPlaceEvent?.Invoke(point);
	}

	/// <summary>
	/// Does a collision check before placing the platform, and will do nothing if a collision is detected.
	/// </summary>
	/// <param name="point">Point in world space</param>
	/// <returns>Success of placement</returns>
	public bool TryPlacePlatform(Vector2 point)
	{
		if (Physics2D.OverlapBox(point, CollisionBoxSize, 0, PlatformCollisionLayers) == null)
		{
			PlacePlatform(point);
			return true;
		}
		else
		{
			OnInvalidPlaceEvent?.Invoke(point);

			return false;
		}
	}

	/// <summary>
    /// Deactivates an active platform
    /// </summary>
	public void RemovePlatform()
	{
		_platform.gameObject.SetActive(false);
		_isPlaced = false;
		OnRemoveEvent?.Invoke(_platform.position);
	}
}