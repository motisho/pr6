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

namespace pr6.Elements
{
    /// <summary>
    /// Логика взаимодействия для PinInputControl.xaml
    /// </summary>
    public partial class PinInputControl : UserControl
    {
        /// <summary>
        /// Логика взаимодействия для PinInputControl.xaml
        /// </summary>

        // Событие, которое срабатывает, когда PIN полностью введен
        public event PinCompletedEventHandler PinCompleted;
        public delegate void PinCompletedEventHandler(string pin);

        private readonly PasswordBox[] pinBoxes;

        public PinInputControl()
        {
            InitializeComponent();
            // Сбор всех PasswordBox в массив для удобной работы
            pinBoxes = new PasswordBox[] { TbPin1, TbPin2, TbPin3, TbPin4 };
            TbPin1.Focus(); // Устанавливаем начальный фокус
        }

        /// <summary>
        /// Свойство для получения введенного PIN-кода
        /// </summary>
        public string PinCode
        {
            get => string.Join("", pinBoxes.Select(box => box.Password));
        }

        /// <summary>
        /// Очищает все поля ввода и устанавливает фокус на первое поле
        /// </summary>
        public void Clear()
        {
            foreach (var box in pinBoxes)
            {
                box.Password = string.Empty;
            }
            TbPin1.Focus();
        }

        private void Pin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var currentBox = sender as PasswordBox;
            int currentIndex = pinBoxes.ToList().IndexOf(currentBox);

            // Автоматический переход к следующему полю при вводе 1 символа
            if (currentBox.Password.Length == 1)
            {
                if (currentIndex < pinBoxes.Length - 1)
                {
                    pinBoxes[currentIndex + 1].Focus();
                }
                else if (currentIndex == pinBoxes.Length - 1)
                {
                    // Если введен последний символ, вызываем событие завершения
                    PinCompleted?.Invoke(PinCode);
                }
            }
        }

        private void Pin_KeyDown(object sender, KeyEventArgs e)
        {
            var currentBox = sender as PasswordBox;
            int currentIndex = pinBoxes.ToList().IndexOf(currentBox);

            // 1. Обработка Backspace для перемещения фокуса назад
            if (e.Key == Key.Back && currentBox.Password.Length == 0 && currentIndex > 0)
            {
                // Перемещаем фокус на предыдущий блок и удаляем из него символ
                pinBoxes[currentIndex - 1].Focus();
                pinBoxes[currentIndex - 1].Password = string.Empty;
                e.Handled = true; // Предотвращаем дальнейшую обработку
            }
            // 2. Обработка ввода только цифр
            // Разрешаем: цифры (D0-D9, NumPad0-NumPad9), Backspace, Delete, стрелки, Tab
            else if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                     && e.Key != Key.Back && e.Key != Key.Delete && e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Tab)
            {
                e.Handled = true; // Блокируем ввод не-цифр
            }
        }
    }
}