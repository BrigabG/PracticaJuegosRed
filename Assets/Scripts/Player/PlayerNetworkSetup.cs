using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerNetworkSetup : MonoBehaviourPun
{
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private Material _enemyMaterial;
    
    private void Start()
    {
        Setup();
        Destroy(this);
    }

    private void Setup()
    {
        if (photonView.IsMine) return;

        foreach (var mesh in _renderers)
            mesh.material = _enemyMaterial;
    }
}
