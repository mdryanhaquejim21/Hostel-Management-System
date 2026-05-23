using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class Reports : Form
    {
        Form3 dashboard;
        string role;

        public Reports(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            this.Shown += (s, e) =>
            {
                dataGridView1_CellContentClick(null, null);
            };

            this.FormClosed += Reports_FormClosed;
        }

        private void Reports_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboard.Show();
        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SqlConnection con = DB.GetConnection();
                con.Open();

                string query = @"
                    SELECT 'Total Students' AS ReportName, COUNT(*) AS Total FROM Students
                    UNION ALL
                    SELECT 'Total Staff', COUNT(*) FROM Staff
                    UNION ALL
                    SELECT 'Total Rooms', COUNT(*) FROM Rooms
                    UNION ALL
                    SELECT 'Total Bookings', COUNT(*) FROM Bookings
                    UNION ALL
                    SELECT 'Pending Requests', COUNT(*) FROM RequestRoom WHERE RequestStatus = 'Pending'
                    UNION ALL
                    SELECT 'Complaints', COUNT(*) FROM Complaints";

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                
                dataGridView1.ReadOnly = true;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridView1.Columns[0].HeaderText = "Report Name";
                dataGridView1.Columns[1].HeaderText = "Total Count";

                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor =
                    System.Drawing.Color.AliceBlue;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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