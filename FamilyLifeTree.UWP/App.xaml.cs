namespace FamilyLifeTree.UWP
{
	using AutoMapper.Extensions.ExpressionMapping;
	using FamilyLifeTree.Core.Interfaces;
    using FamilyLifeTree.Core.Models;
    using FamilyLifeTree.DataAccess;
	using FamilyLifeTree.DataAccess.DbContext;
	using FamilyLifeTree.DataAccess.Mappings;
	using FamilyLifeTree.DataAccess.Repositories;
    using FamilyLifeTree.Services;
    using FamilyLifeTree.UWP.Helpers;
    using FamilyLifeTree.UWP.Services;
	using FamilyLifeTree.UWP.Views.Pages;
    using FamilyLifeTree.ViewModels.Entities;
    using FamilyLifeTree.ViewModels.Pages;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Utils.Dialogs.Services;
	using Utils.Interfaces;
	using Utils.Logger;
	using Utils.Serialization.Services;
	using Utils.Serialization.Services.Interfaces;
	using Windows.ApplicationModel;
	using Windows.ApplicationModel.Activation;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

#nullable enable

	/// <summary>
	/// Предоставляет специфичное для приложения поведение,
	/// дополняющее класс <see cref="Application"/> по умолчанию.
	/// </summary>
	public sealed partial class App : Application
	{
		#region Private Fields

		/// <summary>
		/// Провайдер служб зависимостей.
		/// </summary>
		private IServiceProvider? _serviceProvider;

		/// <summary>
		/// Мьютекс для единственного экземпляра.
		/// </summary>
		private Mutex? _singleInstanceMutex;

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Получает провайдер служб зависимостей приложения.
		/// </summary>
		public IServiceProvider? ServiceProvider => _serviceProvider;

		/// <summary>
		/// Текущий экземпляр приложения.
		/// </summary>
		public static App CurrentApp => (App)Current;

		#endregion Public Properties

		#region Constructor

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="App"/>.
		/// </summary>
		public App()
		{
			InitializeComponent();
			Suspending += OnSuspending;
		}

		#endregion Constructor

		#region Protected Methods

		/// <inheritdoc/>
		protected override async void OnLaunched(LaunchActivatedEventArgs e)
		{
			if (IsSecondInstance())
				return;

			ConfigureServices();
			InitializeDatabase();
			await InitializeServices();

			if (Window.Current.Content is not Frame rootFrame)
			{
				rootFrame = new Frame();
				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Загрузка состояния из ранее приостановленного приложения
				}

				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null)
				{
					rootFrame.Navigate(typeof(MainPage), e.Arguments);
				}

				Window.Current.Activate();
			}

		}

		#endregion Protected Methods

		#region Private Methods
		
		/// <summary>
		/// Инициализирует сервисы.
		/// </summary>
		private async Task InitializeServices()
		{
			var initializables = _serviceProvider?.GetServices<IAsyncInitializable>() ?? new List<IAsyncInitializable>();
			await Task.WhenAll(initializables.Select(x => x.InitializeAsync()));
		}

		/// <summary>
		/// Это второй экземпляр?
		/// </summary>
		private bool IsSecondInstance()
		{
			_singleInstanceMutex = new Mutex(true, Package.Current.Id.FullName, out bool createdNew);

			return !createdNew;
		}

		/// <summary>
		/// Вызывается при сбое навигации на определенную страницу.
		/// </summary>
		/// <param name="sender">Frame, в котором произошел сбой навигации.</param>
		/// <param name="e">Данные о сбое навигации.</param>
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception($"Не удалось загрузить страницу '{e.SourcePageType.FullName}'.");
		}

		/// <summary>
		/// Вызывается при приостановке выполнения приложения.
		/// Состояние приложения сохраняется без знания о том,
		/// будет ли приложение завершено или возобновлено с сохранением содержимого памяти.
		/// </summary>
		/// <param name="sender">Источник запроса на приостановку.</param>
		/// <param name="e">Данные о запросе на приостановку.</param>
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

			// TODO: Сохранение состояния приложения и остановка любой фоновой активности
			deferral.Complete();
		}

		/// <summary>
		/// Конфигурирует службы и зависимости приложения.
		/// </summary>
		private void ConfigureServices()
		{
			var services = new ServiceCollection();

			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.AddProvider(new SerilogLoggerProvider());
				builder.SetMinimumLevel(LogLevel.Debug);
			});

			ConfigureAutoMapper(services);
			ConfigureDatabase(services);
			ConfigureRepositories(services);

			services
				.AddSingleton<IPathHelper, PathHelper>()
				.AddSingleton<IFileService, FileService>()
				.AddSingleton<IJsonSerializationService, JsonSerializationService>()
				.AddSingleton<INavigationService, UWPNavigationService>()
				.AddSingleton<ILocalizationService, LocalizationService>()
				.AddSingleton<IDialogService, DialogService>()
				.AddSingleton<GenderService>()
				.AddSingleton<IEntityService<GenderModel, GenderViewModel>>(sp => sp.GetRequiredService<GenderService>())
				.AddSingleton<IAsyncInitializable>(sp => sp.GetRequiredService<GenderService>())
				.AddScoped<MainPageViewModel>()
				.AddScoped<StartPageViewModel>()
				.AddScoped<TreePageViewModel>();

			_serviceProvider = services.BuildServiceProvider();
		}

		/// <summary>
		/// Конфигурирует AutoMapper для приложения.
		/// </summary>
		/// <param name="services">Коллекция служб.</param>
		private static void ConfigureAutoMapper(IServiceCollection services)
		{
			services.AddAutoMapper(cfg =>
			{
				cfg.AddExpressionMapping();
				cfg.AddProfile<AutoMapperProfile>();
			});
		}

		/// <summary>
		/// Конфигурирует подключение к базе данных.
		/// </summary>
		/// <param name="services">Коллекция служб.</param>
		private static void ConfigureDatabase(IServiceCollection services)
		{
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
		}

		/// <summary>
		/// Конфигурирует репозитории и Unit of Work.
		/// </summary>
		/// <param name="services">Коллекция служб.</param>
		private static void ConfigureRepositories(IServiceCollection services)
		{
			services
				.AddScoped<IPersonRepository, PersonRepository>()
				.AddScoped<IRelationshipRepository, RelationshipRepository>()
				.AddScoped<IUnitOfWork, UnitOfWork>();
		}

		/// <summary>
		/// Инициализирует базу данных при первом запуске приложения.
		/// </summary>
		private void InitializeDatabase()
		{
			try
			{
				if (_serviceProvider == null)
				{
					throw new InvalidOperationException("ServiceProvider не инициализирован.");
				}

				using (IServiceScope scope = _serviceProvider.CreateScope())
				{
					FamilyTreeDbContext context = scope.ServiceProvider.GetRequiredService<FamilyTreeDbContext>();
					context.Database.EnsureCreated();

					// TODO: Добавить начальные данные при первом запуске
					// SeedData.Seed(context);
				}
			}
			catch (Exception ex)
			{
				LogService.GetCurrentLogger()?.Error(ex, ex.Message ?? ex.ToString());
			}
		}

		#endregion Private Methods

		#region Public Methods

		/// <summary>
		/// Получает службу указанного типа из контейнера зависимостей.
		/// </summary>
		/// <typeparam name="T">Тип запрашиваемой службы.</typeparam>
		/// <returns>Экземпляр запрашиваемой службы.</returns>
		public T? GetService<T>() where T : class
		{
			if (_serviceProvider == null)
			{
				throw new InvalidOperationException("ServiceProvider не инициализирован.");
			}

			return _serviceProvider.GetService<T>();
		}

		/// <summary>
		/// Получает обязательную службу указанного типа из контейнера зависимостей.
		/// </summary>
		/// <typeparam name="T">Тип запрашиваемой службы.</typeparam>
		/// <returns>Экземпляр запрашиваемой службы.</returns>
		/// <exception cref="InvalidOperationException">
		/// Выбрасывается, если ServiceProvider не инициализирован или служба не найдена.
		/// </exception>
		public T? GetRequiredService<T>() where T : class
		{
			if (_serviceProvider == null)
			{
				throw new InvalidOperationException("ServiceProvider не инициализирован.");
			}

			return _serviceProvider.GetRequiredService<T>();
		}

		/// <summary>
		/// Получает службу указанного типа из нового области видимости контейнера зависимостей.
		/// </summary>
		/// <typeparam name="T">Тип запрашиваемой службы.</typeparam>
		/// <returns>Экземпляр запрашиваемой службы.</returns>
		public T? GetScopedService<T>() where T : class
		{
			if (_serviceProvider == null)
			{
				throw new InvalidOperationException("ServiceProvider не инициализирован.");
			}

			using (var scope = _serviceProvider.CreateScope())
			{
				return scope.ServiceProvider.GetService<T>();
			}
		}

		/// <summary>
		/// Устанавливает корневой Frame для сервиса навигации.
		/// Вызывается один раз из MainPage после её создания.
		/// </summary>
		/// <param name="frame">Frame, который будет использоваться для навигации.</param>
		/// <exception cref="ArgumentNullException">Если frame равен null.</exception>
		/// <exception cref="InvalidOperationException">Если Frame уже был установлен.</exception>
		public void SetNavigationFrame(Frame frame)
		{
			if (frame == null)
				throw new ArgumentNullException(nameof(frame));

			var navService = _serviceProvider?.GetRequiredService<INavigationService>()
							 ?? throw new InvalidOperationException("ServiceProvider не инициализирован.");

			if (navService is UWPNavigationService uwpNavService)
				uwpNavService.Initialize(frame);
		}

		#endregion Public Methods
	}
}