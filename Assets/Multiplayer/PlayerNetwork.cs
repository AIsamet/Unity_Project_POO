using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    //Network varible exemple (server authoritive)
    //private NetworkVariable<int> randomNumber = new NetworkVariable<int>(1);

    //Network varible exemple (permission managed)
    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
    {
        _int = 56,
        _bool = true,
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;

        //Instead of string : 
        public FixedString128Bytes message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + "; " + newValue.message);
        };
    }

    private void Update()
    {
        if(!IsOwner) return;

        //Network variable generation
        if (Input.GetKeyDown(KeyCode.S))
        {

            //TestServerRpc("test server message");
            //TestClientRpc("test client message");
            
            //To only run for specific client Id
            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { 1 } } });

/*            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = false,
                message = "test"
            };*/

        }

        Vector3 moveDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.Z)) moveDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        if (Input.GetKey(KeyCode.Q)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        float moveSpeed = 3f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

    }


    //Another method to synchronise data on network via RPC, advantage to have lots of params, better for string messages. -> Only server receive from server  
    [ServerRpc]
    private void TestServerRpc(string message)
    {
        Debug.Log("Test server RPC : " + OwnerClientId + "; " + message);
    }

    //Another method to synchronise data on network via RPC, advantage to have lots of params, better for string messages. -> Everyone receive from server  
    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("Test Client RPC : ");
    }
}
