using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private TeleportationProvider provider;
    private InputAction thumbstick;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        rayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI LeftHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;
       
        thumbstick = actionAsset.FindActionMap("XRI LeftHand").FindAction("Move");
        thumbstick.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (thumbstick.triggered) return;
        if (!rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) {
            rayInteractor.enabled = false;
            isActive = false;
        }

        TeleportRequest request = new TeleportRequest() {
            destinationPosition = hit.point,
        };
        provider.QueueTeleportRequest(request);
        rayInteractor.enabled = false;
        isActive = false;
    }

    void OnTeleportActivate(InputAction.CallbackContext context){
        rayInteractor.enabled = true;
        isActive = true;
    }

    void OnTeleportCancel(InputAction.CallbackContext context){
        rayInteractor.enabled = false;
        isActive = false;
    }
}
