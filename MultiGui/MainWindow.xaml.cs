using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MultiGui.Annotations;

//https://blogs.msdn.microsoft.com/dwayneneed/2007/04/26/multithreaded-ui-hostvisual/

namespace MultiGui
	{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : INotifyPropertyChanged
		{
		private bool _isUpdating = true;
		private Brush _ovalBrush;
		private int _ovalHeight;
		private int _ovalWidth;

		public MainWindow()
			{
			InitializeComponent();
			var rnd = new Random();
			Task.Factory.StartNew(async ()=>
				{
				try
				{
					PropertyInfo[] properties = typeof(Brushes).GetProperties();
					while (true)
					{
						if (IsUpdating)
						{
							int random = rnd.Next(properties.Length);
							OvalBrush = (Brush) properties[random].GetValue(null, null);
							OvalHeight = rnd.Next(30, 300);
							OvalWidth = rnd.Next(20, 500);
						}
						await Task.Delay(5);
					}
				}
				catch(TaskCanceledException) { }
				catch (Exception ex)
				{
					Debug.WriteLine(ex);
					Debugger.Break();
				}
				}, TaskCreationOptions.LongRunning);
			Loaded += OnLoaded;
			}

		private void OnLoaded(object sender, RoutedEventArgs e)
			{
			Wrapper.Child = CreateMediaElementOnWorkerThread();
			}

		
		
		private FrameworkElement CreateMediaElement()

			{
			var mc= new MyControl()  ;
			var binding = new Binding
			{
				Source = this,
				Path = new PropertyPath ("OvalHeight")
			};
			BindingOperations.SetBinding (mc, MyControl.OvalHeightProperty, binding);
			binding = new Binding
			{
				Source = this,
				Path = new PropertyPath ("OvalWidth")
			};
			BindingOperations.SetBinding (mc, MyControl.OvalWidthProperty, binding);

			binding = new Binding
			{
				Source = this,
				Path = new PropertyPath ("OvalBrush")
			};
			BindingOperations.SetBinding (mc, MyControl.OvalBrushProperty, binding);
			return mc;
			}


		
		private void Freeze_OnClick(object sender, RoutedEventArgs e)
			{
			Dispatcher.Invoke(()=> { Thread.Sleep(10000); });
			}

		#region Properties

		public Brush OvalBrush
			{
			get { return _ovalBrush; }
			set
			{
				if (Equals(value, _ovalBrush)) return;
				_ovalBrush = value;
				OnPropertyChanged();
			}
			}

		public int OvalHeight
			{
			get { return _ovalHeight; }
			set
			{
				if (value == _ovalHeight) return;
				_ovalHeight = value;
				OnPropertyChanged();
			}
			}

		public int OvalWidth
			{
			get { return _ovalWidth; }
			set
			{
				if (value == _ovalWidth) return;
				_ovalWidth = value;
				OnPropertyChanged();
			}
			}

		public bool IsUpdating
			{
			get { return _isUpdating; }
			set
			{
				if (value == _isUpdating) return;
				_isUpdating = value;
				OnPropertyChanged();
			}
			}

		#endregion

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}

		#endregion
	}
	}