using TMPro;
using UnityEngine;

namespace ShootingHero.Clients
{
    public class RoomMemberElementUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text nameText = null;

        [SerializeField]
        private TMP_Text statusText = null;

        public void Initialize(string nickname)
        {
            nameText.text = nickname;
            statusText.text = "Not Ready";
        }

        public void SetOwner()
        {
            statusText.text = "Owner";
        }

        public void SetReady()
        {
            statusText.text = "Ready!!";
        }
    }
}   