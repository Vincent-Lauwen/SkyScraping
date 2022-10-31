using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private Button button;

    // Start is called before the first frame update
    private void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(delegate { SwitchScene(sceneName); });
    }

    void SwitchScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
