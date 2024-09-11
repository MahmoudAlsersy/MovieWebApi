using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MoviesWebApi.Helper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesWebApi.Services
{
    public interface IUser
    {
        Task<AuthModel> RejsterAsunc(Rejester model);
        Task<AuthModel> LoginAsunc(Login model);
        Task<string> AddRoleAsync(AddRole model);
        Task<string> RoleAsync(Role model);
    }
    public class ClsUser : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Jwt _jwt;

        public ClsUser(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> LoginAsunc(Login model)
        {
            var Authe = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                Authe.Massege = "Email or Password is incorrect";
                return Authe;
            }

            var JwtSecurityToken = await CreateJwtToken(user);
            var Role = await _userManager.GetRolesAsync(user);

            Authe.ISAuthenticate = true;
            Authe.Email = model.Email;
            Authe.Expairon = JwtSecurityToken.ValidTo;
            Authe.Roles =Role.ToList();
            Authe.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);

            return Authe;   
        }

        public async Task<string> AddRoleAsync(AddRole model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || !await _roleManager.RoleExistsAsync(model.Roleid))
                return "UserName or RoleName Is InCorrect";

            if (await _userManager.IsInRoleAsync(user, model.Roleid))
                return "You Were Add This Role To This User";

            var result = await _userManager.AddToRoleAsync(user,model.Roleid);

           return result.Succeeded ? string.Empty : "Something Was Wrong";
        }

        public async Task<AuthModel> RejsterAsunc(Rejester model)
        {
            if(await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel {Massege="That Email is Already SignIn in Application" };

            var User = new ApplicationUser
            {
                FristName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.UserName,
            };

            var Reasult = await _userManager.CreateAsync(User, model.Passwored);

            if (!Reasult.Succeeded)
            {
                var Error = string.Empty;
                foreach (var error in Reasult.Errors)
                {
                    Error +=$"{error.Description}";
                }
                return new AuthModel { Massege = Error};
            }

            await _userManager.AddToRoleAsync(User, "User");


            var JwtSecurityToken = await CreateJwtToken(User);
            return new AuthModel()
            {
                ISAuthenticate = true,
                Email = User.Email,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
                Expairon = JwtSecurityToken.ValidTo
            };

        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var UserClaims = await _userManager.GetClaimsAsync(user);
            var Roles = await _userManager.GetRolesAsync(user);
            var RoleClaim = new List<Claim>();

            foreach (var role in Roles)
            {
                RoleClaim.Add(new Claim("Roles", role));
            }


            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid",user.Id)
            }.Union(UserClaims)
            .Union(RoleClaim);

            var symmtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signin = new SigningCredentials(symmtricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
               issuer: _jwt.Issuer,
            audience: _jwt.Audience,
               claims: Claims,
               signingCredentials: signin,
               expires: DateTime.Now.AddDays(_jwt.DurationInDays)

               );

            return jwtSecurityToken;
        }

        public async Task<string> RoleAsync(Role model)
        {
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role != null)
                return "This Role Exist Already";

            IdentityRole Role = new IdentityRole
            {
                Name = model.RoleName,
                NormalizedName = model.RoleName.ToUpper(),
            };

          var result =  await _roleManager.CreateAsync(Role);

            return result.Succeeded ? string.Empty : " somthis is wrong ";


        }
    }
}
