using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField] private Button PlayServerButton2;
    [SerializeField] private Button PlayHostButton2;
    [SerializeField] private Button PlayClientButton2;

    private void Awake()
    {
        PlayServerButton2.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });

        PlayHostButton2.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        PlayClientButton2.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

}
