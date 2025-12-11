using System;
using System.Collections.Generic;
using Utils.Dialogs.Models.Interfaces;
using Utils.Dialogs.ViewModels;
using Windows.UI.Xaml.Controls;

namespace FamilyLifeTree.UWP.Controls
{
	public sealed partial class DialogHost : Grid, IDialogHostUI
	{
		private readonly List<UserControl> _opened = new();

		

        public void Show(DialogViewModelBase vm)
        {
            throw new NotImplementedException();
        }

        public void Close(DialogViewModelBase vm)
        {
            throw new NotImplementedException();
        }
    }
}
