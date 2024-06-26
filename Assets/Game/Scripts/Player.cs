using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Singleton;

public class Player : Singleton<Player>
{
    // Input fields
    private ThirdPersonActionsAsset _playerActionsAsset;
    private InputAction _move;

    // Movement fields
    private Rigidbody _rb;
    private Animator _animator;
    [SerializeField] float _maxSpeed = 8f;
    [SerializeField] private float _movementForce = 2f;
    [SerializeField] private Vector3 _forceDirection = Vector3.zero;
    [SerializeField] private float _lookAtSpeed = 4f;

    // Puzzle fields
    private Interactor _interactor;
    private bool _canInteract = true;
    private bool _isHoldingItem;

    // Posi��es para sementes e �rg�os
    [SerializeField] private Transform _orgaoTransformPoint;
    [SerializeField] private Transform _sementeTransformPoint;
    // Regador do player
    [SerializeField] private GameObject _thisRegador;
    public GameObject _itemHeld { get; private set; }

    private new void Awake()
    {
        _rb = this.GetComponent<Rigidbody>();

        _animator = this.GetComponentInChildren<Animator>();
        _interactor = this.GetComponent<Interactor>();
        _playerActionsAsset = new ThirdPersonActionsAsset();

        _move = _playerActionsAsset.Player.Move;
    }

    private void OnEnable()
    {
        EnableMovementInteractionInputs();
    }

    private void OnDisable()
    {
        DisableMovementInteractionInputs();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    #region Input

    public void EnableMovementInteractionInputs()
    {
        _playerActionsAsset.Player.Interact.started += DoInteractControl;

        _interactor.ControlActivationInteractor(true);

        _playerActionsAsset.Player.Enable();
    }

    public void DisableMovementInteractionInputs()
    {
        _playerActionsAsset.Player.Interact.started -= DoInteractControl;

        _interactor.ControlActivationInteractor(false);

        _playerActionsAsset.Player.Disable();
    }

    #endregion

    #region Movement
    private void Movement()
    {
        if (_move.ReadValue<Vector2>().x == 0 && _move.ReadValue<Vector2>().y == 0)
        {
            ControlMovementAnimation(false);
        }
        else
        {
            ControlMovementAnimation(true);
        }

        _forceDirection += _move.ReadValue<Vector2>().x * Vector3.right * _movementForce;
        _forceDirection += _move.ReadValue<Vector2>().y * Vector3.forward * _movementForce;

        if (_forceDirection == Vector3.zero)
        {
            _rb.velocity *= 800f / 1000f;
        }

        _rb.AddForce(_forceDirection, ForceMode.Impulse);
        _forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = _rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > _maxSpeed * _maxSpeed)
        {
            _rb.velocity = horizontalVelocity.normalized * _maxSpeed + Vector3.up * _rb.velocity.y;
        }

        RotateWithMovement();
    }

    public IEnumerator LookAtObject(Transform obj)
    {
        DisableMovementInteractionInputs();

        Vector3 relativePos = obj.transform.position - transform.position;
        relativePos.y = 0;

        Quaternion rot = Quaternion.LookRotation(relativePos, Vector3.up);

        var deltaAngle = Quaternion.Angle(this._rb.rotation, rot);

        while (deltaAngle != 0)
        {
            deltaAngle = Quaternion.Angle(this._rb.rotation, rot);
            this._rb.rotation = Quaternion.Slerp(transform.rotation, rot, _lookAtSpeed * Time.deltaTime);
            yield return null;
        }

        EnableMovementInteractionInputs();

    }

    #endregion

    #region Camera

    private void RotateWithMovement()
    {
        Vector3 direction = _rb.velocity;
        direction.y = 0;

        if (_move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this._rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            _rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    #endregion

    #region Animation

    private void ControlMovementAnimation(bool state)
    {
        _animator.SetBool("Walking", state);
    }

    #endregion

    #region Interaction
    private void DoInteractControl(InputAction.CallbackContext obj)
    {
        if (_canInteract)
        {
            _interactor.InteractControl();
            StartCoroutine(InteractCooldown());
            _canInteract = false;
        }
    }

    private IEnumerator InteractCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        _canInteract = true;
    }

    #endregion

    #region Control of held items
    // Player started or stopped holding item
    public void HoldItemControl(bool state, GameObject item=null)
    {
        if (state)
        {
            Debug.Log("Player is holding " + item.name + ".");
        }
        else
        {
            Debug.Log("Player stopped holding " + _itemHeld.name + ".");
        }
        _itemHeld = item;
        _isHoldingItem = state;
        _animator.SetLayerWeight(1, (state ? 1 : 0));
    }

    public void ControlRegador(bool state)
    {
        _thisRegador.SetActive(state);
        HoldItemControl(state, (state ? _thisRegador : null));
    }

    public void ControlSemente(bool state, GameObject semente=null)
    {
        if (state)
        {
            semente.transform.position = _sementeTransformPoint.position;
            semente.transform.SetParent(this.gameObject.transform);
        }
        else
        {
            Destroy(_itemHeld);
        }
        HoldItemControl(state, (state ? semente : null));
    }
    public void ControlOrgao(bool state, GameObject orgao = null)
    {
        if (state)
        {
            orgao.transform.position = _orgaoTransformPoint.position;
            orgao.transform.SetParent(this.gameObject.transform);
        }
        else
        {
            Destroy(_itemHeld);
        }
        HoldItemControl(state, (state ? orgao : null));
    }

    #endregion

    #region Get
    public bool GetIsHoldingItem()
    {
        return _isHoldingItem;
    }

    #endregion
}