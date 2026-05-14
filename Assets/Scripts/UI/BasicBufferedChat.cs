using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class BasicBufferedChat : MonoBehaviourPun
{
    [Header("UI (assign via Inspector)")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI chatOutput;
    [SerializeField] private ScrollRect scrollRect;

    private void Awake()
    {
        if (inputField != null)
            inputField.onSubmit.AddListener(OnInputSubmit);
    }

    private void OnDestroy()
    {
        if (inputField != null)
            inputField.onSubmit.RemoveListener(OnInputSubmit);
    }

    private void OnInputSubmit(string _)
    {
        SendCurrentInput();
    }

    public void SendCurrentInput()
    {
        if (!PhotonNetwork.InRoom) return;
        if (inputField == null) return;

        string message = inputField.text;
        if (string.IsNullOrWhiteSpace(message)) return;

        string sender = GetSenderName(PhotonNetwork.LocalPlayer);
        int serverTimestamp = PhotonNetwork.ServerTimestamp;

        photonView.RPC(
            "RPC_ReceiveMessage",
            RpcTarget.AllBuffered,
            sender,
            message,
            serverTimestamp
        );

        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }

    [PunRPC]
    private void RPC_ReceiveMessage(string sender, string message, int serverTimestamp)
    {
        if (chatOutput == null) return;

        string stamp = FormatServerTimestamp(serverTimestamp);
        chatOutput.text += $"[{stamp}] {sender}: {message}\n";

        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }

    private static string GetSenderName(Player player)
    {
        if (player == null) return "Unknown";
        return $"Player{player.ActorNumber}";
    }

    private static string FormatServerTimestamp(int serverTimestamp)
    {
        uint ms = unchecked((uint)serverTimestamp);
        TimeSpan t = TimeSpan.FromMilliseconds(ms);
        return $"{(int)t.TotalHours:00}:{t.Minutes:00}:{t.Seconds:00}";
    }
}
