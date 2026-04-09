using Photon.Pun;
using UnityEngine;

public class TankColorAssigner : MonoBehaviourPun
{
    private static readonly Color[] playerColors = new Color[]
    {
        new Color(0.2f, 0.6f, 1f),    // Azul   - Actor 1
        new Color(1f, 0.3f, 0.3f),    // Rojo   - Actor 2
        new Color(0.3f, 1f, 0.4f),    // Verde  - Actor 3
        new Color(1f, 0.85f, 0.1f)    // Amarillo - Actor 4
    };

    private void Start()
    {
        int index = (photonView.Owner.ActorNumber - 1) % playerColors.Length;
        Color assignedColor = playerColors[index];

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = assignedColor;
        }

        Debug.Log(photonView.Owner.NickName + " color asignado: Actor " + photonView.Owner.ActorNumber);
    }
}
