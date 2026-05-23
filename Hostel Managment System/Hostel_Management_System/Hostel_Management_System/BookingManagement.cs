using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class BookingManagement : Form
    {
        Form3 dashboard;
        string role;

        public BookingManagement(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            this.FormClosed += BookingManagement_FormClosed;
            this.Activated += BookingManagement_Activated;

            LoadStatus();

            if (role == "Student")
            {
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                cmbStatus.Enabled = false;
            }
        }

        
        private void BookingManagement_Activated(object sender, EventArgs e)
        {
            LoadBookings();
        }

        
        private void BookingManagement_FormClosed(
            object sender,
            FormClosedEventArgs e)
        {
            dashboard.RefreshDashboardData();
            dashboard.Show();
        }

        
        private void LoadStatus()
        {
            cmbStatus.Items.Clear();

            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Approved");
            cmbStatus.Items.Add("Rejected");
        }

        
        private void LoadBookings()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(@"
                    SELECT
                        RequestId,
                        StudentId,
                        RequestedRoomType,
                        RequestDate,
                        RequestStatus
                    FROM RequestRoom", con);

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

        }

        
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Select a request first!");
                    return;
                }

                int requestId = Convert.ToInt32(
                    dataGridView1.CurrentRow.Cells["RequestId"].Value);

                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    string query = @"
                        UPDATE RequestRoom
                        SET
                            StudentId = @sid,
                            RequestStatus = @status
                        WHERE RequestId = @id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@id", requestId);
                    cmd.Parameters.AddWithValue("@sid", txtStudentId.Text);
                    cmd.Parameters.AddWithValue("@status", cmbStatus.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Updated Successfully");

                    LoadBookings();

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
                    MessageBox.Show("Select a request first!");
                    return;
                }

                int requestId = Convert.ToInt32(
                    dataGridView1.CurrentRow.Cells["RequestId"].Value);

                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    string query =
                        "DELETE FROM RequestRoom WHERE RequestId=@id";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@id", requestId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Deleted Successfully");

                    LoadBookings();

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
                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    string query = @"
                        SELECT *
                        FROM RequestRoom
                        WHERE
                            CAST(RequestId AS NVARCHAR) LIKE @s
                            OR CAST(StudentId AS NVARCHAR) LIKE @s
                            OR RequestedRoomType LIKE @s
                            OR RequestStatus LIKE @s";

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue(
                        "@s",
                        "%" + txtSearch.Text + "%");

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

        
        private void dataGridView1_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row =
                    dataGridView1.Rows[e.RowIndex];

                txtStudentId.Text =
                    row.Cells["StudentId"].Value.ToString();

                cmbStatus.Text =
                    row.Cells["RequestStatus"].Value.ToString();
            }
        }

        
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();

            dataGridView1.ClearSelection();

            txtStudentId.Focus();
        }

        
        private void ClearFields()
        {
            txtStudentId.Clear();

            txtSearch.Clear();

            cmbStatus.SelectedIndex = -1;
        }

        
        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();

            login.Show();

            this.Hide();
        }
    }
}