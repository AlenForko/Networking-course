using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInput;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : NetworkBehaviour, IPlayerActions
{
    private PlayerInput _playerInput;
    private Vector2 _moveInput = new();
    private Vector2 _cursorLocation;

    private Shield _shield;
    private Rigidbody2D _rb;

    private Transform turretPivotTransform;

    private NetworkVariable<bool> _isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    private SpriteRenderer _spriteRenderer;
    
    public UnityAction<bool> onFireEvent;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float shipRotationSpeed = 100f;
    [SerializeField] private float turretRotationSpeed = 4f;
    
    [Header("Sprites Settings")]
    [SerializeField] private Sprite[] movingSprites;
    [SerializeField] private Sprite stationarySprite;
    [SerializeField] private GameObject shieldGameObject;
    [SerializeField] private float spriteChangeDelay = 0.2f;
    
    private Coroutine spriteChangeCoroutine;
    private int movingSpriteIndex = 0;
    
    public override void OnNetworkSpawn()
    {
        _shield = GetComponent<Shield>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isMoving.OnValueChanged += MoveSpriteAnimation;
        _shield.hasShield.OnValueChanged += ActivateShieldSprite;
        
        shieldGameObject.SetActive(false);
        if(!IsOwner) return;

        if (_playerInput == null)
        {
            _playerInput = new();
            _playerInput.Player.SetCallbacks(this);
        }
        _playerInput.Player.Enable();

        _rb = GetComponent<Rigidbody2D>();
        turretPivotTransform = transform.Find("PivotTurret");
        if (turretPivotTransform == null) Debug.LogError("PivotTurret is not found", gameObject);
    }

    public override void OnNetworkDespawn()
    {
        _isMoving.OnValueChanged -= MoveSpriteAnimation;
        _shield.hasShield.OnValueChanged -= ActivateShieldSprite;
    }

    public void OnFire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onFireEvent.Invoke(true);
        }
        else if (context.canceled)
        {
            onFireEvent.Invoke(false);
        }
    }

    public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();  
        _isMoving.Value = _moveInput.magnitude > 0.01f;
    }

    private void FixedUpdate()
    {
        if(!IsOwner) return;
        _rb.velocity = transform.up * (_moveInput.y * movementSpeed);
        _rb.MoveRotation(_rb.rotation + _moveInput.x * -shipRotationSpeed * Time.fixedDeltaTime);
    }
    private void LateUpdate()
    {
        if(!IsOwner) return;
        Vector2 screenToWorldPosition = Camera.main.ScreenToWorldPoint(_cursorLocation);
        Vector2 targetDirection = new Vector2(screenToWorldPosition.x - turretPivotTransform.position.x, screenToWorldPosition.y - turretPivotTransform.position.y).normalized;
        Vector2 currentDirection = Vector2.Lerp(turretPivotTransform.up, targetDirection, Time.deltaTime * turretRotationSpeed);
        turretPivotTransform.up = currentDirection;
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _cursorLocation = context.ReadValue<Vector2>();
    }
    
    private IEnumerator ChangeMovingSprite()
    {
        while (_isMoving.Value)
        {
            _spriteRenderer.sprite = movingSprites[movingSpriteIndex];
            movingSpriteIndex = (movingSpriteIndex + 1) % movingSprites.Length;
            
            yield return new WaitForSeconds(spriteChangeDelay);
        }
    }

    void MoveSpriteAnimation(bool oldValue, bool newValue)
    {
        if (newValue && spriteChangeCoroutine == null) 
            spriteChangeCoroutine = StartCoroutine(ChangeMovingSprite());
        else if (spriteChangeCoroutine != null)
        {
            StopCoroutine(spriteChangeCoroutine);
            spriteChangeCoroutine = null;
            _spriteRenderer.sprite = stationarySprite;
        }
    }

    void ActivateShieldSprite(bool oldValue, bool newValue)
    {
        if (newValue)
        {
            shieldGameObject.SetActive(true);
        }
        else if (oldValue)
        {
            shieldGameObject.SetActive(false);
        }
    }
}
