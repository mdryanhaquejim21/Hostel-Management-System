using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class StaffManagement : Form
    {
        string role;
        Form3 dashboard;

        public StaffManagement(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            this.FormClosed += StaffManagement_FormClosed;
            this.Activated += StaffManagement_Activated; 
        }

        
        private void StaffManagement_Load(object sender, EventArgs e)
        {
            LoadDesignation();
            LoadStaff();
        }

        
        private void StaffManagement_Activated(object sender, EventArgs e)
        {
            LoadStaff(); 
        }

        
        private void StaffManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboard.RefreshDashboardData();
            dashboard.Show();
        }

        
        private void LoadDesignation()
        {
            comboDesignation.Items.Clear();
            comboDesignation.Items.Add("Manager");
            comboDesignation.Items.Add("Accountant");
            comboDesignation.Items.Add("Security");
            comboDesignation.Items.Add("Cleaner");
            comboDesignation.Items.Add("Cook");
            comboDesignation.Items.Add("Receptionist");
        }

        
        private void LoadStaff()
        {
            using (SqlConnection conn = DB.GetConnection())
            {
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Staff", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AutoGenerateColumns = true;
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

                    string query = @"INSERT INTO Staff
                                    (FullName, Designation, Salary, JoinDate)
                                    VALUES (@FullName, @Designation, @Salary, @JoinDate)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@FullName", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@Designation", comboDesignation.Text);
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text);
                    cmd.Parameters.AddWithValue("@JoinDate", dtJoinDate.Value.Date);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Staff Added Successfully");

                    LoadStaff();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Select a staff first!");
                    return;
                }

                int staffId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StaffId"].Value);

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Staff 
                                     SET FullName=@FN,
                                         Designation=@DES,
                                         Salary=@SAL,
                                         JoinDate=@JD
                                     WHERE StaffId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ID", staffId);
                    cmd.Parameters.AddWithValue("@FN", txtFullName.Text);
                    cmd.Parameters.AddWithValue("@DES", comboDesignation.Text);
                    cmd.Parameters.AddWithValue("@SAL", txtSalary.Text);
                    cmd.Parameters.AddWithValue("@JD", dtJoinDate.Value.Date);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Updated Successfully");

                    LoadStaff();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Select a staff first!");
                    return;
                }

                int staffId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["StaffId"].Value);

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM Staff WHERE StaffId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", staffId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Deleted Successfully");

                    LoadStaff();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT * FROM Staff
                                     WHERE FullName LIKE @search 
                                     OR Designation LIKE @search";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtFullName.Text = row.Cells["FullName"].Value.ToString();
                comboDesignation.Text = row.Cells["Designation"].Value.ToString();
                txtSalary.Text = row.Cells["Salary"].Value.ToString();
                dtJoinDate.Value = Convert.ToDateTime(row.Cells["JoinDate"].Value);
            }
        }

       
        private bool ValidateInput()
        {
            if (txtFullName.Text == "" ||
                txtSalary.Text == "" ||
                comboDesignation.Text == "")
            {
                MessageBox.Show("Please fill all fields!");
                return false;
            }
            return true;
        }

        
        private void ClearFields()
        {
            txtFullName.Clear();
            txtSalary.Clear();
            comboDesignation.SelectedIndex = -1;
            dtJoinDate.Value = DateTime.Now;
            txtSearch.Clear();
        }

        
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
            dataGridView1.ClearSelection();
            txtFullName.Focus();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}