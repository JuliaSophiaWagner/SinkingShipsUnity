using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public GameObject username;
    public GameObject password;
    public PlayerCredentials playerLoginCredentials;
    public PlayerCredentials playerRegisterCredentials;

    // Start is called before the first frame update
    void Start()
    {
        this.playerLoginCredentials = new PlayerCredentials();
        this.playerRegisterCredentials = new PlayerCredentials();
    }

    public void SaveLoginData()
    {
        if (username != null && password != null)
        {
            playerLoginCredentials.Name = username.GetComponent<TextMeshProUGUI>().text;
            playerLoginCredentials.Password = password.GetComponent<TMP_InputField>().text;
        }
    }

    public void SaveRegisterData()
    {
        if (username != null && password != null)
        {
            playerRegisterCredentials.Name = username.GetComponent<TextMeshProUGUI>().text;
            playerRegisterCredentials.Password = password.GetComponent<TMP_InputField>().text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
