using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.CommandWpf;

namespace LiPFium
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {

        #region Core Function

        public MainWindow()
        {
            InitializeComponent();
            MWindow.SourceInitialized += WinSourceInitialized;
        }

        /// <summary>
        ///     设置一个控件的背景色渐变
        /// </summary>
        /// <param name="b">控件</param>
        /// <param name="from">起始颜色</param>
        /// <param name="to">终止颜色</param>
        /// <param name="t">时间 秒</param>
        /// <param name="fg">是否设置前景色</param>
        private static void SetBGTransform(Control b, Color from, Color to, double t, bool fg = false)
        {
            var brush = new SolidColorBrush();

            var colorAnimation = new ColorAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(t)),
                AutoReverse = false
            };
            brush.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation, HandoffBehavior.Compose);
            if (fg)
            {
                b.Foreground = brush;
            }
            else
            {
                b.Background = brush;
            }
        }
        #endregion


        #region Dependency Property Core

        /// <summary>
        ///     主窗体的外边距（阴影宽度）
        /// </summary>
        public int WMargin
        {
            get { return (int)GetValue(WMarginProperty); }
            set { SetValue(WMarginProperty, value); }
        }

        /// <summary>
        ///     主窗体的外边距（阴影宽度）的DependencyProperty
        /// </summary>
        // Using a DependencyProperty as the backing store for WMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WMarginProperty =
            DependencyProperty.Register("WMargin", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public double ScaledFontSize
        {
            get { return (double)GetValue(ScaledFontSizeProperty); }
            set { SetValue(ScaledFontSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaledFontSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaledFontSizeProperty =
            DependencyProperty.Register("ScaledFontSize", typeof(double), typeof(MainWindow), new PropertyMetadata(30d));

        #endregion

        #region MainWindow Events Core

        public void MWindowSC(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                ((Button)MWindow.Template.FindName("MaximizeAndRestoreButton", MWindow)).Content = "";
                WMargin = 0;
            }
            else
            {
                ((Button)MWindow.Template.FindName("MaximizeAndRestoreButton", MWindow)).Content = "";
                WMargin = 10;
            }
            //ScaledFontSize = (BackspaceButton?.FontSize + 24)/2 ?? 30;
        }

        private void CloseButtonOnME()
        {
            SetBGTransform((Button)MWindow.Template.FindName("CloseButton", MWindow), Color.FromRgb(242, 242, 242), Colors.Red, 0.1);
        }

        private void CloseButtonOnML()
        {
            SetBGTransform((Button)MWindow.Template.FindName("CloseButton", MWindow), Colors.Red, Color.FromRgb(242, 242, 242), 0.3);
        }

        private void MARButtonOnME()
        {
            SetBGTransform((Button)MWindow.Template.FindName("MaximizeAndRestoreButton", MWindow), Color.FromRgb(242, 242, 242), Color.FromRgb(218, 218, 218), 0.1);
        }

        private void MARButtonOnML()
        {
            SetBGTransform((Button)MWindow.Template.FindName("MaximizeAndRestoreButton", MWindow), Color.FromRgb(218, 218, 218), Color.FromRgb(242, 242, 242), 0.3);
        }

        private void MiniumButtonOnME()
        {
            SetBGTransform((Button)MWindow.Template.FindName("MiniumButton", MWindow), Color.FromRgb(242, 242, 242), Color.FromRgb(218, 218, 218), 0.1);
        }

        private void MiniumButtonOnML()
        {
            SetBGTransform((Button)MWindow.Template.FindName("MiniumButton", MWindow), Color.FromRgb(218, 218, 218), Color.FromRgb(242, 242, 242), 0.3);
        }

        private void TitlebarOnMD()
        {
            DragMove();
        }

        private void MiniumButtonOnClick()
        {
            WindowState = WindowState.Minimized;
        }

        private void MARButtonOnClick()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ((Button)MWindow.Template.FindName("MaximizeAndRestoreButton", MWindow)).Content = "";
            }
            else
            {
                WindowState = WindowState.Maximized;
                ((Button)MWindow.Template.FindName("MaximizeAndRestoreButton", MWindow)).Content = "";
            }
        }

     #endregion


        #region Commands Core
        private void ToRed(Control a)
        {
            SetBGTransform(a, ((SolidColorBrush)a.Background).Color, Colors.Red, 0.1);
        }
        private void ToGray(Control a)
        {
            SetBGTransform(a, ((SolidColorBrush)a.Background).Color, Color.FromRgb(218, 218, 218), 0.1);
        }
        private void ToWhite(Control a)
        {
            SetBGTransform(a, ((SolidColorBrush)a.Background).Color, Color.FromRgb(240, 240, 240), 0.1);
        }
        private RelayCommand<Control> _hoverRedCommand;

        public RelayCommand<Control> HoverRedCommand
            => _hoverRedCommand ?? (_hoverRedCommand = new RelayCommand<Control>(ToRed));

        private RelayCommand<Control> _hoverGrayCommand;

        public RelayCommand<Control> HoverGrayCommand
            => _hoverGrayCommand ?? (_hoverGrayCommand = new RelayCommand<Control>(ToGray));

        private RelayCommand<Control> _backWhiteCommand;

        public RelayCommand<Control> BackWhiteCommand
            => _backWhiteCommand ?? (_backWhiteCommand = new RelayCommand<Control>(ToWhite));

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        private RelayCommand _closeCommand;

        public RelayCommand CloseCommand
            => _closeCommand ?? (_closeCommand = new RelayCommand(CloseApp));

        #endregion
    }
}