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

        /// <summary>
        /// Логика взаимодействия для MainWindow.xaml
        /// </summary>

        /// <summary>
        /// Переменная которая ссылается на окно MainWindow
        /// </summary>
        public static MainWindow mainWindow;
        /// <summary>
        /// Авторизированный пользователь
        /// </summary>
        public User UserLogIn = new User();
        public MainWindow()
        {
            InitializeComponent();
            mainWindow = this;
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

        /// <summary>
        /// Точка входа в основное приложение после авторизации/установки PIN.
        /// Здесь можно заменить на реальную загрузку главной страницы приложения.
        /// </summary> 
        public void ShowMainApp()
        {
            OpenPage(new Pages.Login());
        }
    }
}
