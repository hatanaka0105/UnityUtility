using UnityEngine;
using Photon.Pun;

namespace UnityCustomExtension.Photon
{
    public static class PUN2Utility
    {
        public static float ElapsedTime(float timeStamp)
        {
            return Mathf.Max(0f, unchecked(PhotonNetwork.ServerTimestamp - timeStamp) / 1000f);
        }
    }
}