
using System.Net;
using System.Net.Mail;

namespace pr6.Classes
{
    public class SendMail
    {
        /// <summary>
        /// Функция отправки сообщения
        /// </summary>
        /// <param name="Message">Сообщение которое необходимо отправить</param>
        /// <param name="To">Почта на которую отправляется сообщение</param>
        public static void SendMessage(string Message, string To)
        {
            var smtpClient = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("motisho@yandex.ru", "pgzusfqoyjntibvk"),
                EnableSsl = true,
            };
            smtpClient.Send("motisho@yandex.ru", To, "Проект RegIn", Message);
        }
    }
}