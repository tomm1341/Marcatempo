using System;
using System.ComponentModel.DataAnnotations;
using Template.Services.Shared;
using Template.Web.Infrastructure;

namespace Template.Web.Areas.Example.Users
{
    [TypeScriptModule("Example.Users.Server")]
    public class EditViewModel
    {
        public EditViewModel()
        {
        }

        public Guid? Id { get; set; }
        public string Email { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "Cognome")]
        public string Cognome { get; set; }
        [Display(Name = "Username")]
        public string Username { get; set; }

        public string ToJson()
        {
            return Infrastructure.JsonSerializer.ToJsonCamelCase(this);
        }

        public void SetUser(UserDetailDTO userDetailDTO)
        {
            if (userDetailDTO != null)
            {
                Id = userDetailDTO.Id;
                Email = userDetailDTO.Email;
                Nome = userDetailDTO.Nome;
                Cognome = userDetailDTO.Cognome;
                Username = userDetailDTO.Username;
            }
        }

        //public AddOrUpdateUserCommand ToAddOrUpdateUserCommand()
        //{
        //    return new AddOrUpdateUserCommand
        //    {
        //        Id = Id,
        //        Email = Email,
        //        Nome = Nome,
        //        Cognome = Cognome,
        //        Username = Username
        //    };
        //}
    }
}