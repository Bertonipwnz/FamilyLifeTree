namespace Utils.Dialogs.Models.Interfaces
{
    using Utils.Dialogs.ViewModels;

    public interface IDialogHostUI
	{
		void Show(DialogViewModelBase vm);
		void Close(DialogViewModelBase vm);
	}
}
