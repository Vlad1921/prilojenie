using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace prilojenie_na_diplom
{
    public partial class autoriz : Form
    {
        DB dataBase = new DB();
        public autoriz()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.passField.AutoSize = false;
            this.passField.Size = new Size(this.passField.Size.Width, 27);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = loginField.Text;
            var passUser = Hash.hashPassword(passField.Text);
            // 2 объекта для работы с бд
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable datatable = new DataTable();
            // создание строки запроса
            string querystring = $"select id_user, login_user, password_user, is_admin from register " + $"where login_user='{loginUser}' and password_user='{passUser}'";
            // объект для того чтобы это все работало - передаем в строку и подключение
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            //работа с адаптером
            adapter.SelectCommand = command;
            adapter.Fill(datatable);

            if (datatable.Rows.Count == 1)
            {
                var user = new checkUser1(datatable.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(datatable.Rows[0].ItemArray[3]));

                MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form2 form2 = new Form2(user);
                this.Hide();
                form2.ShowDialog();
                this.Show();
            }
            else if (loginField.Text == "" || loginField.Text == "" || passField.Text == "")

                MessageBox.Show("Заполнены не все поля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (datatable.Rows.Count == 0) 
          MessageBox.Show("Аккаунт не найден!", "Аккаунт не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void closeButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeButton_MouseEnter(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.Red;
        }

        private void closeButton_MouseLeave(object sender, EventArgs e)
        {
            closeButton.ForeColor = Color.White;
        }
        Point lastPoint;
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register press = new Register();
            press.Show();
            this.Hide();
        }

        private void clearButton1_Click(object sender, EventArgs e)
        {
            loginField.Text = "";
            
        }

        private void clearButton2_Click(object sender, EventArgs e)
        {
            passField.Text = "";
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }
    }
    }
    
    

