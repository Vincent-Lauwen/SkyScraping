using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class OnlineEnvironment : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private bool LoggedIn;
    [SerializeField] private bool Internet;

    public void Update()
    {
        //Check internet connection
        if (Internet && !NetworkCheck.HasInternet())
        {
            DisableOnlineEnvironment();
            return;
        }

        //Check if logged in (google play games)
        if (LoggedIn && !Social.localUser.authenticated)
        {
            DisableOnlineEnvironment();
            return;
        }
        EnableOnlineEnvironment();
    }

    private void DisableOnlineEnvironment()
    {
        button.interactable = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
    }
    private void EnableOnlineEnvironment()
    {
        button.interactable = true;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
    }

    
}
