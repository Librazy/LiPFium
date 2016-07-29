using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
//http://stackoverflow.com/questions/15641473/how-to-automatically-scale-font-size-for-a-group-of-controls

namespace LiPFium
{
    public class ScaleFontContentControlBehavior<S>
        : Behavior<Grid> where S : ContentControl
    {
        public static readonly DependencyProperty MaxFontSizeProperty = DependencyProperty.Register("MaxFontSize",
            typeof(double), typeof(ScaleFontContentControlBehavior<S>), new PropertyMetadata(20d));

        // MaxFontSize
        public double MaxFontSize
        {
            get { return (double)GetValue(MaxFontSizeProperty); }
            set { SetValue(MaxFontSizeProperty, value); }
        }

        public static readonly DependencyProperty OffsetSizeProperty = DependencyProperty.Register("OffsetSize",
            typeof(double), typeof(ScaleFontContentControlBehavior<S>), new PropertyMetadata(0d));

        // MaxFontSize
        public double OffsetSize
        {
            get { return (double)GetValue(OffsetSizeProperty); }
            set { SetValue(OffsetSizeProperty, value); }
        }

        public static List<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            var children = new List<T>();
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var o = VisualTreeHelper.GetChild(obj, i);
                if (o != null)
                {
                    if (o is T)
                        children.Add((T)o);

                    children.AddRange(FindVisualChildren<T>(o)); // recursive
                }
            }
            return children;
        }

        public static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            var current = initial;

            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += (s, e) => { CalculateFontSize(); };
        }

        private void CalculateFontSize()
        {
            var fontSize = MaxFontSize;

            var tbs = FindVisualChildren<S>(AssociatedObject);

            foreach (var tb in tbs)
            {
                // get desired size with fontsize = MaxFontSize
                var desiredSize = MeasureText(tb);
                var widthMargins = tb.Margin.Left + tb.Margin.Right + OffsetSize;
                var heightMargins = tb.Margin.Top + tb.Margin.Bottom + OffsetSize;

                var desiredHeight = desiredSize.Height + heightMargins;
                var desiredWidth = desiredSize.Width + widthMargins;

                // get column width (if limited)
                var cs = Grid.GetColumnSpan(tb);
                var col = AssociatedObject.ColumnDefinitions[Grid.GetColumn(tb)];
                var colWidth = col.Width == GridLength.Auto ? double.MaxValue : col.ActualWidth * cs;
                // adjust fontsize if text would be clipped horizontally
                if (colWidth < desiredWidth)
                {
                    var factor = (desiredWidth - widthMargins) / (colWidth - widthMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }
                
                var rs = Grid.GetRowSpan(tb);
                var row = AssociatedObject.RowDefinitions[Grid.GetRow(tb)];
                var rowHeight = row.Height == GridLength.Auto ? double.MaxValue : row.ActualHeight * rs;

                // adjust fontsize if text would be clipped vertically
                if (rowHeight < desiredHeight)
                {
                    var factor = (desiredHeight - heightMargins) / (rowHeight - heightMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }
            }
            if (fontSize <= 0) fontSize = 0.01;
            // apply fontsize (always equal fontsizes)
            foreach (var tb in tbs)
            {
                tb.FontSize = fontSize;
            }
        }

        // Measures text size of textblock
        private Size MeasureText(ContentControl tb)
        {
            if (tb.Content != null)
            {
                var formattedText = new FormattedText(tb.Content.ToString(), CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                    MaxFontSize, Brushes.Black); // always uses MaxFontSize for desiredSize
                return new Size(formattedText.Width, formattedText.Height);
            }
            return new Size(0, 0);
        }
    }
    public class ScaleFontGTextBoxBehavior<S>
    : Behavior<Grid> where S : TextBox
    {
        public static readonly DependencyProperty MaxFontSizeProperty = DependencyProperty.Register("MaxFontSize",
            typeof(double), typeof(ScaleFontGTextBoxBehavior<S>), new PropertyMetadata(20d));

        // MaxFontSize
        public double MaxFontSize
        {
            get { return (double)GetValue(MaxFontSizeProperty); }
            set { SetValue(MaxFontSizeProperty, value); }
        }

        public static readonly DependencyProperty OffsetSizeProperty = DependencyProperty.Register("OffsetSize",
    typeof(double), typeof(ScaleFontGTextBoxBehavior<S>), new PropertyMetadata(0d));

        // MaxFontSize
        public double OffsetSize
        {
            get { return (double)GetValue(OffsetSizeProperty); }
            set { SetValue(OffsetSizeProperty, value); }
        }

        public static List<T> FindVisualChildren<T>(DependencyObject obj) where T : DependencyObject
        {
            var children = new List<T>();
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var o = VisualTreeHelper.GetChild(obj, i);
                if (o != null)
                {
                    if (o is T)
                        children.Add((T)o);

                    children.AddRange(FindVisualChildren<T>(o)); // recursive
                }
            }
            return children;
        }

        public static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            var current = initial;

            while (current != null && current.GetType() != typeof(T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        protected override void OnAttached()
        {
            AssociatedObject.SizeChanged += (s, e) => { CalculateFontSize(); };
        }

        private void CalculateFontSize()
        {
            var fontSize = MaxFontSize;

            var tbs = FindVisualChildren<S>(AssociatedObject);

            foreach (var tb in tbs)
            {
                // get desired size with fontsize = MaxFontSize
                var desiredSize = MeasureText(tb);
                var widthMargins = tb.Margin.Left + tb.Margin.Right + OffsetSize;
                var heightMargins = tb.Margin.Top + tb.Margin.Bottom + OffsetSize;

                var desiredHeight = desiredSize.Height + heightMargins;
                var desiredWidth = desiredSize.Width + widthMargins;

                // get column width (if limited)
                var cs = Grid.GetColumnSpan(tb);
                var col = AssociatedObject.ColumnDefinitions[Grid.GetColumn(tb)];
                var colWidth = col.Width == GridLength.Auto ? double.MaxValue : col.ActualWidth * cs;
                // adjust fontsize if text would be clipped horizontally
                if (colWidth < desiredWidth)
                {
                    var factor = (desiredWidth - widthMargins) / (colWidth - widthMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }

                var rs = Grid.GetRowSpan(tb);
                var row = AssociatedObject.RowDefinitions[Grid.GetRow(tb)];
                var rowHeight = row.Height == GridLength.Auto ? double.MaxValue : row.ActualHeight * rs;

                // adjust fontsize if text would be clipped vertically
                if (rowHeight < desiredHeight)
                {
                    var factor = (desiredHeight - heightMargins) / (rowHeight - heightMargins);
                    fontSize = Math.Min(fontSize, MaxFontSize / factor);
                }
            }
            if (fontSize <= 0) fontSize= 0.01;
            // apply fontsize (always equal fontsizes)
            foreach (var tb in tbs)
            {
                tb.FontSize = fontSize;
            }
        }

        // Measures text size of textblock
        private Size MeasureText(TextBox tb)
        {
            if (tb.Text != null)
            {
                var formattedText = new FormattedText(tb.Text, CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                    MaxFontSize, Brushes.Black); // always uses MaxFontSize for desiredSize
                return new Size(formattedText.Width, formattedText.Height);
            }
            return new Size(0, 0);
        }
    }
    public class ScaleFontLabelBehavior : ScaleFontContentControlBehavior<Label>
    {
    }
    public class ScaleFontTextBoxBehavior : ScaleFontGTextBoxBehavior<TextBox>
    {
    }
}