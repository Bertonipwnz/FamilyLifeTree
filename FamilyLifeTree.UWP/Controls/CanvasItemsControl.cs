namespace FamilyLifeTree.UWP.Controls
{
	using Utils.Interfaces;
	using Windows.UI.Xaml;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Data;

	/// <summary>
	/// Элемент ItemsControl с поддержкой позионирования элементов на Canvas.
	/// </summary>
	public partial class CanvasItemsControl : ItemsControl
	{
		#region Protected Methods

		/// <inheritdoc/>
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);

			if (element is not FrameworkElement container)
				return;

			if (item is IMoveable positioned)
			{
				container.SetBinding(Canvas.LeftProperty, new Binding
				{
					Source = positioned,
					Path = new PropertyPath(nameof(IMoveable.X)),
					Mode = BindingMode.OneWay
				});

				container.SetBinding(Canvas.TopProperty, new Binding
				{
					Source = positioned,
					Path = new PropertyPath(nameof(IMoveable.Y)),
					Mode = BindingMode.OneWay
				});
			}
		}

		/// <inheritdoc/>
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			if (element is FrameworkElement container)
			{
				container.ClearValue(Canvas.LeftProperty);
				container.ClearValue(Canvas.TopProperty);
			}

			base.ClearContainerForItemOverride(element, item);
		}

		#endregion
	}
}
