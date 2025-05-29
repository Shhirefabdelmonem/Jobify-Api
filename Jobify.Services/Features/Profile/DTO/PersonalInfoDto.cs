using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jobify.Services.Features.Profile.DTO
{
    public class PersonalInfoDto
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string LinkedIn { get; set; }
            public string GitHub { get; set; }
            public string Portfolio { get; set; }
    }
}
