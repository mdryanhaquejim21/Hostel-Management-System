using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class Complaints : Form
    {
        Form3 dashboard;
        string role;

        
        public Complaints(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            if (role == "Student")
            {
                btnDelete.Enabled = false;
            }

            this.FormClosed += Complaints_FormClosed;
            this.Shown += (s, e) => LoadComplaints();
        }

        
        private void Complaints_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboard.RefreshDashboardData();
            dashboard.Show();
        }

        
        private void LoadComplaints()
        {
            try
            {
                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"
                        SELECT ComplaintId, StudentId, ComplaintText, ComplaintDate
                        FROM Complaints", con);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;

                    dataGridView1.ReadOnly = true;
                    dataGridView1.AllowUserToAddRows = false;
                    dataGridView1.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtStudentId.Text == "" || txtComplaintText.Text == "")
                {
                    MessageBox.Show("Fill all fields!");
                    return;
                }

                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"
                        INSERT INTO Complaints
                        (StudentId, ComplaintText, ComplaintDate)
                        VALUES
                        (@sid, @text, @date)", con);

                    cmd.Parameters.AddWithValue("@sid", txtStudentId.Text);
                    cmd.Parameters.AddWithValue("@text", txtComplaintText.Text);
                    cmd.Parameters.AddWithValue("@date", dtComplaintDate.Value.Date);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Complaint Submitted Successfully!");

                LoadComplaints();
                ClearFields();
                dashboard.RefreshDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Submit Error: " + ex.Message);
            }
        }

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.CurrentRow == null)
                {
                    MessageBox.Show("Select a complaint!");
                    return;
                }

                int id = Convert.ToInt32(
                    dataGridView1.CurrentRow.Cells["ComplaintId"].Value);

                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Complaints WHERE ComplaintId=@id", con);

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Deleted Successfully!");

                LoadComplaints();
                ClearFields();
                dashboard.RefreshDashboardData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete Error: " + ex.Message);
            }
        }

       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtStudentId.Text =
                    dataGridView1.Rows[e.RowIndex].Cells["StudentId"].Value.ToString();

                txtComplaintText.Text =
                    dataGridView1.Rows[e.RowIndex].Cells["ComplaintText"].Value.ToString();

                dtComplaintDate.Value =
                    Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["ComplaintDate"].Value);
            }
        }

        
        private void ClearFields()
        {
            txtStudentId.Clear();
            txtComplaintText.Clear();
            dtComplaintDate.Value = DateTime.Now;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}