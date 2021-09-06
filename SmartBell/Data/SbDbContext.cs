using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace Data
{
    public class SbDbContext : DbContext
    {
        public SbDbContext()
        {
            this.Database.EnsureCreated();
        }
        public SbDbContext(DbContextOptions<SbDbContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.
                    UseLazyLoadingProxies().UseSqlServer(@"Server=tcp:smartbelldb.database.windows.net,1433;Initial Catalog=smartbellDb;Persist Security Info=False;User ID=sbadmin;Password=Passw0rd;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;", b => b.MigrationsAssembly("SmartBell"));
            }
        }

        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateElement> TemplateElements { get; set; }
        public DbSet<BellRing> BellRings { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<OutputPath> OutputPaths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Template tenminutetemplate = new Template { Id = "1", Name = "Tízperces csengetési rend" };
            Template fifteenminutetemplate = new Template { Id = "2", Name = "Tizenöt perces csengetési rend" };
            Template shortenedtemplate = new Template { Id = "3", Name = "Rövidített órák" };
            modelBuilder.Entity<Template>().HasData(tenminutetemplate,fifteenminutetemplate,shortenedtemplate);

            modelBuilder.Entity<TemplateElement>().HasData(

                new { Id = "1", BellRingTime = new DateTime(1, 1, 1, 8, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "2", BellRingTime = new DateTime(1, 1, 1, 8, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "3", BellRingTime = new DateTime(1, 1, 1, 8, 55, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "4", BellRingTime = new DateTime(1, 1, 1, 9, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "5", BellRingTime = new DateTime(1, 1, 1, 9, 55, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "6", BellRingTime = new DateTime(1, 1, 1, 10, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "7", BellRingTime = new DateTime(1, 1, 1, 10, 50, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "8", BellRingTime = new DateTime(1, 1, 1, 11, 35, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "9", BellRingTime = new DateTime(1, 1, 1, 11, 55, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "10", BellRingTime = new DateTime(1, 1, 1, 12, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "11", BellRingTime = new DateTime(1, 1, 1, 12, 50, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "12", BellRingTime = new DateTime(1, 1, 1, 13, 35, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "13", BellRingTime = new DateTime(1, 1, 1, 13, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "14", BellRingTime = new DateTime(1, 1, 1, 14, 25, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "15", BellRingTime = new DateTime(1, 1, 1, 14, 35, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "16", BellRingTime = new DateTime(1, 1, 1, 15, 20, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "17", BellRingTime = new DateTime(1, 1, 1, 15, 25, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "18", BellRingTime = new DateTime(1, 1, 1, 16, 10, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = tenminutetemplate.Id, FilePath = "default.mp3" },



                new { Id = "19", BellRingTime = new DateTime(1, 1, 1, 8, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "20", BellRingTime = new DateTime(1, 1, 1, 8, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "21", BellRingTime = new DateTime(1, 1, 1, 9, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "22", BellRingTime = new DateTime(1, 1, 1, 9, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "23", BellRingTime = new DateTime(1, 1, 1, 10, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "24", BellRingTime = new DateTime(1, 1, 1, 10, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "25", BellRingTime = new DateTime(1, 1, 1, 11, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "26", BellRingTime = new DateTime(1, 1, 1, 11, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "27", BellRingTime = new DateTime(1, 1, 1, 12, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "28", BellRingTime = new DateTime(1, 1, 1, 12, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "29", BellRingTime = new DateTime(1, 1, 1, 13, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "30", BellRingTime = new DateTime(1, 1, 1, 13, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "31", BellRingTime = new DateTime(1, 1, 1, 13, 55, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "32", BellRingTime = new DateTime(1, 1, 1, 14, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "33", BellRingTime = new DateTime(1, 1, 1, 14, 50, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "34", BellRingTime = new DateTime(1, 1, 1, 15, 35, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },

                new { Id = "35", BellRingTime = new DateTime(1, 1, 1, 15, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },
                new { Id = "36", BellRingTime = new DateTime(1, 1, 1, 16, 25, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = fifteenminutetemplate.Id, FilePath = "default.mp3" },



                new { Id = "37", BellRingTime = new DateTime(1, 1, 1, 8, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },
                new { Id = "38", BellRingTime = new DateTime(1, 1, 1, 8, 35, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },

                new { Id = "39", BellRingTime = new DateTime(1, 1, 1, 8, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },
                new { Id = "40", BellRingTime = new DateTime(1, 1, 1, 9, 20, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },

                new { Id = "41", BellRingTime = new DateTime(1, 1, 1, 9, 25, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },
                new { Id = "42", BellRingTime = new DateTime(1, 1, 1, 10, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },

                new { Id = "43", BellRingTime = new DateTime(1, 1, 1, 10, 5, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },
                new { Id = "44", BellRingTime = new DateTime(1, 1, 1, 10, 40, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },

                new { Id = "45", BellRingTime = new DateTime(1, 1, 1, 10, 45, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },
                new { Id = "46", BellRingTime = new DateTime(1, 1, 1, 11, 20, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },

                new { Id = "47", BellRingTime = new DateTime(1, 1, 1, 11, 25, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.Start, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" },
                new { Id = "48", BellRingTime = new DateTime(1, 1, 1, 12, 0, 0), Interval = new TimeSpan(0, 0, 0, 10), IntervalSeconds = 10, Type = BellRingType.End, TemplateId = shortenedtemplate.Id, FilePath = "default.mp3" }

                );

            modelBuilder.Entity<Holiday>().HasData(
                new { Id = "1", Name= "Újév", Type = HolidayType.Holiday, StartTime = new DateTime(1,1,1,0,0,1), EndTime = new DateTime(1,1,1,0,59,23)},
                new { Id = "2", Name = "48-as Forradalom", Type = HolidayType.Holiday, StartTime = new DateTime(1, 3, 15, 0, 0, 1), EndTime = new DateTime(1, 3, 15, 0, 59, 23) },

                new { Id = "3", Name = "Munka ünnepe", Type = HolidayType.Holiday, StartTime = new DateTime(1, 5, 1, 0, 0, 1), EndTime = new DateTime(1, 5, 1, 0, 59, 23) },
                new { Id = "4", Name = "Augusztus 20.", Type = HolidayType.Holiday, StartTime = new DateTime(1, 8, 20, 0, 0, 1), EndTime = new DateTime(1, 8, 20, 0, 59, 23) },

                new { Id = "5", Name = "56-os Forradalom", Type = HolidayType.Holiday, StartTime = new DateTime(1, 10, 23, 0, 0, 1), EndTime = new DateTime(1, 10, 23, 0, 59, 23) },
                new { Id = "6", Name = "Mindenszentek", Type = HolidayType.Holiday, StartTime = new DateTime(1, 11, 1, 0, 0, 1), EndTime = new DateTime(1, 11, 1, 0, 59, 23) },

                new { Id = "7", Name = "Karácsony", Type = HolidayType.Holiday, StartTime = new DateTime(1, 12, 25, 0, 0, 1), EndTime = new DateTime(1, 12, 26, 0, 59, 23) }

                );



            modelBuilder.Entity<TemplateElement>(entity =>
            {
                entity.
                HasOne(templateElement => templateElement.ParentTemplate)
                .WithMany(template => template.TemplateElements)
                .HasForeignKey(templateElement => templateElement.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OutputPath>(entity =>
            {
                entity.
                HasOne(outputPath => outputPath.ParentBellRing)
                .WithMany(bellring => bellring.OutputPaths)
                .HasForeignKey(outputPath => outputPath.BellRingId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

