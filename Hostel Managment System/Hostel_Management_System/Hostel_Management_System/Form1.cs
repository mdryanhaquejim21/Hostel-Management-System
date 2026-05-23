using Hostel_Management_System.Database;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
            txtPassword.UseSystemPasswordChar = true;

            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
        }

        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !checkBox1.Checked;
        }

        
        private void btn_LogIn_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (txtUsername.Text == "" || txtPassword.Text == "")
                {
                    MessageBox.Show("Please Enter Username and Password");
                    return;
                }

                using (SqlConnection con = DB.GetConnection())
                {
                    string query = @"
                        SELECT r.RoleName
                        FROM UserInfo u
                        INNER JOIN Roles r ON u.RoleId = r.RoleId
                        WHERE u.Username = @u AND u.Password = @p";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string role = result.ToString().Trim();

                        MessageBox.Show("Login Successful");

                        Form3 dashboard = new Form3(role);
                        dashboard.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        
        private void btn_SignUp_Click(object sender, EventArgs e)
        {
            Form2 signup = new Form2();
            signup.Show();
            this.Hide();
        }
    }
}