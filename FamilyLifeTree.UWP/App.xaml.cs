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

		#endregion Private Fields

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

		#region Public Properties

		/// <summary>
		/// Получает провайдер служб зависимостей приложения.
		/// </summary>
		public IServiceProvider? ServiceProvider => _serviceProvider;

		#endregion Public Properties

		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			// Инициализация контейнера зависимостей
			ConfigureServices();

			// TODO: Блокировка от двойного запуска.

			// Не повторяем инициализацию приложения, когда в Window уже есть содержимое,
			// просто убеждаемся, что окно активно.
			if (Window.Current.Content is not Frame rootFrame)
			{
				// Создаем Frame, который будет контекстом навигации, и переходим на первую страницу
				rootFrame = new Frame();
				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					// TODO: Загрузка состояния из ранее приостановленного приложения
				}

				// Помещаем frame в текущее окно
				Window.Current.Content = rootFrame;
			}

			if (e.PrelaunchActivated == false)
			{
				if (rootFrame.Content == null)
				{
					// Когда стек навигации не восстановлен, переходим на первую страницу,
					// настраивая новую страницу путем передачи необходимой информации
					// в качестве параметра навигации.
					rootFrame.Navigate(typeof(MainPage), e.Arguments);
				}

				// Убеждаемся, что текущее окно активно
				Window.Current.Activate();
			}

			// Инициализация базы данных
			InitializeDatabase();
		}

		#endregion Protected Methods

		#region Private Methods

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

			ConfigureAutoMapper(services);
			ConfigureDatabase(services);
			ConfigureRepositories(services);

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
				// TODO: Логгирование ошибок инициализации БД
				System.Diagnostics.Debug.WriteLine($"Ошибка инициализации базы данных: {ex.Message}");
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

		#endregion Public Methods
	}
}