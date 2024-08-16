using BatchProcess.Api.Models;
using BatchProcess.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace BatchProcess.Api.Database
{
    /// <summary>
    /// Database context for BatchProcess application.
    /// Manages the entity models and database operations.
    /// </summary>
    public class BatchProcessDbContext : DbContext
    {
        private readonly ILogger<BatchProcessDbContext>? _logger;

        /// <summary>
        /// Default constructor initializing ChangeTracker event handlers.
        /// </summary>
        public BatchProcessDbContext()
        {
            ChangeTracker.StateChanged += UpdateTimestamps;
            ChangeTracker.Tracked += UpdateTimestamps;
        }

        /// <summary>
        /// Constructor with DbContextOptions and ILogger parameters.
        /// </summary>
        /// <param name="options">Options for configuring the DbContext.</param>
        /// <param name="logger">Logger for logging information and errors.</param>
        public BatchProcessDbContext(
            DbContextOptions<BatchProcessDbContext> options,
            ILogger<BatchProcessDbContext> logger
        ) : base(options)
        {
            ChangeTracker.StateChanged += UpdateTimestamps;
            ChangeTracker.Tracked += UpdateTimestamps;
            _logger = logger;
        }

        /// <summary>
        /// DbSet for BapDto entities.
        /// </summary>
        public virtual DbSet<BapDto>? Baps { get; set; }

        /// <summary>
        /// DbSet for BapnDto entities.
        /// </summary>
        public virtual DbSet<BapnDto>? BapNs { get; set; }

        /// <summary>
        /// DbSet for PostDto entities.
        /// </summary>
        public virtual DbSet<PostDto>? Posts { get; set; }

        /// <summary>
        /// Configures the entity mappings and relationships.
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder for configuring entity mappings.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring BapDto entity
            modelBuilder.Entity<BapDto>(entity =>
            {
                entity.HasKey(e => e.Bap_Id).HasName("PK_Bap_BapId");
                entity.HasIndex(idx => idx.Bap_Id, "AK_Bap_BapId").IsUnique();
                entity.ToTable("Bap", "dbo", tb => tb.HasComment("Batch Process notifications"));

                entity.Property(e => e.Bap_Id)
                    .HasComment("Bap's Id.")
                    .HasColumnName("Bap_Id")
                    .IsRequired(true)
                    .HasColumnType("guid");

                entity.Property(e => e.Bap_Code)
                    .HasComment("Bap's code.")
                    .HasColumnName("Bap_Code")
                    .IsRequired(true)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(e => e.Bap_State)
                    .HasComment("0=Αρχική, 1=Σε εξέλιξη, 2=Διακόπηκε, 3=Απέτυχε, 4=Ολοκληρώθηκε")
                    .IsRequired(true)
                    .HasColumnName("Bap_State")
                    .HasColumnType("int");

                entity.Property(e => e.Bap_Started_DateTime)
                    .HasComment("Baps started date time.")
                    .HasColumnType("datetime")
                    .IsRequired(false);

                entity.Property(e => e.Bap_Cancelled_DateTime)
                    .HasComment("Baps cancelled date time.")
                    .HasColumnType("datetime")
                    .IsRequired(false);

                entity.Property(e => e.Bap_Finished_DateTime)
                    .HasComment("Baps finish date time.")
                    .HasColumnType("datetime")
                    .IsRequired(false);

                entity.Property(e => e.Bap_Failed_DateTime)
                    .HasComment("Baps failure date time.")
                    .HasColumnType("datetime")
                    .IsRequired(false);

                entity.Property(e => e.Bap_Session_Id)
                    .HasComment("Baps session id.")
                    .HasColumnName("Bap_Session_Id")
                    .IsRequired(false)
                    .HasColumnType("guid");

                entity.Property(p => p.CreatedBy)
                    .HasMaxLength(100)
                    .HasComment("Name of the user who created the record.")
                    .IsRequired(true);

                entity.Property(p => p.CreatedDate)
                    .HasComment("Date and time the record was created.")
                    .HasColumnType("datetime")
                    .IsRequired(true);

                entity.Property(p => p.ModifiedDate)
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<BapDto>()
                .HasMany(e => e.Bap_BapNs)
                .WithOne(e => e.BapN_BapDto)
                .HasForeignKey(e => e.BapN_BapId)
                .HasConstraintName("FK_Bap_BapN_BapId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring BapnDto entity
            modelBuilder.Entity<BapnDto>(entity =>
            {
                entity.HasKey(e => e.BapN_Id).HasName("PK_BapN_BapNId");
                entity.HasIndex(idx => idx.BapN_Id, "AK_BapN_BapNId").IsUnique();
                entity.ToTable("BapN", "dbo", tb => tb.HasComment("Batch Process notifications details"));

                entity.Property(e => e.BapN_Id)
                    .HasComment("BapN's Id.")
                    .HasColumnName("BapN_Id")
                    .IsRequired(true)
                    .HasColumnType("guid");

                entity.Property(e => e.BapN_BapId)
                    .HasComment("BapN's BapId.")
                    .HasColumnName("BapN_BapId")
                    .IsRequired(true)
                    .HasColumnType("guid");

                entity.Property(e => e.BapN_AA)
                    .HasComment("BapN's BapNAA.")
                    .HasColumnName("BapN_AA")
                    .HasColumnType("integer");

                entity.Property(p => p.BapN_DateTime)
                    .HasComment("Date and time bapn was created.")
                    .HasColumnType("datetime")
                    .IsRequired(false);

                entity.Property(e => e.BapN_kind)
                    .HasComment("BapN's kind.")
                    .HasColumnName("BapN_Kind")
                    .IsRequired(true)
                    .HasColumnType("integer");

                entity.Property(e => e.BapN_Data)
                    .HasComment("Bap's data.")
                    .HasColumnName("BapN_Data")
                    .IsRequired(false)
                    .HasColumnType("varchar")
                    .HasMaxLength(100);

                entity.Property(p => p.CreatedBy)
                    .HasMaxLength(100)
                    .HasComment("Name of the user who created the record.")
                    .IsRequired(true);

                entity.Property(p => p.CreatedDate)
                    .HasComment("Date and time the record was created.")
                    .HasColumnType("datetime")
                    .IsRequired(true);

                entity.Property(p => p.ModifiedDate)
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<BapnDto>()
                .HasOne(e => e.BapN_BapDto)
                .WithMany(e => e.Bap_BapNs)
                .HasForeignKey(e => e.BapN_BapId)
                .HasConstraintName("FK_BapN_Bap_BapId")
                .OnDelete(DeleteBehavior.Cascade);

            // Configuring PostDto entity
            modelBuilder.Entity<PostDto>(entity =>
            {
                entity.HasKey(e => e.PostId).HasName("PK_Post_PostId");
                entity.HasIndex(idx => idx.PostId, "AK_Post_PostId").IsUnique();
                entity.ToTable("Post", "dbo", tb => tb.HasComment("Posts for BatchProcess"));

                entity.Property(p => p.PostId)
                    .HasComment("Primary key for post records.")
                    .HasColumnName("PostId")
                    .HasColumnType("integer")
                    .ValueGeneratedOnAdd();

                entity.Property(p => p.UserId)
                    .HasComment("Key for post user records.")
                    .HasColumnName("UserId")
                    .HasColumnType("integer");

                entity.Property(p => p.Title)
                    .HasComment("Post's title.")
                    .HasColumnName("Title")
                    .IsRequired(true)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(p => p.Body)
                    .HasComment("Post's body description.")
                    .HasColumnName("Body")
                    .IsRequired(true)
                    .HasColumnType("varchar")
                    .HasMaxLength(255);

                entity.Property(p => p.CreatedDate)
                    .HasComment("Date and time the record was created.")
                    .HasColumnType("datetime")
                    .IsRequired(true);

                entity.Property(p => p.ModifiedDate)
                    .HasComment("Date and time the record was last updated.")
                    .HasColumnType("datetime");
            });
        }

        /// <summary>
        /// Updates the timestamps for created and modified entities.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">EntityEntryEventArgs containing entity entry information.</param>
        private void UpdateTimestamps(object? sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is BaseEntity entityWithTimestamps)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Modified:
                        entityWithTimestamps.ModifiedDate = DateTimeOffset.Now.DateTime;
                        _logger?.LogInformation("Stamped for update: {Entity}", e.Entry.Entity);
                        break;
                    case EntityState.Added:
                        entityWithTimestamps.CreatedDate = DateTimeOffset.Now.DateTime;
                        _logger?.LogInformation("Stamped for insert: {Entity}", e.Entry.Entity);
                        break;
                }
            }
        }
    }
}
