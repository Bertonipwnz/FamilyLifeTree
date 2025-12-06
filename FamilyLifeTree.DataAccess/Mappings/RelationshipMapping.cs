namespace FamilyLifeTree.DataAccess.Mappings
{
	using FamilyLifeTree.Core.Enums;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.DataAccess.Entities;
	using System.Collections.Generic;

	/// <summary>
	/// Класс маппинга для <see cref="RelationshipEntity"/> и <see cref="Relationship"/>.
	/// </summary>
	public static class RelationshipMapping
	{
		/// <summary>
		/// Преобразует <see cref="RelationshipEntity"/> в <see cref="Relationship"/>.
		/// </summary>
		/// <param name="entity">Сущность связи из базы данных.</param>
		/// <returns>Модель связи бизнес-логики.</returns>
		public static Relationship ToModel(this RelationshipEntity entity)
		{
			if (entity == null)
				return null;

			return new Relationship
			{
				Id = entity.Id,
				RelationshipType = (RelationshipType)entity.RelationshipType,
				DateFormed = entity.DateFormed,
				DateEnded = entity.DateEnded,
				Notes = entity.Notes,
				PrimaryPersonId = entity.PrimaryPersonId,
				RelatedPersonId = entity.RelatedPersonId,
				IsConfirmed = entity.IsConfirmed,
				ConfirmationReason = entity.ConfirmationReason
			};
		}

		/// <summary>
		/// Преобразует <see cref="Relationship"/> в <see cref="RelationshipEntity"/>.
		/// </summary>
		/// <param name="model">Модель связи бизнес-логики.</param>
		/// <returns>Сущность связи для базы данных.</returns>
		public static RelationshipEntity ToEntity(this Relationship model)
		{
			if (model == null)
				return null;

			return new RelationshipEntity
			{
				Id = model.Id,
				RelationshipType = (int)model.RelationshipType,
				DateFormed = model.DateFormed,
				DateEnded = model.DateEnded,
				Notes = model.Notes,
				PrimaryPersonId = model.PrimaryPersonId,
				RelatedPersonId = model.RelatedPersonId,
				IsConfirmed = model.IsConfirmed,
				ConfirmationReason = model.ConfirmationReason
			};
		}

		/// <summary>
		/// Обновляет сущность <see cref="RelationshipEntity"/> данными из модели <see cref="Relationship"/>.
		/// </summary>
		/// <param name="entity">Сущность для обновления.</param>
		/// <param name="model">Модель с новыми данными.</param>
		public static void UpdateEntity(this RelationshipEntity entity, Relationship model)
		{
			entity.RelationshipType = (int)model.RelationshipType;
			entity.DateFormed = model.DateFormed;
			entity.DateEnded = model.DateEnded;
			entity.Notes = model.Notes;
			entity.PrimaryPersonId = model.PrimaryPersonId;
			entity.RelatedPersonId = model.RelatedPersonId;
			entity.IsConfirmed = model.IsConfirmed;
			entity.ConfirmationReason = model.ConfirmationReason;
		}

		/// <summary>
		/// Преобразует коллекцию сущностей <see cref="RelationshipEntity"/> в коллекцию моделей <see cref="Relationship"/>.
		/// </summary>
		public static IEnumerable<Relationship> ToModels(this IEnumerable<RelationshipEntity> entities)
		{
			if (entities == null)
				yield break;

			foreach (var entity in entities)
			{
				yield return entity.ToModel();
			}
		}

		/// <summary>
		/// Преобразует коллекцию моделей <see cref="Relationship"/> в коллекцию сущностей <see cref="RelationshipEntity"/>.
		/// </summary>
		public static IEnumerable<RelationshipEntity> ToEntities(this IEnumerable<Relationship> models)
		{
			if (models == null)
				yield break;

			foreach (var model in models)
			{
				yield return model.ToEntity();
			}
		}
	}
}
