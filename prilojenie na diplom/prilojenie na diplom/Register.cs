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
    public partial class Register : Form
    {
        DB dataBase = new DB();
        public Register()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkUser())
            {
                return;
            }
            
                var login = loginField.Text;
            var password = Hash.hashPassword(passField.Text);

            string querystring = $"insert into register(login_user, password_user, is_admin) values('{login}','{password}', 0 )";
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            dataBase.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Успешно зарег", " Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                autoriz frm = new autoriz();
                this.Hide();
                frm.ShowDialog();
            }
            else
                MessageBox.Show("Не зарегистрировано", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            dataBase.closeConnection();
        }
            
        
    
       private bool checkUser()
        {
            var loginUser = loginField.Text;
            var passUser = passField.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user, login_user, password_user, is_admin from register where login_user ='{loginUser}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь уже существует");
                return true;
            }
            else
            {
             
                return false;
            }
        }
    

        private void closeButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void closeButton2_MouseEnter(object sender, EventArgs e)
        {
            closeButton2.ForeColor = Color.Red;
        }

        private void closeButton2_MouseLeave(object sender, EventArgs e)
        {
            closeButton2.ForeColor = Color.White;
        }
        Point lastPoint;
        private void Register_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void Register_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void clearButton2_Click(object sender, EventArgs e)
        {
            loginField.Text = "";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            passField.Text = "";
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           autoriz form = new autoriz();
            form.Show();
            this.Hide();
        }
    }
    }

