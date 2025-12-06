namespace FamilyLifeTree.UWP
{
    using AutoMapper.Extensions.ExpressionMapping;
    using FamilyLifeTree.Core.Interfaces;
    using FamilyLifeTree.DataAccess;
    using FamilyLifeTree.DataAccess.DbContext;
    using FamilyLifeTree.DataAccess.Mappings;
    using FamilyLifeTree.DataAccess.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Provides application-specific behavior to supplement the default <see cref="Application"/> class.
    /// </summary>
    public sealed partial class App : Application
	{
		#region Private Fields

		/// <summary>
		/// Экземпляр сервис провайдера.
		/// </summary>
		private IServiceProvider _serviceProvider;

		#endregion Private Fields

		/// <summary>
		/// Initializes the singleton application object. This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();

			Suspending += OnSuspending;
		}

		/// <inheritdoc/>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			//TODO: Блокировка от двойного запуска.
			ConfigureServices();

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active.
			if (Window.Current.Content is not Frame rootFrame)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();
				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null)
				{
					// When the navigation stack isn't restored navigate to the first page, configuring
					// the new page by passing required information as a navigation parameter.
					rootFrame.Navigate(typeof(MainPage), e.Arguments);
				}

				// Ensure the current window is active
				Window.Current.Activate();
			}

			InitializeDatabase();
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails.
		/// </summary>
		/// <param name="sender">The Frame which failed navigation.</param>
		/// <param name="e">Details about the navigation failure.</param>
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception($"Failed to load page '{e.SourcePageType.FullName}'.");
		}

		/// <summary>
		/// Invoked when application execution is being suspended. Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Save application state and stop any background activity
			deferral.Complete();
		}

		private void ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddAutoMapper(cfg =>
			{
				cfg.AddExpressionMapping();
				cfg.AddProfile<AutoMapperProfile>();
			});

			services.AddDbContext<FamilyTreeDbContext>(options =>
			{
				var dbPath = System.IO.Path.Combine(
					Windows.Storage.ApplicationData.Current.LocalFolder.Path,
					"FamilyLifeTree.db");

				options.UseSqlite($"Data Source={dbPath}");

#if DEBUG
				options.EnableSensitiveDataLogging();
				options.EnableDetailedErrors();
#endif
			});

			services
				.AddScoped<IPersonRepository, PersonRepository>()
				.AddScoped<IRelationshipRepository, RelationshipRepository>()
				.AddScoped<IUnitOfWork, UnitOfWork>();

			_serviceProvider = services.BuildServiceProvider();

		}

		private void InitializeDatabase()
		{
			try
			{
				using (IServiceScope scope = _serviceProvider.CreateScope())
				{
					FamilyTreeDbContext context = scope.ServiceProvider.GetRequiredService<FamilyTreeDbContext>();
					context.Database.EnsureCreated();
				}
			}
			catch (Exception ex)
			{
				//TODO: Логгирование.
			}
		}
	}
}
