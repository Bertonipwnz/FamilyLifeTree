namespace FamilyLifeTree.DataAccess.Mappings
{
	using AutoMapper;
	using FamilyLifeTree.Core.Enums;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.DataAccess.Entities;

	/// <summary>
	/// Профиль AutoMapper для настройки маппинга между сущностями и моделями.
	/// </summary>
	public class AutoMapperProfile : Profile
	{
		#region Constructor

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="AutoMapperProfile"/>.
		/// </summary>
		public AutoMapperProfile()
		{
			ConfigurePersonMappings();
			ConfigureRelationshipMappings();
		}

		#endregion Constructor

		#region Private Methods

		/// <summary>
		/// Настраивает маппинги для сущности <see cref="PersonEntity"/> и модели <see cref="Person"/>.
		/// </summary>
		private void ConfigurePersonMappings()
		{
			CreateMap<PersonEntity, Person>()
				.ForMember(dest => dest.Gender, opt =>
					opt.MapFrom(src => (Gender)src.Gender))
				.ForMember(dest => dest.FullName, opt =>
					opt.Ignore()) // Вычисляемое свойство
				.ForMember(dest => dest.ShortName, opt =>
					opt.Ignore()) // Вычисляемое свойство
				.ForMember(dest => dest.Age, opt =>
					opt.Ignore()) // Вычисляемое свойство
				.ForMember(dest => dest.IsAlive, opt =>
					opt.Ignore()) // Вычисляемое свойство
				.ReverseMap()
				.ForMember(dest => dest.Gender, opt =>
					opt.MapFrom(src => (int)src.Gender))
				.ForMember(dest => dest.CreatedAt, opt =>
					opt.Ignore()) // Устанавливается в БД
				.ForMember(dest => dest.UpdatedAt, opt =>
					opt.Ignore()) // Устанавливается в БД
				.ForMember(dest => dest.RelationshipsAsPrimary, opt =>
					opt.Ignore()) // Навигационное свойство
				.ForMember(dest => dest.RelationshipsAsRelated, opt =>
					opt.Ignore()); // Навигационное свойство
		}

		/// <summary>
		/// Настраивает маппинги для сущности <see cref="RelationshipEntity"/> и модели <see cref="Relationship"/>.
		/// </summary>
		private void ConfigureRelationshipMappings()
		{
			CreateMap<RelationshipEntity, Relationship>()
				.ForMember(dest => dest.RelationshipType, opt =>
					opt.MapFrom(src => (RelationshipType)src.RelationshipType))
				.ForMember(dest => dest.DurationYears, opt =>
					opt.Ignore()) // Вычисляемое свойство
				.ForMember(dest => dest.IsActive, opt =>
					opt.Ignore()) // Вычисляемое свойство
				.ReverseMap()
				.ForMember(dest => dest.RelationshipType, opt =>
					opt.MapFrom(src => (int)src.RelationshipType))
				.ForMember(dest => dest.CreatedAt, opt =>
					opt.Ignore()) // Устанавливается в БД
				.ForMember(dest => dest.UpdatedAt, opt =>
					opt.Ignore()) // Устанавливается в БД
				.ForMember(dest => dest.PrimaryPerson, opt =>
					opt.Ignore()) // Навигационное свойство
				.ForMember(dest => dest.RelatedPerson, opt =>
					opt.Ignore()); // Навигационное свойство
		}

		#endregion Private Methods
	}
}