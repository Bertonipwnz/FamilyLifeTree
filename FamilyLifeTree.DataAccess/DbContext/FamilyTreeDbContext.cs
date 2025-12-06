namespace FamilyLifeTree.DataAccess.DbContext
{
	using FamilyLifeTree.DataAccess.Entities;
	using Microsoft.EntityFrameworkCore;

	/// <summary>
	/// Контекст БД.
	/// </summary>
	public class FamilyTreeDbContext : DbContext
	{
		#region Public Properties

		/// <summary>
		/// Сущности персон.
		/// </summary>
		public DbSet<PersonEntity> Persons { get; set; }

		/// <summary>
		/// Сущности связей.
		/// </summary>
		public DbSet<RelationshipEntity> Relationships { get; set; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="FamilyTreeDbContext"/>
		/// </summary>
		public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options)
			: base(options)
		{
		}

		#endregion Public Constructors

		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			ConfigurePersonEntity(modelBuilder);
			ConfigureRelationshipEntity(modelBuilder);
		}

		#endregion Protected Methods

		#region Private Methods

		/// <summary>
		/// Конфигурирует сущность персоны.
		/// Устанавливает ограничения полей, индексы и значения по умолчанию.
		/// </summary>
		private static void ConfigurePersonEntity(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PersonEntity>(entity =>
			{
				entity.ToTable("Persons");

				entity.HasKey(e => e.Id);

				entity.Property(e => e.FirstName)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.LastName)
					.IsRequired()
					.HasMaxLength(100);

				entity.Property(e => e.MiddleName)
					.HasMaxLength(100);

				entity.Property(e => e.Biography)
					.HasMaxLength(2000);

				entity.Property(e => e.PhotoPath)
					.HasMaxLength(500);

				entity.Property(e => e.Gender)
					.IsRequired();

				entity.Property(e => e.CreatedAt)
					.IsRequired()
					.HasDefaultValueSql("DATETIME('now', 'utc')");

				entity.Property(e => e.UpdatedAt)
					.IsRequired()
					.HasDefaultValueSql("DATETIME('now', 'utc')");

				// Индексы
				entity.HasIndex(e => e.LastName)
					.HasDatabaseName("IX_Persons_LastName");

				entity.HasIndex(e => e.FirstName)
					.HasDatabaseName("IX_Persons_FirstName");

				entity.HasIndex(e => e.BirthDate)
					.HasDatabaseName("IX_Persons_BirthDate");

				entity.HasIndex(e => e.Gender)
					.HasDatabaseName("IX_Persons_Gender");
			});
		}

		/// <summary>
		/// Конфигурирует сущность связей.
		/// Устанавливает ограничения полей, индексы и значения по умолчанию.
		/// </summary>
		private static void ConfigureRelationshipEntity(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RelationshipEntity>(entity =>
			{
				entity.ToTable("Relationships", table =>
				{
					table.HasCheckConstraint(
						"CK_Relationships_SelfReference",
						"PrimaryPersonId != RelatedPersonId");

					table.HasCheckConstraint(
						"CK_Relationships_ValidDates",
						"DateEnded IS NULL OR DateFormed IS NULL OR DateEnded >= DateFormed");
				});

				entity.HasKey(e => e.Id);

				entity.Property(e => e.RelationshipType)
					.IsRequired();

				entity.Property(e => e.Notes)
					.HasMaxLength(2000);

				entity.Property(e => e.ConfirmationReason)
					.HasMaxLength(500);

				entity.Property(e => e.CreatedAt)
					.IsRequired()
					.HasDefaultValueSql("DATETIME('now', 'utc')");

				entity.Property(e => e.UpdatedAt)
					.IsRequired()
					.HasDefaultValueSql("DATETIME('now', 'utc')");

				entity.Property(e => e.IsConfirmed)
					.IsRequired()
					.HasDefaultValue(true);

				// Внешние ключи и связи
				entity.HasOne(e => e.PrimaryPerson)
					.WithMany(p => p.RelationshipsAsPrimary)
					.HasForeignKey(e => e.PrimaryPersonId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Relationships_PrimaryPerson");

				entity.HasOne(e => e.RelatedPerson)
					.WithMany(p => p.RelationshipsAsRelated)
					.HasForeignKey(e => e.RelatedPersonId)
					.OnDelete(DeleteBehavior.Restrict)
					.HasConstraintName("FK_Relationships_RelatedPerson");

				// Уникальный индекс: не допускаем дублирование связей
				entity.HasIndex(e => new { e.PrimaryPersonId, e.RelatedPersonId, e.RelationshipType })
					.IsUnique()
					.HasDatabaseName("IX_Relationships_Unique_Relationship");

				// Индексы для производительности
				entity.HasIndex(e => e.PrimaryPersonId)
					.HasDatabaseName("IX_Relationships_PrimaryPersonId");

				entity.HasIndex(e => e.RelatedPersonId)
					.HasDatabaseName("IX_Relationships_RelatedPersonId");

				entity.HasIndex(e => e.RelationshipType)
					.HasDatabaseName("IX_Relationships_RelationshipType");

				entity.HasIndex(e => e.DateFormed)
					.HasDatabaseName("IX_Relationships_DateFormed");

				entity.HasIndex(e => e.IsConfirmed)
					.HasDatabaseName("IX_Relationships_IsConfirmed");
			});
		}

		#endregion Private Methods
	}
}