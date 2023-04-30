using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoHideMenu : MonoBehaviour
{

    [SerializeField] private Canvas canva;
    // Start is called before the first frame update
    void Start()
    {
        canva.enabled = false;
    }

}
