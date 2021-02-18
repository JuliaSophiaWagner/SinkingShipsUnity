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
            string name = username.GetComponent<TextMeshProUGUI>().text;
            string passwort = password.GetComponent<TMP_InputField>().text;

            playerLoginCredentials.Name = name.ToUpper();
            playerLoginCredentials.Password = this.DecryptPasswort(passwort);
        }
    }

    public void SaveRegisterData()
    {
        if (username != null && password != null)
        {
            string name = username.GetComponent<TextMeshProUGUI>().text;
            string pw = password.GetComponent<TMP_InputField>().text;

            playerRegisterCredentials.Name = name.ToUpper();
            playerRegisterCredentials.Password = this.DecryptPasswort(pw);
        }
    }

    private string DecryptPasswort(string pw)
    {
        byte[] data = System.Text.Encoding.ASCII.GetBytes(pw);
        data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);

        return System.Text.Encoding.ASCII.GetString(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
