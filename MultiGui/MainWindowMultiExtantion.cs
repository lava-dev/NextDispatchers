using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;

namespace MultiGui
	{
	public partial class MainWindow
	{
		private static readonly AutoResetEvent SEvent = new AutoResetEvent (false);
		private void MediaWorkerThread (object arg)
		{
			// Create the VisualTargetPresentationSource and then signal the
			// calling thread, so that it can continue without waiting for us.

			var hostVisual = (HostVisual)arg;
			var visualTargetPs = new VisualTargetPresentationSource (hostVisual);
			SEvent.Set ();
			// Create a MediaElement and use it as the root visual for the
			// VisualTarget.

			visualTargetPs.RootVisual = CreateMediaElement ();
			// Run a dispatcher for this worker thread.  This is the central
			// processing loop for WPF.
			Dispatcher.Run ();
		}
		private HostVisual CreateMediaElementOnWorkerThread ()

		{
			// Create the HostVisual that will “contain” the VisualTarget
			// on the worker thread.

			var hostVisual = new HostVisual ();
			// Spin up a worker thread, and pass it the HostVisual that it
			// should be part of.
			var thread = new Thread (MediaWorkerThread);
			thread.SetApartmentState (ApartmentState.STA);
			thread.IsBackground = true;
			thread.Start (hostVisual);
			// Wait for the worker thread to spin up and create the VisualTarget.
			SEvent.WaitOne ();
			return hostVisual;
		}
		/*
		 * ALSO MUST HAVE NEXT CODE
		 * private void OnLoaded(object sender, RoutedEventArgs e)
			{
			Wrapper.Child = CreateMediaElementOnWorkerThread();
			}
		*where Wrapper is element Name (x:Name)
		* 
		* private FrameworkElement CreateMediaElement()

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
			*/
	}
}
 
 