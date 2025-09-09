[System.Serializable]
public class User
{
    public float age;
    public string gender;
    public float weight;
    public float height;
    public float depression;
    public float burnout;
    public float alchohol_intake;
}

[System.Serializable]
public class UserResponse
{
    public bool success;
    public User[] user;
}