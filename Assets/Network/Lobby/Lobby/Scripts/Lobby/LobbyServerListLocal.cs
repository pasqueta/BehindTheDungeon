using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Types;
using UnityEngine.EventSystems;

namespace Prototype.NetworkLobby
{
    public struct Matches
    {
        public string networkId;
        public string name;
        public int maxSize;
        public int currentSize;
    }

    public class LobbyServerListLocal : MonoBehaviour
    {
        [SerializeField]
        EventSystem es;
        public LobbyManager lobbyManager;

        public RectTransform serverListRect;
        public GameObject serverEntryPrefab;
        public GameObject noServerFound;

        protected int currentPage = 0;
        protected int previousPage = 0;
        

        Matches matches = new Matches();

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

        void OnEnable()
        {
            es.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
            currentPage = 0;
            previousPage = 0;

            foreach (Transform t in serverListRect)
                Destroy(t.gameObject);

            noServerFound.SetActive(false);

            StartCoroutine(RefreshPage());
           
            GetComponentInParent<NetworkDiscovery>().StartAsClient();
        }

        public void OnGUIMatchList(bool success, string extendedInfo, Matches matches)
        {
            if (GetComponentInParent<NetworkDiscovery>().broadcastsReceived.Count == 0)
            {
                if (currentPage == 0)
                {
                    noServerFound.SetActive(true);
                }

                currentPage = previousPage;

                return;
            }

            noServerFound.SetActive(false);

            foreach (Transform t in serverListRect)
            {
                Destroy(t.gameObject);
            }

            if (GetComponentInParent<NetworkDiscovery>().broadcastsReceived != null)
            {
                foreach (var addr in GetComponentInParent<NetworkDiscovery>().broadcastsReceived.Keys)
                {
                    var value = GetComponentInParent<NetworkDiscovery>().broadcastsReceived[addr];

                    GameObject o = Instantiate(serverEntryPrefab) as GameObject;

                    o.GetComponent<LobbyLocalServerEntry>().ServerInfo(GetComponentInParent<NetworkDiscovery>().broadcastsReceived.Values.ToString(), GetComponentInParent<NetworkDiscovery>().broadcastsReceived[addr].serverAddress, GetComponentInParent<NetworkDiscovery>().broadcastsReceived.Count, 2, value);
                    
                    o.transform.SetParent(serverListRect, false);
                }
            }
        }

        public void ChangePage(int dir)
        {
            int newPage = Mathf.Max(0, currentPage + dir);

            //if we have no server currently displayed, need we need to refresh page0 first instead of trying to fetch any other page
            if (noServerFound.activeSelf)
                newPage = 0;

            RequestPage(newPage);
        }

        public void RequestPage(int page)
        {
            previousPage = currentPage;
            currentPage = page;
            OnGUIMatchList(true, "", matches);
        }

        IEnumerator RefreshPage()
        {
            while (true)
            {
                //Debug.Log(es.currentSelectedGameObject);
                //Debug.Log(GetComponentInChildren<Button>().gameObject);
               
                RequestPage(0);
               
                yield return new WaitForSeconds(1.0f);

                if (!es.currentSelectedGameObject)
                {

                    es.SetSelectedGameObject(GetComponentsInChildren<Button>()[GetComponentsInChildren<Button>().Length-1].gameObject);
                }
                Debug.Log(es.currentSelectedGameObject);

                //GetComponentInParent<NetworkDiscovery>().Initialize();
                //GetComponentInParent<NetworkDiscovery>().StartAsClient();
                //yield return new WaitForSeconds(3.0f);
            }
        }
    }
}