#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using UnityEngine.Events;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using System;
using Vincent.Wanderlost.Code;


public class PlayGamesAuthService
{
    public static void InitializePlayGames()
    {
        // Initialize Play Games Configuration and Activate it.
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false /*forceRefresh*/).Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public static void SignInWithPlayGamesAndFirebase(Action<FirebaseUser> callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        // Sign In.
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                throw new LoginException("Failed to sign in Play Games");
            }

            //Get auth code.
            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            if (string.IsNullOrEmpty(authCode))
            {
                throw new LoginException("Invalid server auth code");
            }

            // Use Server Auth Code to make a credential
            Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
            if (credential == null || string.IsNullOrEmpty(credential.Provider) || !credential.IsValid())
            {
                throw new LoginException("Invalid credentials");
            }

            //Use credential to login with Firebase auth
            auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (!task.IsCompleted)
                {
                    throw new LoginException("Failed to sign in Firebase");
                }
                callback(task.Result);
            });
        });
    }

    public static void SignOut(Action<bool> callback)
    {
        FirebaseAuth.DefaultInstance.SignOut();
        PlayGamesPlatform.Instance.SignOut();

        //signout registered?
        if (!Social.localUser.authenticated)
        {
            callback(true);
        }
        else
        {
            callback(false);
        }
    }
}
#endif
