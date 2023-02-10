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
    public partial class Add : Form
    {
        DB dataBase = new DB();
        public Add()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataBase.openConnection();

            var type = textBox_type2.Text;
            var count = textBox_count2.Text;
            var tarif = textBox_tarif2.Text;
            int price;

            if (int.TryParse(textBox_price2.Text, out price))
            {
                var addQuery = $"insert into test_db3 (type_of, count_of, tarif, price) values('{type}', '{count}', '{tarif}', '{price}')";

                var command = new SqlCommand(addQuery, dataBase.getConnection());
                command.ExecuteNonQuery();

                MessageBox.Show("Запись успешно создана!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Цена должна иметь числовой формат!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            dataBase.closeConnection();
        }
    }
}
