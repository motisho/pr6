using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using pr6.Classes;

namespace pr6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static MainWindow mainWindow;

        public User UserLogIn = new User();
        public MainWindow()
        {
            InitializeComponent();
            OpenPage(new Pages.Login());
        }
        /// <summary>
        /// Функция открытия страницы
        /// </summary>
        /// <param name="page">Страница которую необходимо открыть</param>
        public void OpenPage(Page page)
        {
            DoubleAnimation StartAnimation = new DoubleAnimation();
            StartAnimation.From = 1;
            StartAnimation.To = 0;
            StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
            StartAnimation.Completed += delegate
            {
                frame.Navigate(page);
                DoubleAnimation EndAnimation = new DoubleAnimation();
                EndAnimation.From = 0;
                EndAnimation.To = 1;
                EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                frame.BeginAnimation(Frame.OpacityProperty, EndAnimation);
            };
            frame.BeginAnimation(Frame.OpacityProperty, StartAnimation);
        }
    }
}