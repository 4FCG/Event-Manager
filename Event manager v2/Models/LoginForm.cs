using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Event_manager_v2.Models
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Gebruikersnaam is verplicht")]
        public string gebruikersnaam { get; set; }
    }
}