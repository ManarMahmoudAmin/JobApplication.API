using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Domain.Entities
{
    internal class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CvUrl { get; set; }
    }
}
