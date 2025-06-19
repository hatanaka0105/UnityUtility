using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDestroyer : MonoBehaviour
{
    [SerializeField]
    private List<string> _targetTags;

    [SerializeField, Header("触った対象がリスポーンするか")]
    private bool _isRespawn;

    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in _targetTags) 
        {
            if (other.CompareTag(tag) && !other.isTrigger)
            {
                if (_isRespawn)
                {
                    other.gameObject.SendMessage("Retry");
                }
                else
                {
                    var view = other.GetComponent<PhotonView>();
                    if (view != null)
                    {
                        PhotonNetwork.Destroy(view);
                    }
                    else
                    {
                        Destroy(other);
                    }
                }
            }
        }
    }
}
