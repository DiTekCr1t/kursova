using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;


namespace kursova2._0
{
    public partial class Form2 : Form
    {
        public event Action<string, string, string, string, string> DataAdded;

        // Конструктор, который принимает соединение с базой данных
        public Form2()
        {
            InitializeComponent();
        }


        private void Add_button_Click(object sender, EventArgs e)
        {
            // Проверка на заполнение всех текстовых полей
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Будь ласка, заповніть усі поля перед збереженням.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Получение значений из текстовых полей
            string str1 = textBox1.Text;
            string str2 = textBox2.Text;
            string str3 = textBox3.Text;
            string str4 = textBox4.Text;
            string str5 = textBox5.Text;

            // Вызов события DataAdded
            DataAdded?.Invoke(str1, str2, str3, str4, str5);

            // Отображение сообщения об успешном сохранении
            MessageBox.Show("Дані успішно збережено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Закрытие формы
            this.Close();
        }

    }
}
