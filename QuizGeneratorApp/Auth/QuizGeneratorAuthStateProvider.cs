using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using QuizGeneratorCommon.Auth;

namespace QuizGeneratorApp.Auth;

public class QuizGeneratorAuthStateProvider : AuthenticationStateProvider
{
	private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity()); 
    
    public QuizGeneratorAuthStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<UserSession>("IpnQuizGeneratorData");
            var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;
            if (userSession == null)
                return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Sid, userSession.Id),
                new(ClaimTypes.Name, userSession.UserName),
                new(ClaimTypes.Email, userSession.Email),
            }, "CustomAuth"));
        
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        ClaimsPrincipal claimsPrincipal;
        if (userSession != null)
        {
            await _sessionStorage.SetAsync("IpnQuizGeneratorData", userSession);
            claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Sid, userSession.Id),
                new(ClaimTypes.Name, userSession.UserName),
                new(ClaimTypes.Email, userSession.Email),
            }, "CustomAuth"));
        }
        else
        {
            await _sessionStorage.DeleteAsync("IpnQuizGeneratorData");
            claimsPrincipal = _anonymous;
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}