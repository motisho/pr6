using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using pr6.Classes;


namespace pr6.Pages
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        string OldLogin;
        int CountSetPassword;
        bool IsCapture;
        public Login()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;
            Capture.HandlerCorrectCapture += CorrectCapture;
        }
        public void CorrectLogin()
        {
            if (OldLogin != TbLogin.Text)
            {
                SetNotification("Hi, " + MainWindow.mainWindow.UserLogIn.Name, Brushes.Black);
                try
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(MainWindow.mainWindow.UserLogIn.Image);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    ImageSource imgSrc = biImg;
                    DoubleAnimation StartAnimation = new DoubleAnimation();
                    StartAnimation.From = 1;
                    StartAnimation.To = 0;
                    StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                    StartAnimation.Completed += delegate
                    {
                        IUser.Source = imgSrc;
                        DoubleAnimation EndAnimation = new DoubleAnimation();
                        EndAnimation.From = 0;
                        EndAnimation.To = 1;
                        EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                        IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                    };
                    IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
                }
                catch (Exception exp)
                {
                    Debug.WriteLine(exp.Message);
                }
                ;
                OldLogin = TbLogin.Text;
            }
        }
        /// <summary>
        /// Метод не успешной авторизации
        /// </summary>
        public void InCorrectLogin()
        {
            if (LNameUser.Content != "")
            {
                LNameUser.Content = "";
                // Создаём анимацию старта
                DoubleAnimation StartAnimation = new DoubleAnimation();
                StartAnimation.From = 1;
                StartAnimation.To = 0;
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                StartAnimation.Completed += delegate
                {
                    IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/ic_user.png"));
                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    EndAnimation.From = 0;
                    EndAnimation.To = 1;
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                    IUser.BeginAnimation(OpacityProperty, EndAnimation);
                };
                IUser.BeginAnimation(OpacityProperty, StartAnimation);
            }
            if (TbLogin.Text.Length > 0)
                SetNotification("Login is incorrect", Brushes.Red);
        }
        public void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;
        }
        /// <summary>
        /// Ввод пароля
        /// </summary>
        private void SetPassword(object sender, KeyEventArgs e)
        {
            // Если пользователь нажал клавишу Enter
            if (e.Key == Key.Enter)
                // Вызываем метод ввода пароля
                SetPassword();
        }

        /// <summary>
        /// Ввод пароля
        /// </summary>
        public void SetPassword()
        {
            if (MainWindow.mainWindow.UserLogIn.Password != String.Empty)
            {
                if (IsCapture)
                {
                    if (MainWindow.mainWindow.UserLogIn.Password == TbPassword.Password)
                    {
                        MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Login));
                    }
                    else
                    {
                        if (CountSetPassword > 0)
                        {
                            SetNotification($"Password is incorrect, {CountSetPassword} attempts left", Brushes.Red);
                            CountSetPassword--;
                        }
                        else
                        {
                            Thread TBlockAuthorization = new Thread(BlockAuthorization);
                            TBlockAuthorization.Start();
                            SendMail.SendMessage("An attempt was made to log into your account.", MainWindow.mainWindow.UserLogIn.Login);
                        }
                    }
                }
                else
                    SetNotification($"Enter capture", Brushes.Red);
            }
        }
        /// <summary>
        /// Метод блокировки авторизации
        /// </summary>
        public void BlockAuthorization()
        {
            DateTime StartBlock = DateTime.Now.AddMinutes(3);
            Dispatcher.Invoke(() =>
            {
                TbLogin.IsEnabled = false;
                TbPassword.IsEnabled = false;
                Capture.IsEnabled = false;
            });
            for (int i = 0; i < 180; i++)
            {
                TimeSpan TimeIdle = StartBlock.Subtract(DateTime.Now);
                string s_minutes = TimeIdle.Minutes.ToString();
                if (TimeIdle.Minutes < 10)
                    s_minutes = "0" + TimeIdle.Minutes;
                string s_seconds = TimeIdle.Seconds.ToString();
                if (TimeIdle.Seconds < 10)
                    s_seconds = "0" + TimeIdle.Seconds;
                Dispatcher.Invoke(() =>
                {
                    SetNotification($"Reauthorization available in: {s_minutes}:{s_seconds}", Brushes.Red);
                });
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                SetNotification("Hi, " + MainWindow.mainWindow.UserLogIn.Name, Brushes.Black);
                TbLogin.IsEnabled = true;
                TbPassword.IsEnabled = true;
                Capture.IsEnabled = true;
                Capture.CreateCapture();
                IsCapture = false;
                CountSetPassword = 2;
            });
        }
        /// <summary>
        /// Ввод логина пользователя
        /// </summary>
        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
                if (TbPassword.Password.Length > 0)
                    SetPassword();
            }
        }
        /// <summary>
        /// Ввод логина пользователя
        /// </summary>
        private void SetLogin(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            if (TbPassword.Password.Length > 0)
                SetPassword();
        }
        /// <summary>
        /// Метод уведомлений пользователя
        /// </summary>
        /// <param name="Message">Сообщение которое необходимо вывести</param>
        /// <param name="_Color">Цвет сообщения</param>
        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }
        /// <summary>
        /// Метод открытия страницы восстановления пароля
        /// </summary>
        private void RecoveryPassword(object sender, MouseButtonEventArgs e) =>
            MainWindow.mainWindow.OpenPage(new Recovery());

        /// <summary>
        /// Метод открытия страницы регистрации
        /// </summary>
        private void OpenRegin(object sender, MouseButtonEventArgs e) =>
            MainWindow.mainWindow.OpenPage(new Regin());

        private void BtnPinLogin_Click(object sender, RoutedEventArgs e)
        {
            // Открываем страницу входа по PIN
            MainWindow.mainWindow.OpenPage(new PinLogin());
        }
    }
}