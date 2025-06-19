using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCustomExtension
{
    public class ScreenResolutionSetter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //4Kモニター用に解像度をフルHDで固定する
            if(Screen.width > 1920 && Screen.height > 1080)
            {
                Screen.SetResolution(1920, 1080, true);
            }
        }
    }
}