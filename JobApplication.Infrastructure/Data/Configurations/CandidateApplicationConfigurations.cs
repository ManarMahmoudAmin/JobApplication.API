using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Data.Configurations
{
    internal class CandidateApplicationConfigurations : IEntityTypeConfiguration<CandidateApplication>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CandidateApplication> builder)
        {
            builder.HasKey(ca => new
            {
                ca.CandidateId,
                ca.JobId
            });
        }
    }
}
