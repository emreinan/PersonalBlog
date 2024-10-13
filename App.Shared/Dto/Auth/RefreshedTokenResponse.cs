namespace App.Shared.Dto.Auth;

public class RefreshedTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
