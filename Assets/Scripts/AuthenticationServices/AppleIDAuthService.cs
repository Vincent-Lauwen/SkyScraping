//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Firebase.Auth;
//using Firebase.Extensions;
//using UnityEngine.Apple;
//using AppleAuth;
//using AppleAuth.Native;
//using AppleAuth.Enums;
//using AppleAuth.Interfaces;
//using System.Text;
//using AppleAuth.Extensions;
//using System.Security.Cryptography;
//using System;
//using Firebase;
//using System.Threading.Tasks;
//using Vincent.Wanderlost.Code;

//public class AppleIDAuthService
//{
//    private static IAppleAuthManager appleAuthManager;

//    public void Initialize()
//    {
//        // If the current platform is supported
//        if (AppleAuthManager.IsCurrentPlatformSupported)
//        {
//            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
//            var deserializer = new PayloadDeserializer();
//            // Creates an Apple Authentication manager with the deserializer
//            appleAuthManager = new AppleAuthManager(deserializer);
//        }
//    }

//    public void InitializeUpdate()
//    {
//        // Updates the AppleAuthManager instance to execute
//        // pending callbacks inside Unity's execution loop
//        if (appleAuthManager != null)
//        {
//            appleAuthManager.Update();
//        }
//    }

//    public void Authenticate(Action<FirebaseUser> callback)
//    {
//        var rawNonce = GenerateRandomString(32);
//        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);
//        AppleAuthLoginArgs loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeFullName | LoginOptions.IncludeEmail, nonce);
//        appleAuthManager.LoginWithAppleId(loginArgs, async credential => {
//            IAppleIDCredential appleIdCredential = credential as IAppleIDCredential;
//            if (appleIdCredential != null)
//            {
//                callback(await PerformFirebaseAuthentication(appleIdCredential, rawNonce));
//            }
//        },
//        error => {
//            throw new LoginException("Failed to sign in with Apple");
//        });
//    }


//    private static async Task<FirebaseUser> PerformFirebaseAuthentication(IAppleIDCredential appleIdCredential, string rawNonce)
//    {
//        FirebaseAuth firebaseAuth = FirebaseAuth.DefaultInstance;

//        var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
//        if (identityToken == null)
//        {
//            throw new LoginException("Invalid identity token");
//        }

//        var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.AuthorizationCode);
//        if (authorizationCode == null)
//        {
//            throw new LoginException("Invalid server auth code");
//        }

//        var firebaseCredential = OAuthProvider.GetCredential("apple.com", identityToken, rawNonce, authorizationCode);
//        if (firebaseCredential == null)
//        {
//            throw new LoginException("Invalid credentials");
//        }

//        FirebaseUser firebaseUser = await firebaseAuth.SignInWithCredentialAsync(firebaseCredential);
//        if (firebaseUser == null)
//        {
//            throw new LoginException("Failed to sing in Firebase");
//        }
//        return firebaseUser;
//    }

//    private static string GenerateRandomString(int length)
//    {
//        if (length <= 0)
//        {
//            throw new Exception("Expected nonce to have positive length");
//        }

//        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
//        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
//        var result = string.Empty;
//        var remainingLength = length;

//        var randomNumberHolder = new byte[1];
//        while (remainingLength > 0)
//        {
//            var randomNumbers = new List<int>(16);
//            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
//            {
//                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
//                randomNumbers.Add(randomNumberHolder[0]);
//            }

//            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
//            {
//                if (remainingLength == 0)
//                {
//                    break;
//                }

//                var randomNumber = randomNumbers[randomNumberIndex];
//                if (randomNumber < charset.Length)
//                {
//                    result += charset[randomNumber];
//                    remainingLength--;
//                }
//            }
//        }

//        return result;
//    }

//    private static string GenerateSHA256NonceFromRawNonce(string rawNonce)
//    {
//        var sha = new SHA256Managed();
//        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
//        var hash = sha.ComputeHash(utf8RawNonce);

//        var result = string.Empty;
//        for (var i = 0; i < hash.Length; i++)
//        {
//            result += hash[i].ToString("x2");
//        }

//        return result;
//    }
//}
