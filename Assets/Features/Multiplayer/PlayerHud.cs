using TMPro;
using Unity.Netcode;
using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : NetworkBehaviour
{
    [SerializeField]
    private NetworkVariable<NetworkString> playerNetworkName = new NetworkVariable<NetworkString>();

    private bool overlaySet = false;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            playerNetworkName.Value = $"Player {OwnerClientId}";
        }
        string player = playerNetworkName.Value;
        string[] parts = player.Split(' ');
        string numberString = parts[1];
        int number = int.Parse(numberString);
        Debug.Log(number);
        Debug.Log(number == 1);
        if (number % 2 != 0)
        {
            PlayerSkin playerSkin = GetComponent<PlayerSkin>();
            playerSkin.ChangeTexture();
        }
    }

    public void SetOverlay()
    {
        var localPlayerOverlay = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        localPlayerOverlay.text = $"{playerNetworkName.Value}";
    }

    public void Update()
    {
        if(!overlaySet && !string.IsNullOrEmpty(playerNetworkName.Value))
        {
            SetOverlay();
            overlaySet = true;
        }
    }
}
