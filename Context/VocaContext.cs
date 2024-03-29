﻿using Microsoft.EntityFrameworkCore;
using VocaSRS.Context.Entities;

namespace VocaSRS.Context
{
    public class VocaContext : DbContext
    {
        public VocaContext(DbContextOptions<VocaContext> options) : base(options)
        {
        }

        public DbSet<Vocabulary> Vocabularies { get; set; }

        public DbSet<Paragraph> Paragraphs { get; set; }

        public DbSet<DailyReview> DailyReviews { get; set; }
    }
}
