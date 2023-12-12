﻿using MySql.Data.MySqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace WinFormsApp1
{
    public partial class ChangePass : Form
    {
        public ChangePass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            DataTable dt = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `email`=@uE", db.getConnection());
            command.Parameters.Add("@uE", MySqlDbType.VarChar).Value = emailBox.Text;

            adapter.SelectCommand = command;
            adapter.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                code = SendMessage(emailBox.Text);
                MessageBox.Show("Код отправлен на почту");
            }
            else
                MessageBox.Show("Почта не найдена");
        }
        public int code;

        public int SendMessage(string adressTo)
        {
            emailBox.Enabled = false;
            try
            {
                string from = @"schoolhelper634@gmail.com"; // адреса отправителя
                string pass = "ewlr vryt jiey qyqa"; // пароль отправителя
                MailMessage mess = new MailMessage();
                mess.To.Add(adressTo); // адрес получателя
                mess.From = new MailAddress(from);
                mess.Subject = "Одноразовый код для смены пароля"; // тема
                var r = new Random();
                var code = r.Next(100000, 999999);
                mess.Body = code.ToString(); // текст сообщения
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com"; //smtp-сервер отправителя
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], pass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mess); // отправка пользователю

                return code;
            }
            catch
            {
                MessageBox.Show("Ошибка отправки");
                return 0;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (int.Parse(CodeBox.Text) == code && code != 0)
            {
                DB db = new DB();
                DataTable dt = new DataTable();
                MySqlCommand command = new MySqlCommand("UPDATE `users` SET `password` = @uP WHERE `users`.`email` LIKE @email", db.getConnection());
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = emailBox.Text;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = newPass.Text;
                db.openConnection();
                MessageBox.Show("Пароль успешно изменен");
                command.ExecuteNonQuery();
                db.closeConnection();
                Thread.Sleep(500);
                var form = new LoginForm();
                form.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный код подтверждения");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new LoginForm();
            form.Show();
            this.Close();
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowPassword.Checked == false)
                newPass.UseSystemPasswordChar = true;
            if (ShowPassword.Checked == true)
                newPass.UseSystemPasswordChar = false;
        }

        private void ChangePass_Load(object sender, EventArgs e)
        {

        }
    }
}
