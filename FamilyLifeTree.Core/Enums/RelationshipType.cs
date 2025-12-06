namespace FamilyLifeTree.Core.Enums
{
	/// <summary>
	/// Тип взаиимоотношений.
	/// </summary>
	public enum RelationshipType
	{
		Parent = 1,     // Родитель
		Child = 2,      // Ребенок
		Spouse = 3,     // Супруг(а)
		Partner = 4,    // Партнер (незарегистрированные отношения)
		Sibling = 5,    // Брат/Сестра
		Grandparent = 6,// Дедушка/Бабушка
		Grandchild = 7, // Внук/Внучка
		UncleAunt = 8,  // Дядя/Тетя
		NephewNiece = 9,// Племянник/Племянница
		Cousin = 10,    // Двоюродный брат/сестра
		Godparent = 11, // Крестный родитель
		StepParent = 12,// Отчим/Мачеха
		StepChild = 13, // Падчерица/Пасынок
		AdoptiveParent = 14, // Приемный родитель
		AdoptedChild = 15,   // Приемный ребенок
		Unknown = 99    // Неизвестный тип
	}
}
