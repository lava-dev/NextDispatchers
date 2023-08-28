using System.Windows;
using System.Windows.Media;

namespace MultiGui
{
	/// <summary>
	/// Interaction logic for MyControl.xaml
	/// </summary>
	public partial class MyControl
		{
		public MyControl ()
		{
			InitializeComponent ();
		}

		public int OvalHeight
		{
			get { return (int)GetValue (OvalHeightProperty); }
			set { SetValue (OvalHeightProperty, value); }
		}
		public static readonly DependencyProperty OvalHeightProperty =
			DependencyProperty.Register ("OvalHeight", typeof (int), typeof (MyControl), new FrameworkPropertyMetadata (100));
		public int OvalWidth
		{
			get { return (int)GetValue (OvalWidthProperty); }
			set { SetValue (OvalWidthProperty, value); }
		}
		public static readonly DependencyProperty OvalWidthProperty =
			DependencyProperty.Register ("OvalWidth", typeof (int), typeof (MyControl), new FrameworkPropertyMetadata (100));
		public Brush OvalBrush
		{
			get { return (Brush)GetValue (OvalBrushProperty); }
			set { SetValue (OvalBrushProperty, value); }
		}
		public static readonly DependencyProperty OvalBrushProperty =
			DependencyProperty.Register ("OvalBrush", typeof (Brush), typeof (MyControl), new FrameworkPropertyMetadata (new SolidColorBrush(Colors.Gray)));
		}

}
