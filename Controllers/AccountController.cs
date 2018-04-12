using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCookieAuth.Models;

namespace NetCoreCookieAuth.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        [HttpGet]
        [Authorize]
        public IActionResult Info()
        {
            return Ok("ok");
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var userList = new List<User>()
            {
                new User(){Id = Guid.NewGuid().ToString(),Name = "admin",Password = "123456"},
                new User(){Id = Guid.NewGuid().ToString(),Name = "test",Password = "test"},
                new User(){Id = Guid.NewGuid().ToString(),Name = "zhouyu",Password = "123456"}
            };

            if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("用户名和密码不能为空");
            }

            var existUser = userList.FirstOrDefault(x => x.Name == user.Name && x.Password == user.Password);

            if (existUser != null)
            {
                var claims = new ClaimsIdentity("Cookie");
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, existUser.Id));
                claims.AddClaim(new Claim(ClaimTypes.Name, existUser.Name));

                var claimsPrincipal = new ClaimsPrincipal(claims);

                await HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return Ok("登录成功");
            }
            return Unauthorized();
        }

        
    }
}