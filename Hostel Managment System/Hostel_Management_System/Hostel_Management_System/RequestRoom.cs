using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class RequestRoom : Form
    {
        Form3 dashboard;
        string role;

       
        int currentStudentId = 1;

        public RequestRoom(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            this.FormClosed += RequestRoom_FormClosed;
            this.Shown += (s, e) => LoadRequests();

            LoadRoomTypes();
            LoadStatus();

            
            if (role == "Student")
            {
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

                cmbStatus.Text = "Pending";
                cmbStatus.Enabled = false;
            }
            else
            {
                cmbStatus.Enabled = true;
            }
        }

       
        private void RequestRoom_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboard.RefreshDashboardData();
            dashboard.Show();
        }

       
        private void LoadRoomTypes()
        {
            cmbRoomType.Items.Clear();

            cmbRoomType.Items.Add("Luxury");
            cmbRoomType.Items.Add("Standard");
        }

       
        private void LoadStatus()
        {
            cmbStatus.Items.Clear();

            cmbStatus.Items.Add("Pending");
            cmbStatus.Items.Add("Approved");
            cmbStatus.Items.Add("Rejected");
        }

        
        private void LoadRequests()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                SqlCommand cmd;

                
                if (role == "Student")
                {
                    cmd = new SqlCommand(@"
                        SELECT
                            RequestId,
                            StudentId,
                            RequestedRoomType,
                            RequestDate,
                            RequestStatus
                        FROM RequestRoom
                        WHERE StudentId=@sid", con);

                    cmd.Parameters.AddWithValue("@sid", currentStudentId);
                }
                else
                {
                    
                    cmd = new SqlCommand(@"
                        SELECT
                            RequestId,
                            StudentId,
                            RequestedRoomType,
                            RequestDate,
                            RequestStatus
                        FROM RequestRoom", con);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            if (role != "Student")
            {
                MessageBox.Show("Only students can request room!");
                return;
            }

            if (cmbRoomType.Text == "")
            {
                MessageBox.Show("Please select room type!");
                return;
            }

            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO RequestRoom
                    (
                        StudentId,
                        RequestedRoomType,
                        RequestDate,
                        RequestStatus
                    )
                    VALUES
                    (
                        @sid,
                        @type,
                        GETDATE(),
                        @status
                    )", con);

                cmd.Parameters.AddWithValue("@sid", currentStudentId);
                cmd.Parameters.AddWithValue("@type", cmbRoomType.Text);
                cmd.Parameters.AddWithValue("@status", "Pending");

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Room Request Sent Successfully!");

            LoadRequests();
        }

       
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            
            if (role == "Student")
            {
                MessageBox.Show("You are not allowed to update!");
                return;
            }

            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a request!");
                return;
            }

            if (cmbStatus.Text == "")
            {
                MessageBox.Show("Please select status!");
                return;
            }

            int requestId = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["RequestId"].Value);

            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
                    UPDATE RequestRoom
                    SET RequestStatus=@status
                    WHERE RequestId=@id", con);

                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@id", requestId);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Request Status Updated Successfully!");

            LoadRequests();
        }

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (role == "Student")
            {
                MessageBox.Show("You are not allowed!");
                return;
            }

            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0)
            {
                MessageBox.Show("Please select a request!");
                return;
            }

            int requestId = Convert.ToInt32(
                dataGridView1.CurrentRow.Cells["RequestId"].Value);

            DialogResult result = MessageBox.Show(
                "Are you sure to delete?",
                "Confirm",
                MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes)
                return;

            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM RequestRoom WHERE RequestId=@id", con);

                cmd.Parameters.AddWithValue("@id", requestId);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                    MessageBox.Show("Deleted Successfully!");
                else
                    MessageBox.Show("Nothing deleted!");
            }

            LoadRequests();
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                SqlCommand cmd;

                
                if (role == "Student")
                {
                    cmd = new SqlCommand(@"
                        SELECT *
                        FROM RequestRoom
                        WHERE StudentId=@sid
                        AND
                        (
                            CAST(RequestId AS NVARCHAR) LIKE @s
                            OR RequestedRoomType LIKE @s
                            OR RequestStatus LIKE @s
                        )", con);

                    cmd.Parameters.AddWithValue("@sid", currentStudentId);
                }
                else
                {
                    
                    cmd = new SqlCommand(@"
                        SELECT *
                        FROM RequestRoom
                        WHERE
                            CAST(RequestId AS NVARCHAR) LIKE @s
                            OR CAST(StudentId AS NVARCHAR) LIKE @s
                            OR RequestedRoomType LIKE @s
                            OR RequestStatus LIKE @s", con);
                }

                cmd.Parameters.AddWithValue("@s", "%" + txtSearch.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
        }

        
        private void dataGridView1_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtStudentId.Text =
                    dataGridView1.Rows[e.RowIndex]
                    .Cells["StudentId"]
                    .Value.ToString();

                cmbRoomType.Text =
                    dataGridView1.Rows[e.RowIndex]
                    .Cells["RequestedRoomType"]
                    .Value.ToString();

                cmbStatus.Text =
                    dataGridView1.Rows[e.RowIndex]
                    .Cells["RequestStatus"]
                    .Value.ToString();
            }
        }

       
        private void btnNew_Click(object sender, EventArgs e)
        {
            txtStudentId.Clear();

            txtSearch.Clear();

            cmbRoomType.SelectedIndex = -1;

            if (role == "Student")
            {
                cmbStatus.Text = "Pending";
            }
            else
            {
                cmbStatus.SelectedIndex = -1;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}