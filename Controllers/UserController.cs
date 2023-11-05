
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using System.Data;

public class UserController : Controller
{
 
    private readonly IUserService _service;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(IUserService service,IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
    {
     
      _service = service; 
      _configuration = configuration;  
      _httpContextAccessor = httpContextAccessor;
    } 
public IActionResult SignUp()
{
    return View();
}

[HttpPost]
public async Task<ActionResult<User>> SignUp(User request)
{
try{
      var user = _service.GetUserByEmail(request.Email);
      if(user==null)
    {
        CreatePasswordHash(request.Password,out byte[] passwordHash, out byte[] passwordSalt);
        UserDto usr = new UserDto{
                   FullName=request.FullName,
                   Email=request.Email,
                   PasswordHash = passwordHash,
                   PasswordSalt = passwordSalt,
                   Role ="User"
        };
         _service.InsertUser(usr);
         return RedirectToAction("SignIn"); 
    }
    else
    {
       return Content("User already exists !");
    }
   
   }
catch(Exception ex)
{
  return Content("An error occured :"  +  ex.Message);
}
}

public IActionResult LogOut()
{   
  var cookieOptions = new CookieOptions{
  Expires = DateTime.Now.AddMinutes(-1),
};
    
     Response.Cookies.Delete("Token");
     Response.Cookies.Delete("Email");
     return RedirectToAction("Index","Home");
}


private void CreatePasswordHash(string password, out byte[] passwordHash,out byte[] passwordSalt)
{
    using(var hmac = new HMACSHA512())
    {
        passwordSalt =  hmac.Key;
        passwordHash =  hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  
    }
}


public IActionResult SignIn()
{
    return View();
}

[HttpPost]
public async Task<ActionResult<string>> SignIn(User request)
{
try{
      var user = _service.GetUserByEmail(request.Email); 
    if(user!=null)
    {
      if(!VerifyPasswordHash(request.Password,user.PasswordHash,user.PasswordSalt))
      {
        return BadRequest("Wrong Password");
      }
      else
      {
       string token = CreateToken(user);
      _httpContextAccessor?.HttpContext?.Response.Cookies.Append("Token", token);
      }
     Response.Cookies.Append("Email",user.Email);
     return RedirectToAction("ShowMovies","Movie");
      
    } 
   else
    {
      return BadRequest("User Not Found");
    }
}
  catch(Exception ex)
{ 
        return Content("Error Occured :"+ ex.Message);
 }
  
  }
private bool VerifyPasswordHash(string password, byte[] passwordHash,byte [] passwordSalt)
{
    using(var hmac = new HMACSHA512(passwordSalt))
    {
        var computerHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computerHash.SequenceEqual(passwordHash);
    }
}
private string CreateToken(UserDto user)
{
     List<Claim> claims = new List<Claim>
     {
        new Claim(ClaimTypes.Email,user.Email)
     };
     if(user.Role == "Admin"){
      claims.Add(new Claim(ClaimTypes.Role,"Admin"));
     }
     else{
      claims.Add(new Claim(ClaimTypes.Role,"User"));
     }
     var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("TokenSettings:Token").Value));
     var cred = new SigningCredentials(key , SecurityAlgorithms.HmacSha256Signature);
     var token = new JwtSecurityToken(
        claims:claims,
        expires:DateTime.Now.AddDays(1),
        signingCredentials: cred); 
        var jwt = new JwtSecurityTokenHandler().WriteToken(token); 
        return jwt; 
}
     public string? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("TokenSettings:Token").Value!));
 
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = key
            };
 
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
 
            string role = principal.FindFirst(ClaimTypes.Role)?.Value!;
 
            return role;
        }
        catch (Exception)
        {
            return null;
        }
    }
}











