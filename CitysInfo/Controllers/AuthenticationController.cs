using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CitysInfo.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;

        public class AuthenticateRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        public class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }   
            public string LastName { get; set; }
            public string City {  get; set; }
            
            public CityInfoUser(int intId,string userName,string firstName,string lastName,string city)
            {
                UserId = intId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(_configuration));
        }

        [HttpPost("authenticate")]
       public ActionResult<string> Authenticate(AuthenticateRequestBody authenticateRequestBody)
        {
            //step 1 : validate the username/password
            var user = ValidateCredentials(authenticateRequestBody.Username, authenticateRequestBody.Password);
            if(user == null)
            {
                return Unauthorized();
            }
            //step 2 : create a token
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            //the claim that
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            var jwtSecurityToken = new JwtSecurityToken(_configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);
            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


            return Ok(tokenToReturn);


        }

        private CityInfoUser? ValidateCredentials(string? username, string? password)
        {
            // we dont have a user DB or table. for demo purpose ,we assume the credentials are valid
            return new CityInfoUser(
                1,
                username ?? "",
                "Kevin",
                "Dockx",
                "Antwerp"
                );
        }

    }
}
