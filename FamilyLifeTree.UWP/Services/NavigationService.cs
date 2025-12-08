namespace FamilyLifeTree.UWP.Services
{
    using Serilog;
    using System;
    using Utils.Logger;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Сервис навигации.
    /// </summary>
    public class NavigationService : INavigationService<Frame, Page>
    {
        private readonly ILogger _logger = LogService.GetCurrentLogger();

        private Frame _host;

        private bool _isDisposed = false;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void NavigateToPage(Page page, object param)
        {
            throw new NotImplementedException();
        }

        public void SetHost(Frame host)
        {
            if(host == null)
                throw new NullReferenceException("Host is null");

            _logger?.Debug("Host inited");
            _host = host;
		}
    }
}
