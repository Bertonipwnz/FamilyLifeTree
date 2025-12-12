namespace Utils.Dialogs.Services
{
	using System.Threading.Tasks;
	using Utils.Dialogs.Enums;
	using Utils.Dialogs.ViewModels;

	/// <summary>
	/// Сервис диалога.
	/// </summary>
	public interface IDialogService
	{
		/// <summary>
		/// Устанавливает хост для отображения диалогов.
		/// </summary>
		/// <param name="host">Хост.</param>
		public void SetHost(object host);

		/// <summary>
		/// Показывает диалог.
		/// </summary>
		/// <param name="vm">Модель представления диалога.</param>
		public Task<DialogResult> ShowAsync(DialogViewModelBase vm);
	}
}
