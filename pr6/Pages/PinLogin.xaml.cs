using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace pr6.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinLogin.xaml
    /// </summary>
    public partial class PinLogin : Page
    {
        public PinLogin()
        {
            InitializeComponent();
            LoadUserInfo();
            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += LoginOk;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += LoginFail;
        }

        private void PinControl_PinCompleted(string pin)
        {
            BtnLogin.IsEnabled = true;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string pin = PinControl.PinCode;

            if (pin.Length == 4)
            {
                MainWindow.mainWindow.UserLogIn.LoginByPin(pin);
            }
            else
            {
                MessageBox.Show("Введите 4 цифры PIN!");
                PinControl.Clear();
                BtnLogin.IsEnabled = false;
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                // Имя
                LUserName.Content = MainWindow.mainWindow.UserLogIn.Name;

                // Фото
                byte[] img = MainWindow.mainWindow.UserLogIn.Image;
                if (img != null && img.Length > 0)
                {
                    using (var ms = new MemoryStream(img))
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                        IUserControl.Source = bi;
                    }
                }
                else
                {
                    IUserControl.Source = new BitmapImage(new Uri("/Images/ic_user.png", UriKind.Relative));
                }
            }
            catch
            {
                IUserControl.Source = new BitmapImage(new Uri("/Images/ic_user.png", UriKind.Relative));
            }
        }

        private void LoginOk()
        {
            Dispatcher.Invoke(() =>
            {
                string namae = MainWindow.mainWindow.UserLogIn.Name;
                MainWindow.mainWindow.ShowMainApp();
            });
        }

        private void LoginFail()
        {
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Incorrect PIN");

               PinControl.Clear();

            });
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}