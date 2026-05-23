using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class StudentManagement : Form
    {
        Form3 dashboard;
        string role;

        public StudentManagement(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            this.FormClosed += StudentManagement_FormClosed;

            
            this.Shown += (s, e) => LoadStudents();
        }

        
        private void StudentManagement_Load(object sender, EventArgs e)
        {
            LoadStudents();
        }

       
        private void StudentManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboard.RefreshDashboardData();
            dashboard.Show();
        }

        
        private void LoadStudents()
        {
            try
            {
                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter(
                        "SELECT StudentId, StudentName, Department, Session, Address, PhoneNo FROM Students",
                        conn);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AutoGenerateColumns = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO Students
                                    (StudentName, Department, Session, Address, PhoneNo)
                                    VALUES
                                    (@Name, @Dept, @Session, @Address, @Phone)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Name", txtStudentName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Dept", comboDepartment.Text);
                    cmd.Parameters.AddWithValue("@Session", txtSession.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhoneNo.Text.Trim());

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Added Successfully");

                    LoadStudents();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Error: " + ex.Message);
            }
        }

        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Select a student first!");
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StudentId"].Value);

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Students
                                     SET StudentName=@Name,
                                         Department=@Dept,
                                         Session=@Session,
                                         Address=@Address,
                                         PhoneNo=@Phone
                                     WHERE StudentId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Name", txtStudentName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Dept", comboDepartment.Text);
                    cmd.Parameters.AddWithValue("@Session", txtSession.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhoneNo.Text.Trim());

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Updated Successfully");

                    LoadStudents();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update Error: " + ex.Message);
            }
        }

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Select a student first!");
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StudentId"].Value);

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM Students WHERE StudentId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Student Deleted Successfully");

                    LoadStudents();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Error: " + ex.Message);
            }
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT * FROM Students
                                     WHERE StudentName LIKE @search
                                     OR Department LIKE @search
                                     OR PhoneNo LIKE @search";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message);
            }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtStudentName.Text = row.Cells["StudentName"].Value.ToString();
                comboDepartment.Text = row.Cells["Department"].Value.ToString();
                txtSession.Text = row.Cells["Session"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
                txtPhoneNo.Text = row.Cells["PhoneNo"].Value.ToString();
            }
        }

        
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

       
        private bool ValidateInput()
        {
            if (txtStudentName.Text == "" ||
                comboDepartment.Text == "" ||
                txtSession.Text == "" ||
                txtAddress.Text == "" ||
                txtPhoneNo.Text == "")
            {
                MessageBox.Show("Please fill all fields!");
                return false;
            }
            return true;
        }

        
        private void ClearFields()
        {
            txtStudentName.Clear();
            comboDepartment.SelectedIndex = -1;
            txtSession.Clear();
            txtAddress.Clear();
            txtPhoneNo.Clear();
            txtSearch.Clear();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}