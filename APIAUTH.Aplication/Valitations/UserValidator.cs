using APIAUTH.Aplication.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.Valitations
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Password).MinimumLength(6).NotEmpty();
        }

    }
}
