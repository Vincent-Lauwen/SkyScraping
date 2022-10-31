using Firebase;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

namespace Vincent.Wanderlost.Code
{
    public class FirebaseInit
    {
        public static bool FirebaseInitReady { get; private set; } = false;

        public static async Task InitializeFirebase()
        {
            DependencyStatus dependency = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependency != DependencyStatus.Available)
            {
                throw new Exception("Missing or invalid Firebase dependencies");
            }
            else
            {
                FirebaseInitReady = true;
            }
        }
    } 
}
