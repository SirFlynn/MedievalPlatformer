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
	
	[Header("Lifetime")]
	[Tooltip("How long the platform is active after placing it. Keep it at 0 for indefinite.")]
	public float LifetimeDuration = 0;
	public float ActiveTime = 0;
	
	[Header("Input")]
	public KeyCode PlacementInputKey = KeyCode.F;
	public KeyCode RemovalInputKey = KeyCode.G;
	[Range(0, 4)]
	public int PlacementMouseButton = 0;

	private Transform _platform;
	private Transform _ghost;
	private Transform _platformParent;
	private PlacementState _placementState = PlacementState.Idle;

	public PlacementState CurrentPlacementState { get { return _placementState; } }

	[Header("Events")]
	public UnityEvent<Transform> OnPlaceEvent;
	public UnityEvent<Transform> OnRemoveEvent;

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
		if (_placementState == PlacementState.PlatformActive)
		{
			// Remove platform
			if (Input.GetKeyDown(RemovalInputKey))
				RemovePlatform();
			else
			{
				// Increments by delta time and removes the platform if it exceeds the lifetime duration
				ActiveTime += Time.deltaTime;
				if (LifetimeDuration > 0 && ActiveTime > LifetimeDuration)
					RemovePlatform();
			}

		}
		else
		{
			// Enter placement mode
			if (Input.GetKey(PlacementInputKey))
			{
				_placementState = PlacementState.PlacingPlatform;

				// Set the ghost platform's position
				Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				_ghost.position = point;

				// Activate the ghost if it isn't active
				if (!_ghost.gameObject.activeSelf)
					_ghost.gameObject.SetActive(true);

				// Place the platform if left mouse is clicked
				if (Input.GetMouseButtonDown(PlacementMouseButton))
					PlacePlatform(point);
			}
			else
			{
				// Deactivate the ghost if it is active
				if (_ghost.gameObject.activeSelf)
					_ghost.gameObject.SetActive(false);
			}
		}
	}

	/// <summary>
    /// Activates the platform at the defined position
    /// </summary>
    /// <param name="point">Point in world space</param>
	public void PlacePlatform(Vector2 point)
	{
		_placementState = PlacementState.PlatformActive;
		
		_platform.position = point;
		_platform.gameObject.SetActive(true);

		// Reset active time
		ActiveTime = 0;

		// Platform placed!!
		OnPlaceEvent?.Invoke(_platform);
	}

	/// <summary>
    /// Deactivates an active platform
    /// </summary>
	public void RemovePlatform()
	{
		_placementState = PlacementState.Idle;

		_platform.gameObject.SetActive(false);
		OnRemoveEvent?.Invoke(_platform);
	}

	public enum PlacementState
	{
		Idle,
		PlacingPlatform,
		PlatformActive
	}
}