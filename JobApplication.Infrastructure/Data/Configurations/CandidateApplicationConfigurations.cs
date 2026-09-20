using JobApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplication.Infrastructure.Data.Configurations
{
    internal class CandidateApplicationConfigurations : IEntityTypeConfiguration<CandidateApplication>
    {
        public void Configure(EntityTypeBuilder<CandidateApplication> builder)
        {
            builder.HasKey(ca => ca.Id);
            builder.HasIndex(ca => new
            {
                ca.CandidateId,
                ca.JobId
            }).IsUnique();
        }
    }
}
