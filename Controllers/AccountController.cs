using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreCookieAuth.Models;

namespace NetCoreCookieAuth.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]UserViewModel model)
        {
            var userList = new List<User>()
            {
                new User(){Id = Guid.NewGuid().ToString(),Name = "admin",Password = "123456"},
                new User(){Id = Guid.NewGuid().ToString(),Name = "test",Password = "test"},
                new User(){Id = Guid.NewGuid().ToString(),Name = "zhouyu",Password = "123456"}
            };

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("用户名和密码不能为空");
            }

            var existUser = userList.FirstOrDefault(x => x.Name == model.Name && x.Password == model.Password);

            if (existUser != null)
            {
                var claims = new ClaimsIdentity("Cookie");
                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, existUser.Id));
                claims.AddClaim(new Claim(ClaimTypes.Name, existUser.Name));

                var claimsPrincipal = new ClaimsPrincipal(claims);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return Ok("登录成功");
            }
            return Unauthorized();
        }

        [HttpGet]
        [Authorize()]
        public IActionResult Info()
        {
            return Ok("ok");
        }
    }
}