using Hostel_Management_System.Database;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class Form3 : Form
    {
        string role = "";

        public Form3(string userRole)
        {
            InitializeComponent();

            role = userRole.Trim();

            this.FormClosed += Form3_FormClosed;
        }

        
        private void Form3_Load(object sender, EventArgs e)
        {
            DisableAllButtons();
            SetupDashboard();
            LoadDashboardCounts();

            MessageBox.Show("Welcome " + role);
        }

        
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
        }

        
        private void DisableAllButtons()
        {
            btnStaffManagement.Enabled = false;
            btnStudentManagement.Enabled = false;
            btnRoomManagement.Enabled = false;
            btnBookingManagement.Enabled = false;
            btnRequestRoom.Enabled = false;
            
            btnComplaints.Enabled = false;
            btnReports.Enabled = false;
        }

        
        private void SetupDashboard()
        {
            if (role == "Admin")
            {
                btnStaffManagement.Enabled = true;
                btnStudentManagement.Enabled = true;
                btnRoomManagement.Enabled = true;
                btnBookingManagement.Enabled = true;
                btnRequestRoom.Enabled = true;
                
                btnComplaints.Enabled = true;
                btnReports.Enabled = true;
            }
            else if (role == "Staff")
            {
                btnStudentManagement.Enabled = true;
                btnRoomManagement.Enabled = true;
                btnBookingManagement.Enabled = true;
                btnComplaints.Enabled = true;
            }
            else if (role == "Student")
            {
                btnRequestRoom.Enabled = true;
                
                btnComplaints.Enabled = true;
            }
        }

        
        private void LoadDashboardCounts()
        {
            using (SqlConnection con = DB.GetConnection())
            {
                con.Open();

                lblStudents.Text = "Total Students : " + GetCount(con, "Students");
                lblStaff.Text = "Total Staff : " + GetCount(con, "Staff");
                lblRooms.Text = "Total Rooms : " + GetCount(con, "Rooms");
                lblBookings.Text = "Total Bookings : " + GetCount(con, "Bookings");
                lblComplaints.Text = "Total Complaints : " + GetCount(con, "Complaints");
            }
        }

        private int GetCount(SqlConnection con, string tableName)
        {
            string query = $"SELECT COUNT(*) FROM {tableName}";
            SqlCommand cmd = new SqlCommand(query, con);
            return (int)cmd.ExecuteScalar();
        }

        
        public void RefreshDashboardData()
        {
            LoadDashboardCounts();
        }

        
        private void btnStaffManagement_Click(object sender, EventArgs e)
        {
            StaffManagement sm = new StaffManagement(role, this);
            sm.Show();
            this.Hide();
        }

        
        private void btnStudentManagement_Click(object sender, EventArgs e)
        {
            StudentManagement stdm = new StudentManagement(role, this);
            stdm.Show();
            this.Hide();
        }

        private void btnRoomManagement_Click(object sender, EventArgs e)
        {
            RoomManagement rm = new RoomManagement(role, this);
            rm.Show();
            this.Hide();
        }

        private void btnBookingManagement_Click(object sender, EventArgs e)
        {
            BookingManagement bm = new BookingManagement(role, this);
            bm.Show();
            this.Hide();
        }

        private void btnRequestRoom_Click(object sender, EventArgs e)
        {
            RequestRoom rr = new RequestRoom(role, this);
            rr.Show();
            this.Hide();

        }

        private void btnComplaints_Click(object sender, EventArgs e)
        {
            Complaints frm = new Complaints(role, this);
            frm.Show();
            this.Hide();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Reports frm = new Reports(role, this);
            frm.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Form1 login = new Form1();
            login.Show();
            this.Hide();
        }
    }
}