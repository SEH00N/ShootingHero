using Cinemachine;
using UnityEngine;

namespace ShootingHero.Clients
{
    public static class ClientInstance
    {
        private static CinemachineVirtualCamera mainVCam = null;
        public static CinemachineVirtualCamera MainVCam
        {
            get
            {
                if(mainVCam == null)
                {
                    GameObject mainVCamObject = GameObject.Find("MainVCam");
                    if(mainVCamObject != null)
                        mainVCam = mainVCamObject.GetComponent<CinemachineVirtualCamera>();
                }
                
                return mainVCam;
            }
        }

        public static string MyPlayerID = string.Empty;
        public static bool IsFireWeaponPacketProcessing = false;
    }
}