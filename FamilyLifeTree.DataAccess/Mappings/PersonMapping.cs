namespace FamilyLifeTree.DataAccess.Mappings
{
    using FamilyLifeTree.Core.Enums;
    using FamilyLifeTree.Core.Models;
    using FamilyLifeTree.DataAccess.Entities;

    /// <summary>
    /// Класс мапинга для <see cref="PersonEntity"/> и <see cref="Person"/>
    /// </summary>
    public static class PersonMapping
	{
		/// <summary>
		/// Преобразует <see cref="PersonEntity"/> в <see cref="Person"/>
		/// </summary>
		public static Person ToModel(this PersonEntity entity)
		{
			if (entity == null) 
				return null;

			return new Person
			{
				Id = entity.Id,
				FirstName = entity.FirstName,
				LastName = entity.LastName,
				MiddleName = entity.MiddleName,
				BirthDate = entity.BirthDate,
				DeathDate = entity.DeathDate,
				Gender = (Gender)entity.Gender,
				Biography = entity.Biography,
				PhotoPath = entity.PhotoPath
			};
		}

		/// <summary>
		/// Преобразует <see cref="Person"/> в <see cref="PersonEntity"/>
		/// </summary>
		public static PersonEntity ToEntity(this Person model)
		{
			if (model == null) 
				return null;
			return new PersonEntity
			{
				Id = model.Id,
				FirstName = model.FirstName,
				LastName = model.LastName,
				MiddleName = model.MiddleName,
				BirthDate = model.BirthDate,
				DeathDate = model.DeathDate,
				Gender = (int)model.Gender,
				Biography = model.Biography,
				PhotoPath = model.PhotoPath
			};
		}

		/// <summary>
		/// Обновляет сущность <see cref="PersonEntity"/>
		/// </summary>
		public static void UpdateEntity(this PersonEntity entity, Person model)
		{
			entity.FirstName = model.FirstName;
			entity.LastName = model.LastName;
			entity.MiddleName = model.MiddleName;
			entity.BirthDate = model.BirthDate;
			entity.DeathDate = model.DeathDate;
			entity.Gender = (int)model.Gender;
			entity.Biography = model.Biography;
			entity.PhotoPath = model.PhotoPath;
		}
	}
}
