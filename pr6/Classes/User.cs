using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace pr6.Classes
{
    public class User
    {
        public int Id { get; set; } = -1;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public byte[] Image = new byte[0];
        public DateTime DateUpdate { get; set; }
        public DateTime DateCreate { get; set; }
        public string PinCode { get; set; } = string.Empty;

        public CorrectLogin HandlerCorrectLogin;
        public InCorrectLogin HandlerInCorrectLogin;

        public delegate void CorrectLogin();
        public delegate void InCorrectLogin();

        void ReadUserFromReader(MySqlDataReader reader)
        {
            Id = reader.GetInt32("Id");
            Login = reader.GetString("Login");
            Password = reader.GetString("Password");
            Name = reader.GetString("Name");

            int imageOrdinal = reader.GetOrdinal("Image");
            if (!reader.IsDBNull(imageOrdinal))
            {
                long length = reader.GetBytes(imageOrdinal, 0, null, 0, 0);
                if (length > 0)
                {
                    Image = new byte[length];
                    reader.GetBytes(imageOrdinal, 0, Image, 0, (int)length);
                }
                else
                {
                    Image = new byte[0];
                }
            }
            else
            {
                Image = new byte[0];
            }

            DateUpdate = reader.GetDateTime("DateUpdate");
            DateCreate = reader.GetDateTime("DateCreate");

            int pinOrdinal = reader.GetOrdinal("PinCode");
            if (!reader.IsDBNull(pinOrdinal))
                PinCode = reader.GetString(pinOrdinal);
            else
                PinCode = string.Empty;
        }

        /// <summary>
        /// Получение данных пользователя по логину (защищённый запрос)
        /// </summary>
        public void GetUserLogin(string login)
        {
            Id = -1;
            Login = Password = Name = string.Empty;
            Image = new byte[0];
            PinCode = string.Empty;

            using (var conn = WorkingDB.OpenConnection())
            {
                if (!WorkingDB.OpenConnection(conn))
                {
                    HandlerInCorrectLogin?.Invoke();
                    return;
                }

                using (var cmd = new MySqlCommand("SELECT * FROM users WHERE Login = @login LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ReadUserFromReader(reader);
                            HandlerCorrectLogin?.Invoke();
                            return;
                        }
                    }
                }
            }
            HandlerInCorrectLogin?.Invoke();
        }

        /// <summary>
        /// Сохранение нового пользователя (параметризованный INSERT)
        /// </summary>
        public void SetUser()
        {
            using (var conn = WorkingDB.OpenConnection())
            {
                if (!WorkingDB.OpenConnection(conn)) return;

                using (var cmd = new MySqlCommand(
                    "INSERT INTO users (Login, Password, Name, Image, DateUpdate, DateCreate, PinCode) " +
                    "VALUES (@Login, @Password, @Name, @Image, @DateUpdate, @DateCreate, @PinCode); SELECT LAST_INSERT_ID();", conn))
                {
                    cmd.Parameters.AddWithValue("@Login", Login);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Image", Image);
                    cmd.Parameters.AddWithValue("@DateUpdate", DateUpdate);
                    cmd.Parameters.AddWithValue("@DateCreate", DateCreate);
                    cmd.Parameters.AddWithValue("@PinCode", PinCode);

                    Id = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Сгенерировать и отправить новый пароль
        /// </summary>
        public void CrateNewPassword()
        {
            if (string.IsNullOrEmpty(Login))
                return;

            Password = GeneratePass();
            using (var conn = WorkingDB.OpenConnection())
            {
                if (!WorkingDB.OpenConnection(conn)) return;

                using (var cmd = new MySqlCommand("UPDATE users SET Password = @pwd WHERE Login = @login", conn))
                {
                    cmd.Parameters.AddWithValue("@pwd", Password);
                    cmd.Parameters.AddWithValue("@login", Login);
                    cmd.ExecuteNonQuery();
                }
            }
            SendMail.SendMessage($"Your account password has been changed. Password: {Password}", Login);
        }

        /// <summary>
        /// Вход по PIN (безопасный параметризованный запрос)
        /// </summary>
        public void LoginByPin(string pin)
        {
            Id = -1;
            Login = Password = Name = string.Empty;
            Image = new byte[0];
            PinCode = string.Empty;

            using (var conn = WorkingDB.OpenConnection())
            {
                if (!WorkingDB.OpenConnection(conn))
                {
                    HandlerInCorrectLogin?.Invoke();
                    return;
                }

                using (var cmd = new MySqlCommand("SELECT * FROM users WHERE PinCode = @pin LIMIT 1", conn))
                {
                    cmd.Parameters.AddWithValue("@pin", pin);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ReadUserFromReader(reader);
                            HandlerCorrectLogin?.Invoke();
                            return;
                        }
                    }
                }
            }
            HandlerInCorrectLogin?.Invoke();
        }

        /// <summary>
        /// Сохранение PIN пользователя в БД
        /// </summary>
        public void SavePin(string pin)
        {
            if (Id <= 0) return;

            using (var conn = WorkingDB.OpenConnection())
            {
                if (!WorkingDB.OpenConnection(conn)) return;

                using (var cmd = new MySqlCommand("UPDATE users SET PinCode = @pin WHERE Id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@pin", pin);
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                    PinCode = pin;
                }
            }
        }

        /// <summary>
        /// Генерация случайного пароля (упрощённо и безопасно)
        /// </summary>
        public string GeneratePass()
        {
            var rnd = new Random();
            var chars = "0123456789-.,!?*()_+ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var sb = new StringBuilder(10);
            for (int i = 0; i < 10; i++)
                sb.Append(chars[rnd.Next(chars.Length)]);
            return sb.ToString();
        }
    }
}