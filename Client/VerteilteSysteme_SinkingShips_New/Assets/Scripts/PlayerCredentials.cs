public class PlayerCredentials
{
    public PlayerCredentials()
    {

    }
    public PlayerCredentials(string name, string password)
    {
        this.Name = name;
        this.Password = password;
    }

    public string Name
    {
        get;
        set;
    }

    public string Password
    {
        get;
        set;
    }
    public string ID { get; set; }
}