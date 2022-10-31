using Firebase;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System.Collections.Generic;
using System.IO;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using Vincent.Wanderlost.Code;

public class ServiceController : MonoBehaviour
{
    [SerializeField] private GameObject onlineProfile;
    [SerializeField] private GameObject offlineProfile;

    [Header("Login popup")]
    [SerializeField] private GameObject loginPopup;
    [SerializeField] private Text errorLog;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button offlineBtn;

    [Header("Profile popup")]
    [SerializeField] private Text profileMenuName;
    [SerializeField] private Text profileMenuId;

    [Header("Main menu player info")]
    [SerializeField] private Text mainMenuName;

    [Header("Internet Status UI")]
    [SerializeField] private GameObject InternetIcon;

    private static bool OnApplicationStart = true;
    
    private async void Start()
    {
        try
        {
            
            //When starting the game. This should be called once.
            if (OnApplicationStart)
            {
                OnApplicationStart = false;

                await FirebaseInit.InitializeFirebase();
                
#if UNITY_ANDROID
                PlayGamesAuthService.InitializePlayGames();
#endif
                SetActiveLoginPopup(true);
            }
            UpdateProfile();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void Update()
    {
        try
        {
            ShowInternetStatus();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private void UpdateProfile()
    {
        try
        {
            if (Social.localUser.authenticated && Player.playerData != null)
            {
                onlineProfile.SetActive(true);
                offlineProfile.SetActive(false);

                mainMenuName.text = Player.playerData.Name;
                profileMenuName.text = Player.playerData.Name;
                profileMenuId.text = Player.playerData.Id;
            }
            else
            {
                onlineProfile.SetActive(false);
                offlineProfile.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("UpdateProfile method failed", ex);
        }
    }

    public void AttemptLogin()
    {
        try
        {
            BroadcastErrorMessage("");
            LoginInteractionUI(false);

            if (!NetworkCheck.HasInternet())
                throw new LoginException("No internet connection available");

#if UNITY_ANDROID
            PlayGamesAuthService.SignInWithPlayGamesAndFirebase(GetPlayerData);
#endif

#if UNITY_IOS
            GameCenterAuthService.SignInWithGameCenterAndFirebase(GetPlayerData);
#endif
        }
        catch (LoginException ex)
        {
            LoginInteractionUI(true);
            BroadcastErrorMessage(ex.Message);
        }
        catch (Exception ex)
        {
            LoginInteractionUI(true);
            Debug.LogException(ex);
        }
    }
    private async void GetPlayerData(FirebaseUser firebaseUser)
    {
        try
        {
            PlayerData liveData = await SynchronizeData(firebaseUser);
            if (liveData != null)
            {
                Player.playerData = liveData;
                await AchievementProgression.Instance.UpdateAchievementProgression("Ye3CCAca9Jjid8vdzE2p", 1);
                Player.playerData.Achievements = await Firestore.GetUserAchievementsAsync("Achievements", ("Players/" + liveData.Id + "/Achievements"));
                Player.playerData.UnlockedPowers = await Firestore.GetUserPowerups("Players/" + liveData.Id + "/Powerups");
                UpdateProfile();
                SetActiveLoginPopup(false);
            }
        }
        catch (LoginException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

#if UNITY_ANDROID
    public void LogoutHandler()
    {
        PlayGamesAuthService.SignOut(success =>
        {
            if (success)
            {
                OfflineHandler();
                UpdateProfile();
            }
        });
    }
#endif
    public void OfflineHandler()
    {
        try
        {
            string fileName = LocalStorageManager.getLatestSave();
            if (fileName != null)
            {
                PlayerData playerData = LocalStorageManager.LoadData<PlayerData>(fileName);
                Player.playerData = playerData;
            }
            else
            {
                PlayerData anonymousePlayerData = new PlayerData("Guest", "Guest", 100, 0, new Score(0, 0), new List<PowerUp>(), new List<Achievement>());
                LocalStorageManager.SaveData(anonymousePlayerData, "Anonymous");
                Player.playerData = anonymousePlayerData;
            }
        }
        catch (Exception ex)
        {
            PlayerData anonymousePlayerData = new PlayerData("Guest", "Guest", 100, 0, new Score(0, 0), new List<PowerUp>(), new List<Achievement>());
            Player.playerData = anonymousePlayerData;
            Debug.LogException(ex);
        }
    }

    private async Task<PlayerData> SynchronizeData(FirebaseUser user)
    {
        Debug.Log("IM here at start sync: " + user.UserId + " - " + user.DisplayName );
        try
        {
            PlayerData livePlayerData = await Firestore.GetUser<PlayerData>("Players/" + user.UserId);
            PlayerData localPlayerData = LocalStorageManager.LoadData<PlayerData>(user.UserId);
            PlayerData localAnonymousData = LocalStorageManager.LoadData<PlayerData>("Anonymous");

            //One of two local datas is available and live data also available
            if ((localPlayerData != null || localAnonymousData != null) && livePlayerData != null)
            {
                Debug.Log("IF");
                PlayerData local = (localPlayerData ?? localAnonymousData);
                if (local.Timestamp > livePlayerData.Timestamp)
                {
                    string[] fields = { "Highscore", "Timestamp", };
                    await DataSynchronisation.SynchronizeLocalWithLiveDatabase(local, user, fields);
                    return await Firestore.GetDocumentAsync<PlayerData>("Players/" + livePlayerData.Id);
                }
                else
                {
                    DataSynchronisation.SynchronizeLiveWithLocalDatabase(livePlayerData, livePlayerData.Id);
                    return livePlayerData;
                }
            }
            //One of two local datas is available and no live data
            else if ((localPlayerData != null || localAnonymousData != null) && livePlayerData == null)
            {
                Debug.Log("ELSE IF 1");
                string[] fields = { "GoldCurrency", "Health", "Highscore", "Id", "Name", "Timestamp" };
                await DataSynchronisation.SynchronizeLocalWithLiveDatabase((localPlayerData ?? localAnonymousData), user, fields);
                PlayerData syncedData = await Firestore.GetDocumentAsync<PlayerData>("Players/" + localPlayerData.Id);

                //When this local data was anonymous, create a new local file with the players' ID to play furthur with.
                if (localPlayerData == null)
                {
                    DataSynchronisation.SynchronizeLiveWithLocalDatabase(syncedData, syncedData.Id);
                }
                return syncedData;
            }
            //no local data and live data available
            else if ((localPlayerData == null && localAnonymousData == null) && livePlayerData != null)
            {
                Debug.Log("ELSE IF 2");
                DataSynchronisation.SynchronizeLiveWithLocalDatabase(livePlayerData, livePlayerData.Id);
                return livePlayerData;
            }
            //Nothing
            else
            {
                Debug.Log("ELSE");
                PlayerData newPlayer = new PlayerData(user.UserId, user.DisplayName, 100, 0, new Score(0, 0), new List<PowerUp>(), new List<Achievement>());
                await SaveSystem.SaveProgression(newPlayer);
                return newPlayer;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            throw new LoginException("Failed to retrieve online data", ex);
        }
    }

    public void BroadcastErrorMessage(string message)
    {
        errorLog.text = message;
    }
    public void LoginInteractionUI(bool value)
    {
        loginBtn.interactable = value;
        offlineBtn.interactable = value;
    }
    public void SetActiveLoginPopup(bool value)
    {
        loginPopup.SetActive(value);
        LoginInteractionUI(value);
    } 
    private void ShowInternetStatus()
    {
        if (NetworkCheck.HasInternet())
        {
            InternetIcon.SetActive(false);
        }
        else
        {
            InternetIcon.SetActive(true);
        }
    }
}
