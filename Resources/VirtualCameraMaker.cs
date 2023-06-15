using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class VirtualCameraMaker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateVirtualCamera()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        GameObject virtualCameraObject = new GameObject("VirtualCamera");
        CinemachineVirtualCamera virtualCamera = virtualCameraObject.AddComponent<CinemachineVirtualCamera>();
        CinemachineCollider collider = virtualCameraObject.AddComponent<CinemachineCollider>();

        collider.m_Damping = 0f;
        collider.m_MinimumDistanceFromTarget = 0.1f;
        collider.m_MaximumEffort = 0;
        collider.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
        collider.m_CollideAgainst = LayerMask.GetMask();

        virtualCamera.Follow = playerObject.transform;
        virtualCamera.LookAt = playerObject.transform;
        virtualCamera.AddCinemachineComponent<CinemachinePOV>();
        virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
    }
    public void AddCinemachineBrain()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            CinemachineBrain brain = mainCamera.GetComponent<CinemachineBrain>();

            if (brain == null)
            {
                brain = mainCamera.gameObject.AddComponent<CinemachineBrain>();

                brain.m_DefaultBlend.m_Time = 1f; // Kamera geçiþ süresini belirle
                brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut; // Kamera geçiþ stilini belirle

                Debug.Log("Cinemachine Brain added to the Main Camera.");
            }
            else
            {
                Debug.Log("Cinemachine Brain already exists on the Main Camera.");
            }
        }
        else
        {
            Debug.LogError("Main Camera not found in the scene.");
        }
    }
}
