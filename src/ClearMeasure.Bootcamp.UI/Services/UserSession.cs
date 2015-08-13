using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Web;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.Core.Services;
using System;
using MediatR;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Authorization;

namespace ClearMeasure.Bootcamp.UI.Services
{
    public class UserSession : IUserSession
    {
        private readonly IMediator _bus;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserSession(IMediator bus, IHttpContextAccessor contextAccessor)
        {
            _bus = bus;
            _contextAccessor = contextAccessor;
        }

        #region IUserSession Members

        public Employee GetCurrentUser()
        {
            //IOwinContext context = HttpContext.Current.GetOwinContext();
            var user = _contextAccessor.HttpContext.User;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userName = user.Claims.Single(claim => claim.Type == ClaimTypes.Name).Value;
            var currentUser = _bus.Send(new EmployeeByUserNameQuery(userName)).Result;
            blowUpIfEmployeeCannotLogin(currentUser);
            return currentUser;
        }

        public void LogIn(Employee employee)
        {
            blowUpIfEmployeeCannotLogin(employee);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.UserName),
                new Claim(ClaimTypes.Email, employee.EmailAddress)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            _contextAccessor.HttpContext.Authentication
                .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }

        public void LogOut()
        {
            _contextAccessor.HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #endregion

        private void blowUpIfEmployeeCannotLogin(Employee employee)
        {
            if (employee == null)
            {
                throw new InvalidCredentialException("That user doesn't exist or is not valid.");
            }
        }
    }
}