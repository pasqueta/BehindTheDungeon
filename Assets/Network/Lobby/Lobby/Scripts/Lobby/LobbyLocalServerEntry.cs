using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyLocalServerEntry : MonoBehaviour
    {
        public Text serverInfoText;
        public Text slotInfo;
        public Button joinButton;

        static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public void ServerInfo(string name, string address, int currentPlayer, int maxPlayer, UnityEngine.Networking.NetworkBroadcastResult value)
        {
            serverInfoText.text = address;
            slotInfo.text = currentPlayer.ToString() + "/" + maxPlayer.ToString();

            joinButton.onClick.RemoveAllListeners();
            joinButton.onClick.AddListener(() => { JoinMatch(value, address); });
        }

        public void JoinMatch(UnityEngine.Networking.NetworkBroadcastResult value, string address)
        {
            string dataString = BytesToString(value.broadcastData);
            var items = dataString.Split(':');

            if (items.Length == 3 && items[0] == "NetworkManager")
            {
                if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
                {
                    NetworkManager.singleton.networkAddress = address;
                    NetworkManager.singleton.networkPort = System.Convert.ToInt32(items[2]);
                    NetworkManager.singleton.StartClient();
                    GetComponentInParent<Transform>().GetComponentInParent<NetworkDiscovery>().StopBroadcast();
                }
            }
        }
    }
}