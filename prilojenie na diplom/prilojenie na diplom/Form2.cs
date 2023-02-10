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
    enum RowState
    {
        Existed,
        New,
        Modified,
        ModifiendNew,
        Deleted
    }
    public partial class Form2 : Form
    {
        private readonly checkUser1 _user;
        DB dataBase = new DB();

        int selectedRow;

      public Form2(checkUser1 user)
        { 
            _user = user;
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private void IsAdmin()
        {
            управлениеToolStripMenuItem.Enabled = _user.IsAdmin;

        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("type_of", "Тип товара");
            dataGridView1.Columns.Add("count_of", "Количество");
            dataGridView1.Columns.Add("Tarif", "Тариф");
            dataGridView1.Columns.Add("price", "Цена");
            dataGridView1.Columns.Add("IsNew", String.Empty);

        }
        private void ClearField()
        {
            textBox_id.Text = "";
            textBox_type.Text = "";
            textBox_count.Text = "";
            textBox_tarif.Text = "";
            textBox_price.Text = "";


        }

        private void ReadSingleRow(DataGridView dgw,IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetInt32(2), record.GetString(3), record.GetInt32(4), RowState.ModifiendNew);
        } 
        
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string quertyString = $"select * from test_db3";
            SqlCommand command = new SqlCommand(quertyString, dataBase.getConnection());

            dataBase.openConnection();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw,reader);
            }
            reader.Close();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            
            tlsUserStatus.Text = $"{_user.Login}: {_user.Status}";
            IsAdmin();
            CreateColumns();
            RefreshDataGrid(dataGridView1);

        }

       

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                textBox_id.Text = row.Cells[0].Value.ToString();
                textBox_type.Text = row.Cells[1].Value.ToString();
                textBox_count.Text = row.Cells[2].Value.ToString();
                textBox_tarif.Text = row.Cells[3].Value.ToString();
                textBox_price.Text = row.Cells[4].Value.ToString(); 
            }
        }

        private void pictureRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
            ClearField();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add addform = new Add();
            addform.Show();
        }



        private void Search(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string searchString = $"select * from test_db3 where concat (id, type_of, count_of, tarif, price) like '%" + textBox_search.Text + "%'";
            SqlCommand com = new SqlCommand(searchString, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                ReadSingleRow(dgw, read);
            }
            read.Close();
        }

        private void deleteRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;
                return;
            }
            dataGridView1.Rows[index].Cells[5].Value = RowState.Deleted;

        }
        private void Update()
        {
            dataBase.openConnection();

            for(int index = 0; index < dataGridView1.RowCount; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[5].Value;

                if (rowState == RowState.Existed)
                    continue;

                if(rowState == RowState.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from test_db3 where id = {id}";

                    var command = new SqlCommand(deleteQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
                if(rowState == RowState.Modified)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var type = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var count = dataGridView1.Rows[index].Cells[2].Value.ToString();
                    var tarif = dataGridView1.Rows[index].Cells[3].Value.ToString();
                    var price = dataGridView1.Rows[index].Cells[4].Value.ToString();

                    var changeQuery = $"update test_db3 set type_of ='{type}', count_of = '{count}', tarif = '{tarif}', price '{price}' where id = '{id}'";

                    var command = new SqlCommand(changeQuery, dataBase.getConnection());
                    command.ExecuteNonQuery();
                }
            }
            dataBase.closeConnection();
        }
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            deleteRow();
            ClearField();
        }

        private void button_save1_Click(object sender, EventArgs e)
        {
            Update();
        }


        private void Change()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = textBox_id.Text;
            var type = textBox_type.Text;
            var count = textBox_count.Text;
            var tarif = textBox_tarif.Text;
            int price;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString()!= string.Empty)
            {
                if(int.TryParse(textBox_price.Text, out price))
                {
                    dataGridView1.Rows[selectedRowIndex].SetValues(id, type, count, tarif, price);
                    dataGridView1.Rows[selectedRowIndex].Cells[5].Value = RowState.Modified;
                }
                else
                {
                    MessageBox.Show("Цена должна иметь числовой формат!");
                }
            }
        }
        private void btnChange_Click(object sender, EventArgs e)
        {
            Change();
            ClearField();
        }

        private void pictureEraser3_Click(object sender, EventArgs e)
        {
            ClearField();
        }
    }
}
