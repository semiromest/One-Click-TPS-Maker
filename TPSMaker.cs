using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEditor;
using System.IO;
//using Cinemachine;

public class TPSMaker : EditorWindow
{
    //VirtualCameraMaker virtualCamera;

    private string packageName = "com.unity.cinemachine"; 

    private AddRequest addRequest; 

    public Object targetObject; 

    [MenuItem("Tools/TPSMaker")]
    public static void ShowWindow()
    {
        GetWindow<TPSMaker>("TPSMaker");
    }

    private void OnGUI()
    {
        GUILayout.Label("TPSMaker", EditorStyles.boldLabel);

        GUILayout.Space(10f);

        GUILayout.Label("Make TPS", EditorStyles.boldLabel);

        targetObject = EditorGUILayout.ObjectField("Target Object", targetObject, typeof(Object), true);

        if (GUILayout.Button("Make TPS"))
        {
            if (targetObject != null)
            {
                AddScriptToObject(targetObject);
                FindVirtualCameraMaker();
                InstallPackage();

                //AddCinemachineBrain();
                //CreateVirtualCamera();
            }
            else
            {
                Debug.LogError("Target object is null!");
            }
        }
    }

    private void InstallPackage()
    {
        addRequest = Client.Add(packageName); 
        EditorApplication.update += HandleInstallationProgress; // 
    }

    private void HandleInstallationProgress()
    {
        if (addRequest.IsCompleted)
        {
            if (addRequest.Status == StatusCode.Success)
            {
                Debug.Log("Package installed successfully!");
            }
            else if (addRequest.Status >= StatusCode.Failure) // Hata oluþtu ise
            {
                Debug.LogError($"Failed to install package: {addRequest.Error.message}");
            }

            EditorApplication.update -= HandleInstallationProgress; 
        }
    }

    private void AddScriptToObject(Object targetObject)
    {
        AddCharacterController();
        string scriptPath = "Assets/Resources/PlayerScript.cs";

        MonoScript scriptToAdd = UnityEditor.AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);

        if (scriptToAdd == null)
        {
            Debug.LogError("Script not found at the specified path: " + scriptPath);
            return;
        }

        string scriptName = Path.GetFileNameWithoutExtension(scriptPath);

        GameObject targetGameObject = targetObject as GameObject;

        if (targetGameObject != null)
        {
            Component existingComponent = targetGameObject.GetComponent(scriptName);

            if (existingComponent != null)
            {
                Debug.LogWarning("The script is already attached to the target object!");
            }
            else
            {
                System.Type scriptType = scriptToAdd.GetClass();
                if (scriptType != null)
                {
                    targetGameObject.AddComponent(scriptType);
                    Debug.Log("Script added to the target object!");
                }
                else
                {
                    Debug.LogError("Failed to get script class!");
                }
            }
        }
        else
        {
            Debug.LogError("Target object is not a GameObject!");
        }
    }

    //private void AddCinemachineBrain()
    //{
    //    Camera mainCamera = Camera.main;

    //    if (mainCamera != null)
    //    {
    //        CinemachineBrain brain = mainCamera.GetComponent<CinemachineBrain>();

    //        if (brain == null)
    //        {
    //            brain = mainCamera.gameObject.AddComponent<CinemachineBrain>();

    //            brain.m_DefaultBlend.m_Time = 1f; // Kamera geçiþ süresini belirle
    //            brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut; // Kamera geçiþ stilini belirle

    //            Debug.Log("Cinemachine Brain added to the Main Camera.");
    //        }
    //        else
    //        {
    //            Debug.Log("Cinemachine Brain already exists on the Main Camera.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError("Main Camera not found in the scene.");
    //    }
    //}

    private void AddCharacterController()
    {
        GameObject characterObject = targetObject as GameObject;
        characterObject.tag = "Player";
        CharacterController characterController = characterObject.AddComponent<CharacterController>();
    }
    //private void CreateVirtualCamera()
    //{
    //    GameObject characterObject = targetObject as GameObject;
    //    GameObject virtualCameraObject = new GameObject("VirtualCamera");
    //    CinemachineVirtualCamera virtualCamera = virtualCameraObject.AddComponent<CinemachineVirtualCamera>();
    //    CinemachineCollider collider = virtualCameraObject.AddComponent<CinemachineCollider>();

    //    collider.m_Damping = 0f;
    //    collider.m_MinimumDistanceFromTarget = 0.1f;
    //    collider.m_MaximumEffort = 0;
    //    collider.m_Strategy = CinemachineCollider.ResolutionStrategy.PullCameraForward;
    //    collider.m_CollideAgainst = LayerMask.GetMask();

    //    virtualCamera.Follow = characterObject.transform;
    //    virtualCamera.LookAt = characterObject.transform;
    //    virtualCamera.AddCinemachineComponent<CinemachinePOV>();
    //    virtualCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
    //}
    private void FindVirtualCameraMaker()
    {
        //string scriptPath = "Assets/Scripts/VirtualCameraMaker.cs";
        ////VirtualCameraMaker virtualCameraMaker = AssetDatabase.LoadAssetAtPath<VirtualCameraMaker>(scriptPath);
        //MonoScript virtualCameraMaker = UnityEditor.AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
        //,
        //if (virtualCameraMaker != null)
        //{
        //    virtualCameraMaker.AddCinemachineBrain();
        //    virtualCameraMaker.CreateVirtualCamera();
        //}
        //else
        //{
        //    Debug.LogError("VirtualCameraMaker asset not found!");
        //}


        GameObject targetObject = Resources.Load<GameObject>("GameObject");
        VirtualCameraMaker targetScript = targetObject.GetComponent<VirtualCameraMaker>();
        targetScript.AddCinemachineBrain();
        targetScript.CreateVirtualCamera();
    }
}