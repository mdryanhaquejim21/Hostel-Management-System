using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class RoomDetails : Form
    {
        int studentId = 1;

        public RoomDetails()
        {
            InitializeComponent();
        }

        private void RoomDetails_Load(object sender, EventArgs e)
        {
            // 🚨 IMPORTANT: prevent designer crash
            if (DesignMode) return;

            LoadRoomDetails();
        }

        private void LoadRoomDetails()
        {
            try
            {
                using (SqlConnection con = DB.GetConnection())
                {
                    con.Open();

                    string query = @"
                    SELECT 
                        b.BookingId,
                        r.RoomNumber,
                        r.RoomType,
                        r.FloorNo,
                        b.BookingDate,
                        b.BookingStatus,
                        r.AvailableBeds
                    FROM Bookings b
                    INNER JOIN Rooms r ON b.RoomId = r.RoomId
                    WHERE b.StudentId = @StudentId";

                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentId", studentId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}