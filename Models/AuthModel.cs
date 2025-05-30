namespace PlayerManagementAPI.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
    }

    public class UserConstants
    {
        public static List<UserModel> Users = new()
        {
                new UserModel(){ Username="admin",Password="admin123",Role="Admin"}
        };
    }

    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
