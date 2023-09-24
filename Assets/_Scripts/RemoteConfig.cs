using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    private async void Start()
    {
        if (Utilities.CheckForInternetConnection())
        {
            await UnityServices.InitializeAsync();
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            // Fetch Remote configs data
            await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        }
    }

    private async Task InitializeRemoteConfigAsync()
    {
        await UnityServices.InitializeAsync();    

        if(!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}
