using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BingoGame.Elements
{
    public class StretchPanel : Panel
    {
        /**********************************************************************/
        #region Attached Properties

        public static readonly DependencyProperty SpanProperty
             = DependencyProperty.RegisterAttached(nameof(SpanProperty).Replace("Property", ""),
                                                   typeof(double),
                                                   typeof(StretchPanel),
                                                   new FrameworkPropertyMetadata(1.0,
                                                       FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                                                       FrameworkPropertyMetadataOptions.AffectsParentArrange),
                                                   ValidateSpan);

        public static double GetSpan(UIElement element)
            => (double)element.GetValue(SpanProperty);

        public static void SetSpan(UIElement element, double value)
            => element.SetValue(SpanProperty, value);

        #endregion Attached Properties

        /**********************************************************************/
        #region Properties

        public static readonly DependencyProperty OrientationProperty
             = DependencyProperty.Register(nameof(Orientation),
                                           typeof(Orientation),
                                           typeof(StretchPanel),
                                           new FrameworkPropertyMetadata(Orientation.Vertical,
                                                       FrameworkPropertyMetadataOptions.AffectsRender),
                                           ValidateOrientation);

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        #endregion Properties

        /**********************************************************************/
        #region FrameworkElement Overrides

        protected override Size MeasureOverride(Size availableSize)
        {
            var desiredWidth = 0.0;
            var desiredHeight = 0.0;

            foreach(var item in EnumerateChildenAndCalculateSizes(availableSize))
            {
                item.ChildElement.Measure(item.CalculatedSize);

                desiredWidth = (Orientation == Orientation.Vertical)
                    ? Math.Max(desiredWidth, item.ChildElement.DesiredSize.Width)
                    : (desiredWidth + item.ChildElement.DesiredSize.Width);

                desiredHeight = (Orientation == Orientation.Vertical)
                    ? (desiredHeight + item.ChildElement.DesiredSize.Height)
                    : Math.Max(desiredHeight, item.ChildElement.DesiredSize.Height);
            }

            return new Size(desiredWidth, desiredHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var childPosition = new Point();

            foreach(var item in EnumerateChildenAndCalculateSizes(finalSize))
            {
                item.ChildElement.Arrange(new Rect(childPosition, item.CalculatedSize));

                childPosition += (Orientation == Orientation.Vertical)
                    ? new Vector(0, item.CalculatedSize.Height)
                    : new Vector(item.CalculatedSize.Width, 0);
            }

            return finalSize;
        }

        #endregion FrameworkElement Overrides

        /**********************************************************************/
        #region Private Methods

        private static bool ValidateSpan(object value)
            => (double)value >= 0.0;

        private static bool ValidateOrientation(object value)
            => Enum.IsDefined(typeof(Orientation), value);

        private IEnumerable<ChildElementWithCalculatedSize> EnumerateChildenAndCalculateSizes(Size containerSize)
        {
            var totalSpan = 0.0;

            return InternalChildren.Cast<UIElement>()
                                   .Select(x => new { ChildElement = x, Span = GetSpan(x) })
                                   .Select(x =>
                                   {
                                       totalSpan += x.Span;
                                       return x;
                                   })
                                   .ToArray()
                                   .Select(x => new { x.ChildElement, WeightedSpan = x.Span / totalSpan })
                                   .Select(x => new ChildElementWithCalculatedSize()
                                   {
                                       ChildElement = x.ChildElement,
                                       CalculatedSize = (Orientation == Orientation.Vertical) ?
                                                        new Size(containerSize.Width, (containerSize.Height * x.WeightedSpan)) :
                                                        new Size((containerSize.Width * x.WeightedSpan), containerSize.Height)
                                   });
        }

        #endregion Private Methods

        /**********************************************************************/
        #region Private Types

        private struct ChildElementWithCalculatedSize
        {
            public UIElement ChildElement;
            public Size CalculatedSize;
        }

        #endregion Private Types
    }
}
