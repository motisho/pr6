using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pr6.Pages
{
    /// <summary>
    /// Логика взаимодействия для PinLogin.xaml
    /// </summary>
    public partial class PinLogin : Page
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
            private void BtnLogin_Click(object sender, RoutedEventArgs e)
            {
                string pin = $"{TbPin1.Password}{TbPin2.Password}{TbPin3.Password}{TbPin4.Password}";

                if (pin.Length == 4 && pin.All(char.IsDigit))
                    MainWindow.mainWindow.UserLogIn.LoginByPin(pin);
                else
                    MessageBox.Show("Введите 4 цифры PIN!");
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
                    TbPin1.Password = TbPin2.Password = TbPin3.Password = TbPin4.Password = "";
                    TbPin1.Focus();
                });
            }

            private void TbPin1_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter && TbPin1.Password.Length == 1) TbPin2.Focus();
            }

            private void TbPin2_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter && TbPin2.Password.Length == 1) TbPin3.Focus();
            }

            private void TbPin3_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter && TbPin3.Password.Length == 1) TbPin4.Focus();
            }

            private void TbPin4_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter) BtnLogin_Click(null, null);
            }

            private void OpenLogin(object sender, MouseButtonEventArgs e)
            {
                MainWindow.mainWindow.OpenPage(new Login());
            }
        }
    }