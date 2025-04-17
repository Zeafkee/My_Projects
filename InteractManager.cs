using System;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    public static InteractManager Instance;
    [Tooltip("The length of the ray that will be casted from the camera to the world")]
    [SerializeField]
    float rayLength = 5f;

    [Tooltip("If true, the ray will be drawn in the scene view")]
    [SerializeField]
    bool DrawRay = true;

    [Tooltip("The color of the ray that will be drawn in the scene view")]
    [SerializeField]
    Color rayColor = Color.yellow;

    [Tooltip("The layer that the ray will target")]
    [SerializeField]
    LayerMask InteractableLayer;

    private IInteractable currentIInteractable;

    [SerializeField] protected Camera povCamera;
    RaycastHit CurrentHit;
    RaycastHit lastValidHit;
    Collider lastHitCollider;
    Ray MainRay { get; set; }

    private Player _player;
    #region Events
    public event Action<IInteractable> OnInteractBegin;
    public event Action<IInteractable> OnInteractOver;
    #endregion
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        _player = MapManager.Instance.player;
    }
    void Update()
    {
        CastRay();
        if (DrawRay)
            Debug.DrawRay(MainRay.origin, MainRay.direction * rayLength, rayColor);
    }

    void CastRay()
    {
        if (_player.stateMachine.currentState != _player.moveState) { return; }

        MainRay = povCamera.ScreenPointToRay(Input.mousePosition);

        //Raycast
        if (Physics.Raycast(MainRay, out CurrentHit, rayLength, InteractableLayer.value))
        {
            if (CurrentHit.collider.Equals(lastHitCollider))
            {
                return;
            }
            if (!HandleObservation())
                return;

            lastValidHit = CurrentHit;
            lastHitCollider = CurrentHit.collider;
        }
        else if (currentIInteractable != null)
        {
            OnInteractOver?.Invoke(currentIInteractable);
            currentIInteractable = default;
            lastHitCollider = null;
        }
    }
    bool HandleObservation()
    {
        if (CurrentHit.collider.gameObject.TryGetComponent(out IInteractable target))
        {
            currentIInteractable = target;
            lastHitCollider = CurrentHit.collider;
            OnInteractBegin?.Invoke(target);
        }
        else
            Debug.Log("Object has no IObservationListener but it is in the target layer");

        return true;
    }
}
