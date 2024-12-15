using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class Auth
{
    public static bool CheckNetworkStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No network connection.");
            return false;
        }
        else
        {
            Debug.Log("Network connection is available.");
            return true;
        }
    }

    public static async Task SignInAnonymously()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                Debug.Log(s);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}
