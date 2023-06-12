using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInterfaz : MonoBehaviour
{
    [SerializeField] private GameObject textoDebugPrefab;

    public static ManagerInterfaz singleton;

    private void Awake()
    {
        singleton = this;
    }

    public GameObject CrearTextoDebug()
    {
        GameObject gO = Instantiate(textoDebugPrefab, textoDebugPrefab.transform.parent, true);
        gO.transform.localScale = Vector3.one;
        
        return gO;
    }
}
