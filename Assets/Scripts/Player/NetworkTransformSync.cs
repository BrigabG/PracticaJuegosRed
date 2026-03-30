using Photon.Pun;
using UnityEngine;

public class NetworkTransformSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float lerpSpeed = 10f;

    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private void Awake()
    {
        networkPosition = transform.position;
        networkRotation = transform.rotation;
    }

    private void Update()
    {
        if (photonView.IsMine) return;

        transform.position = Vector3.Lerp(transform.position, networkPosition, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, lerpSpeed * Time.deltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            Debug.Log("Enviando posicion: " + transform.position);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            Debug.Log("Recibiendo posicion: " + networkPosition);
        }
    }
}
