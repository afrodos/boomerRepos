using System;
using System.Management;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace login
{
    public partial class Form1 : Form
    {
        private MySqlConnection conn;
        private string server;
        private string database;
        private string uid;
        private string password;


        public Form1()
        {
            InitializeComponent();
            getHWID();
            server = "sql2.freemysqlhosting.net";
            database = "sql2231671";
            uid = "sql2231671";
            password = "wK5*lI7!";

            string connstring;
            connstring = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";
            conn = new MySqlConnection(connstring);
        }

        public void getHWID()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            textBox1.Text = cpuInfo;
        }


        public bool islogin(string hwid)
        {
            string hwidpass = textBox1.Text;
            string query = $"SELECT * FROM users WHERE hwid= '{hwidpass}';";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        reader.Close();
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        reader.Close();
                        conn.Close();
                        return false;
                    }
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string hwidpass = textBox1.Text;

            if (Register(hwidpass))
            {
                MessageBox.Show($"User {hwidpass} has been created");
            }
            else
            {
                MessageBox.Show($"User has not been created");
            }

        }
        public bool Register(string hwidpass)
        {
            string query = $"INSERT INTO users (id, hwid) VALUES ('','{hwidpass}');";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    conn.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                return false;
            }
        }
        public bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Connection failed.");
                        break;
                    case 1045:
                        MessageBox.Show("invalid username or password.");
                        break;
                }
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string hwidpass = textBox1.Text;

            if (islogin(hwidpass))
            {
                MessageBox.Show($"Welcome!");
            }
            else
            {
                MessageBox.Show($"Incorrect password or account does not exist");
            }
        }
    }
}
