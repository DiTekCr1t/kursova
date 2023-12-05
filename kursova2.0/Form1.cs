using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace kursova2._0
{
    public partial class Form1 : Form
    {
        private SqlConnection dbConnection;
        private SqlDataAdapter adapter;
        private SqlCommandBuilder builder;
        private ToolTip toolTip1;
        private DataTable originalDataTableBeforeSort;
        private DataTable StudentsDataTable;
        private DataTable originalDataTableForSorting;
        private string originalSortString;
        private TextBox textBoxSearch;
        private string databaseFileName = "Database1.mdf";
        private string connectionString;
        public Form1()
        {
            InitializeComponent();
            toolTip1 = new ToolTip();
            // Установка подсказки для кнопки button1
            toolTip1.SetToolTip(button1, "Натисніть, щоб зберегти дані");

            // Добавление других кнопок и соответствующих подсказок
            toolTip1.SetToolTip(button2, "Натисніть, щоб видалити вибрану строчку");
            toolTip1.SetToolTip(button3, "Натисніть, щоб закрити форму");
            toolTip1.SetToolTip(button4, "Натисніть, щоб отримати статистику присутніх та відсутніх студентів");
            toolTip1.SetToolTip(button6, "Натисніть, щоб відкрити інформацію про виробника");
            toolTip1.SetToolTip(button8, "Натисніть, щоб скинулася фільтрація");
            toolTip1.SetToolTip(pictureBox1, "Натисніть, щоб відкрити інформацію");

            // Connect to the database
            string projectFolder = Environment.CurrentDirectory;
            string databasePath = Path.Combine(projectFolder, databaseFileName);

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True;Connect Timeout=30";
            dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            // Load data from the database into the DataGridView
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Students", dbConnection);
            StudentsDataTable = new DataTable();
            adapter.Fill(StudentsDataTable);
            dataGridView1.DataSource = StudentsDataTable;
            originalDataTableForSorting = StudentsDataTable.Copy();
            originalSortString = string.Empty;
        }

        
        public static int CountPresentStudents(DataTable dt)
        {
            int presentCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Attendance"].ToString() == "Present")
                {
                    presentCount++;
                }
            }

            return presentCount;
        }




        private void button1_Click(object sender, EventArgs e)
        {
            // Get the data from the DataGridView
            DataTable dt = (DataTable)dataGridView1.DataSource;

            // Add any new rows to the DataTable


            string tableName = "Students";

            if (!string.IsNullOrEmpty(tableName))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + tableName, dbConnection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update((DataTable)dataGridView1.DataSource);
            }

            MessageBox.Show("Дані успішно збережено!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Delete the selected row from the database
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the StudentID of the selected row
                int studentID = (int)dataGridView1.SelectedRows[0].Cells["StudentID"].Value;

                // Prepare the SQL DELETE statement
                string deleteQuery = "DELETE FROM Students WHERE StudentID = @StudentID";

                // Create a SqlCommand object
                SqlCommand command = new SqlCommand(deleteQuery, dbConnection);

                // Add parameters to the SqlCommand object
                command.Parameters.AddWithValue("@StudentID", studentID);

                // Execute the DELETE statement
                int rowsAffected = command.ExecuteNonQuery();

                // Check if the delete was successful
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Дані успішно видалено!");
                    // Refresh the DataGridView
                    string query = "SELECT * FROM Students";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, dbConnection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("Помилка видалення даних.");
                }
            }
            else
            {
                MessageBox.Show("Виберіть рядок для видалення.");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the data from the DataGridView
            DataTable dt = (DataTable)dataGridView1.DataSource;

            // Get the StudentID of the selected row
            int studentID = (int)dt.Rows[e.RowIndex]["StudentID"];

            // Update the presence of the student
            string updateQuery = "UPDATE Students SET Presence = 'Present' WHERE StudentID = @StudentID";

            // Create a SqlCommand object
            SqlCommand command = new SqlCommand(updateQuery, dbConnection);

            // Add parameters to the SqlCommand object
            command.Parameters.AddWithValue("@StudentID", studentID);

            // Execute the UPDATE statement
            int rowsAffected = command.ExecuteNonQuery();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Close the form
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }




        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            // Подсчитайте количество присутствующих и отсутствующих студентов
            int presentCount = 0;
            int absentCount = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Presence"].ToString() == "Present")
                {
                    presentCount++;
                }
                else
                {
                    absentCount++;
                }
            }

            // Выведите результат в MessageBox на украинском языке
            MessageBox.Show(
                $"Присутні студенти: {presentCount}\nВідсутні студенти: {absentCount}",
                "Кількість студентів",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        private void SaveAttendanceData()
        {
            string tableName = "Attendance";

            if (!string.IsNullOrEmpty(tableName))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + tableName, dbConnection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.Update((DataTable)dataGridView1.DataSource);
            }

            MessageBox.Show("Дані про відвідуваність збережено!");
        }
        private void SaveAttendanceButton_Click(object sender, EventArgs e)
        {
            SaveAttendanceData();
        }
        

        private void button6_Click(object sender, EventArgs e)
        {
            // Создайте новый экземпляр Form3
            Form3 form3 = new Form3();

            // Покажите Form3
            form3.Show();
        }

       


        private void button8_Click(object sender, EventArgs e)
        {
            // Создайте соединение с базой данных "Students"
            string projectFolder = Environment.CurrentDirectory;
            string databasePath = Path.Combine(projectFolder, databaseFileName);

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True;Connect Timeout=30";
            using (SqlConnection studentsConnection = new SqlConnection(connectionString))
            {
                studentsConnection.Open();

                // Создайте SQL-запрос для выборки данных из таблицы "Students"
                string selectQuery = "SELECT * FROM Students";
                SqlDataAdapter studentsAdapter = new SqlDataAdapter(selectQuery, studentsConnection);
                DataTable studentsDt = new DataTable();

                // Заполните DataTable данными из базы данных
                studentsAdapter.Fill(studentsDt);

                // Установите DataGridView как источник данных для DataTable
                dataGridView1.DataSource = studentsDt;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Создайте новую форму Form5
            Form4 infoForm = new Form4();

            // Установите текст для TextBox на Form5
            string infoText = "Інформація!!!\r\n Кнопка зберегти виконує збереження бази даних у проекті \r\n Кнопка вийти робить закриття журналу \r\n Кнопка видалення виконує видалення певного рядка в базі даних \r\n Кнопка Інформація надає інформацію про розробника \r\n Кнопка присутні показує присутніх студентів \r\n Кнопка відсутні показує відсутніх студентів \r\n Кнопка студенти показує базу даних, де знаходиться інформація про студента \r\n Кнопка скасування виконує повернення на головну сторінку \r\n ";
            infoForm.SetInfoText(infoText);

            // Покажите Form5 как диалоговое окно
            infoForm.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string projectFolder = Environment.CurrentDirectory;
            string databasePath = Path.Combine(projectFolder, databaseFileName);

            string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True;Connect Timeout=30";
            SqlConnection connection = new SqlConnection(connectionString);

            // Открыть соединение
            connection.Open();

            // Создать запрос
            string query = "SELECT COUNT(*) AS TotalStudents FROM Students";

            // Выполнить запрос
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            System.Console.WriteLine(reader.ToString());
            // Получить результат
            if (reader.Read())
            {
                // Преобразовать значение в тип int
                int totalStudents = Convert.ToInt32(reader["TotalStudents"]);

                // Показать результат
                MessageBox.Show($"Количество студентов: {totalStudents}");
            }
            
            // Закрыть соединение
            connection.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ApplyFilter("Presence = 'Present'");
        }
        private void ApplyFilter(string filterExpression)
        {
            DataView dv = new DataView(StudentsDataTable);
            dv.RowFilter = filterExpression;
            dataGridView1.DataSource = dv;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ApplyFilter("Presence = 'Absent'");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ResetFilter();
        }
        private void ResetFilter()
        {
            StudentsDataTable = originalDataTableBeforeSort.Copy();

            if (dataGridView1.SortedColumn != null)
            {
                originalDataTableForSorting.DefaultView.Sort = originalSortString;
                dataGridView1.DataSource = originalDataTableForSorting;
                StudentsDataTable = originalDataTableBeforeSort.Copy();
            }
            else
            {
                dataGridView1.DataSource = StudentsDataTable;
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();

            DataTable dataTable = ((DataTable)dataGridView1.DataSource).Copy();
            string filterExpression = string.Join(" OR ", dataTable.Columns.Cast<DataColumn>().Select(column => $"CONVERT({column.ColumnName}, System.String) LIKE '%{searchText}%'"));

            dataTable.DefaultView.RowFilter = filterExpression;

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                dataGridView1.DataSource = dataTable.DefaultView.ToTable();
            }
            else
            {
                string query = "SELECT * FROM Students";
                SqlDataAdapter adapter = new SqlDataAdapter(query, dbConnection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        private void SearchByLastName(string lastName)
        {
            // Используйте DataView для фильтрации данных по фамилии
            DataView dv = new DataView(originalDataTableBeforeSort);
            dv.RowFilter = $"[ПІБ_Студента] LIKE '%{lastName}%'";
            dataGridView1.DataSource = dv;

            // Очистите текстовое поле поиска, если нужно
            textBox1.Clear();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            // Создаем новый экземпляр Form2, передавая ему текущее соединение с базой данных
            Form2 form2 = new Form2();
            form2.Show();

            form2.DataAdded += (StudentID, FirstName, LastName, Groupp, Presence) =>
            {
                string projectFolder = Environment.CurrentDirectory;
                string databasePath = Path.Combine(projectFolder, databaseFileName);

                string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={databasePath};Integrated Security=True;Connect Timeout=30";
                using (SqlConnection sqlConnect = new SqlConnection(connectionString))
                {
                    sqlConnect.Open();

                    string query = $"SET LANGUAGE RUSSIAN; INSERT INTO Students (StudentID, FirstName, LastName, Groupp, Presence) VALUES (N'{StudentID}', N'{FirstName}', N'{LastName}', N'{Groupp}', N'{Presence}')";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnect);
                    try
                    {
                        // Додайте новий рядок з отриманими значеннями до існуючого DataTable
                        StudentsDataTable.Rows.Add(StudentID, FirstName, LastName, Groupp, Presence);
                    }
                    catch (Exception exs)
                    {
                        MessageBox.Show(exs.Message);
                    }
                }

            };

        }
    }
}

