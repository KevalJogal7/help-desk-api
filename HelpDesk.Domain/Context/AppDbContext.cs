using System;
using System.Collections.Generic;
using HelpDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Domain.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketAssignment> TicketAssignments { get; set; }

    public virtual DbSet<TicketAttachment> TicketAttachments { get; set; }

    public virtual DbSet<TicketComment> TicketComments { get; set; }

    public virtual DbSet<TicketHistory> TicketHistories { get; set; }

    public virtual DbSet<TicketPriority> TicketPriorities { get; set; }

    public virtual DbSet<TicketStatus> TicketStatuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=help_desk_db;Username=postgres;Password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditLogId).HasName("AuditLogs_pkey");

            entity.Property(e => e.AuditLogId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.PerformedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.PerformedByNavigation).WithMany(p => p.AuditLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AuditLogs_PerformedBy_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Roles_pkey");

            entity.Property(e => e.RoleId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("Tickets_pkey");

            entity.Property(e => e.TicketId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TicketAssignedToNavigations).HasConstraintName("fk_tickets_assigned_to");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TicketCreatedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tickets_created_by");

            entity.HasOne(d => d.Priority).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tickets_priority");

            entity.HasOne(d => d.Status).WithMany(p => p.Tickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tickets_status");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TicketUpdatedByNavigations).HasConstraintName("fk_tickets_updated_by");
        });

        modelBuilder.Entity<TicketAssignment>(entity =>
        {
            entity.HasKey(e => e.AssignmentId).HasName("TicketAssignments_pkey");

            entity.Property(e => e.AssignmentId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.AssignedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TicketAssignmentAssignedByNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TicketAssignments_AssignedBy_fkey");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TicketAssignmentAssignedToNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TicketAssignments_AssignedTo_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketAssignments).HasConstraintName("TicketAssignments_TicketId_fkey");
        });

        modelBuilder.Entity<TicketAttachment>(entity =>
        {
            entity.HasKey(e => e.AttachmentId).HasName("TicketAttachments_pkey");

            entity.Property(e => e.AttachmentId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.UploadedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Comment).WithMany(p => p.TicketAttachments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("TicketAttachments_CommentId_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketAttachments).HasConstraintName("TicketAttachments_TicketId_fkey");
        });

        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("TicketComments_pkey");

            entity.Property(e => e.CommentId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.CommentByNavigation).WithMany(p => p.TicketComments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TicketComments_CommentBy_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketComments).HasConstraintName("TicketComments_TicketId_fkey");
        });

        modelBuilder.Entity<TicketHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("TicketHistory_pkey");

            entity.Property(e => e.HistoryId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.ChangedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.ChangedByNavigation).WithMany(p => p.TicketHistories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TicketHistory_ChangedBy_fkey");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketHistories).HasConstraintName("TicketHistory_TicketId_fkey");
        });

        modelBuilder.Entity<TicketPriority>(entity =>
        {
            entity.HasKey(e => e.PriorityId).HasName("TicketPriority_pkey");

            entity.Property(e => e.PriorityId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<TicketStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("TicketStatus_pkey");

            entity.Property(e => e.StatusId).UseIdentityAlwaysColumn();
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.Property(e => e.UserId).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
