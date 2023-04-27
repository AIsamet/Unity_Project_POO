using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField] public Button PlayServerButton;
    [SerializeField] public Button PlayHostButton;
    [SerializeField] public Button PlayClientButton;


    public void Awake()
    {
        Cursor.visible = true;
    }

    public void Update()
    {
        //PlayersInGameText = $"Players un game : {PlayersManager.Instance.PlayersInGame}";
    }

    public void Start()
    {
        PlayHostButton.onClick.AddListener(() =>
        {
            // Charge la seconde scène de manière synchrone
            SceneManager.LoadScene(2, LoadSceneMode.Single);

            // Attend la fin de la frame pour permettre au NetworkManager de s'initialiser
            StartCoroutine(WaitForEndOfFrame());
        });

        PlayClientButton.onClick.AddListener(() =>
        {
            // Charge la seconde scène de manière synchrone
            SceneManager.LoadScene(2, LoadSceneMode.Single);

            // Attend la fin de la frame pour permettre au NetworkManager de s'initialiser
            StartCoroutine(WaitForEndOfFrame());
        });

        PlayServerButton.onClick.AddListener(() =>
        {
            // Charge la seconde scène de manière synchrone
            SceneManager.LoadScene(2, LoadSceneMode.Single);

            // Attend la fin de la frame pour permettre au NetworkManager de s'initialiser
            StartCoroutine(WaitForEndOfFrame());
        });
    }

    IEnumerator WaitForEndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        // Trouve le NetworkManager et démarre le host ou le client ou le serveur
        NetworkManager nm = FindObjectOfType<NetworkManager>();
        if (nm != null)
        {
            if (PlayHostButton.isActiveAndEnabled && nm.StartHost())
            {
                Debug.LogError("host started");
            }
            else if (PlayClientButton.isActiveAndEnabled && nm.StartClient())
            {
                Debug.LogError("client started");
            }
            else if (PlayServerButton.isActiveAndEnabled && nm.StartServer())
            {
                Debug.LogError("server started");
            }
            else
            {
                Debug.LogError("not host/client/server started");
            }
        }
        else
        {
            Debug.LogError("NetworkManager not found");
        }
    }
}
