using System.Windows;
using System.Windows.Media;

namespace MultiGui
	{
	public class VisualTargetPresentationSource : PresentationSource
		{
		private readonly VisualTarget _visualTarget;

		public VisualTargetPresentationSource(HostVisual hostVisual)
			{
			_visualTarget = new VisualTarget(hostVisual);
			}


		public override Visual RootVisual
			{
			get { return _visualTarget.RootVisual; }
			set
			{
				Visual oldRoot = _visualTarget.RootVisual;
				// Set the root visual of the VisualTarget.  This visual will
				// now be used to visually compose the scene.
				_visualTarget.RootVisual = value;
				// Tell the PresentationSource that the root visual has
				// changed.  This kicks off a bunch of stuff like the
				// Loaded event.

				RootChanged(oldRoot, value);
				// Kickoff layout…
				var rootElement = value as UIElement;
				if (rootElement != null)
				{
					rootElement.Measure(new Size(double.PositiveInfinity,
					                             double.PositiveInfinity));
					rootElement.Arrange(new Rect(rootElement.DesiredSize));
				}
			}
			}


		public override bool IsDisposed=>false;


		protected override CompositionTarget GetCompositionTargetCore()
			{
			return _visualTarget;
			}
		}
	}