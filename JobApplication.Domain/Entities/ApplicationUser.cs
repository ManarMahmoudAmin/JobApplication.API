using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public int? CandidateId { get; set; }
    }
}
