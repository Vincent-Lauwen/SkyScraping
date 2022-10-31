#if UNITY_IOS
using Firebase.Auth;
using System;
using UnityEngine;
using Vincent.Wanderlost.Code;

public class GameCenterAuthService
{
    public static void SignInWithGameCenterAndFirebase(Action<FirebaseUser> callback)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        // Sign In.
        Social.localUser.Authenticate(async (bool success) =>
        {
            if (!success)
            {
                throw new LoginException("Failed to sign in Play Games");
            }

            Credential credential = await GameCenterAuthProvider.GetCredentialAsync();
            if (credential == null || string.IsNullOrEmpty(credential.Provider) || !credential.IsValid())
            {
                throw new LoginException("Invalid credentials");
            }

            //Use credential to login with Firebase auth
            await auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
            {
                if (task.Result == null)
                {
                    throw new LoginException("Failed to sign in Firebase");
                }
                callback(task.Result);
            });  
        });
    }
}
#endif
