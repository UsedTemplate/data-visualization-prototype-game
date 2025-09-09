using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class DataAPI : MonoBehaviour
{
    public static DataAPI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator GetRequest(string uri, System.Action<User[]> onComplete)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    string json = webRequest.downloadHandler.text;

                    try
                    {
                        UserResponse response = JsonUtility.FromJson<UserResponse>(json);
                        if (response != null && response.success)
                            onComplete?.Invoke(response.user);
                        else
                            onComplete?.Invoke(null);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("JSON parse error: " + e);
                        onComplete?.Invoke(null);
                    }

                    break;

            }
        }
    }
}
