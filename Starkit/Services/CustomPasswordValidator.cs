using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Starkit.Models;

namespace Starkit.Services
{
    public class CustomPasswordValidator : IPasswordValidator<User>
    {
        public int RequiredLength { get; }

        public CustomPasswordValidator(int requiredLength)
        {
            RequiredLength = requiredLength;
        }

        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            string pattern = ".*[0-9]";
            if (!Regex.IsMatch(password,pattern))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать в себе минимум 1 цифру"
                });
            }
            string upList = ".*[A-Z]";
            if (!Regex.IsMatch(password,upList))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать в себе минимум 1 букву верхнего регистра"
                });
            }
            string lowList = ".*[a-z]";
            if (!Regex.IsMatch(password,lowList))
            {
                errors.Add(new IdentityError
                {
                    Description = "Пароль должен содержать в себе минимум 1 букву нижнего регистра"
                });
            }

            return Task.FromResult(errors.Count == 0
                ? IdentityResult.Success
                : IdentityResult.Failed(errors.ToArray())
            );
        }
    }
}