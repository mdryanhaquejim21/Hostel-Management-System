using Hostel_Management_System.Database;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

            comboBox1.Items.Clear();
            comboBox1.Items.Add("Admin");
            comboBox1.Items.Add("Staff");
            comboBox1.Items.Add("Student");

            txtPassword.UseSystemPasswordChar = true;

            this.FormClosed += Form2_FormClosed;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
        }

        private void btn_SignUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text == "" || txtPassword.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Fill all fields");
                    return;
                }

                if (!radioButton1.Checked && !radioButton2.Checked)
                {
                    MessageBox.Show("Select Gender");
                    return;
                }

                string gender = radioButton1.Checked ? "Male" : "Female";
                string role = comboBox1.SelectedItem.ToString();

                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    
                    string userQuery = @"
            INSERT INTO UserInfo
            (Username, Password, Email, Phone, Gender, RoleId)
            VALUES
            (@u, @p, @e, @ph, @g,
            (SELECT RoleId FROM Roles WHERE RoleName=@r));
            
            SELECT SCOPE_IDENTITY();";

                    SqlCommand cmd = new SqlCommand(userQuery, con);

                    cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());
                    cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@ph", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@g", gender);
                    cmd.Parameters.AddWithValue("@r", role);

                    int newUserId = Convert.ToInt32(cmd.ExecuteScalar());

                    

                    if (role == "Student")
                    {
                        SqlCommand stuCmd = new SqlCommand(@"
                INSERT INTO Students
                (StudentName, Department, Session, Address, PhoneNo)
                VALUES (@name, 'Not Set', 'Not Set', 'Not Set', @phone)", con);

                        stuCmd.Parameters.AddWithValue("@name", txtUsername.Text.Trim());
                        stuCmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());

                        stuCmd.ExecuteNonQuery();
                    }

                    else if (role == "Staff")
                    {
                        SqlCommand staffCmd = new SqlCommand(@"
                INSERT INTO Staff
                (FullName, Designation, Salary, JoinDate)
                VALUES (@name, 'New Staff', 0, GETDATE())", con);

                        staffCmd.Parameters.AddWithValue("@name", txtUsername.Text.Trim());

                        staffCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Signup Successful");

                new Form1().Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}