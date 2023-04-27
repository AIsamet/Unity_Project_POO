using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Starter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager networkManager = FindObjectOfType<NetworkManager>();
        networkManager.StartHost();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
