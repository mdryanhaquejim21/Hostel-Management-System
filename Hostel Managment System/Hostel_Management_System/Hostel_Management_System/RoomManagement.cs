using Hostel_Management_System.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hostel_Management_System
{
    public partial class RoomManagement : Form
    {
        Form3 dashboard;
        string role;

        public RoomManagement(string userRole, Form3 form3)
        {
            InitializeComponent();

            role = userRole;
            dashboard = form3;

            this.FormClosed += RoomManagement_FormClosed;
            this.Shown += (s, e) => LoadRooms();
        }

        
        private void RoomManagement_FormClosed(object sender, FormClosedEventArgs e)
        {
            dashboard.RefreshDashboardData();
            dashboard.Show();
        }

        
        private void LoadRooms()
        {
            try
            {
                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Rooms", conn);
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

        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput()) return;

                int capacity;
                int beds;
                int floor;

                if (!int.TryParse(txtCapacity.Text, out capacity) ||
                    !int.TryParse(txtAvailableBeds.Text, out beds) ||
                    !int.TryParse(txtFloorNo.Text, out floor))
                {
                    MessageBox.Show("Please enter valid numbers!");
                    return;
                }

                if (beds > capacity)
                {
                    MessageBox.Show("Available beds cannot exceed capacity");
                    return;
                }

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"INSERT INTO Rooms
                                    (RoomNumber, RoomType, Capacity, AvailableBeds, FloorNo)
                                    VALUES
                                    (@RoomNo, @Type, @Capacity, @Beds, @Floor)";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@RoomNo", txtRoomNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Type", comboRoomType.Text);
                    cmd.Parameters.AddWithValue("@Capacity", capacity);
                    cmd.Parameters.AddWithValue("@Beds", beds);
                    cmd.Parameters.AddWithValue("@Floor", floor);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Room Added Successfully");
                LoadRooms();
                dashboard.RefreshDashboardData();
                ClearFields();
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
                    MessageBox.Show("Select a room first!");
                    return;
                }

                int roomId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["RoomId"].Value);

                int capacity;
                int beds;
                int floor;

                if (!int.TryParse(txtCapacity.Text, out capacity) ||
                    !int.TryParse(txtAvailableBeds.Text, out beds) ||
                    !int.TryParse(txtFloorNo.Text, out floor))
                {
                    MessageBox.Show("Invalid number input!");
                    return;
                }

                if (beds > capacity)
                {
                    MessageBox.Show("Available beds cannot exceed capacity");
                    return;
                }

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Rooms SET
                                    RoomNumber=@RoomNo,
                                    RoomType=@Type,
                                    Capacity=@Capacity,
                                    AvailableBeds=@Beds,
                                    FloorNo=@Floor
                                    WHERE RoomId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@ID", roomId);
                    cmd.Parameters.AddWithValue("@RoomNo", txtRoomNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("@Type", comboRoomType.Text);
                    cmd.Parameters.AddWithValue("@Capacity", capacity);
                    cmd.Parameters.AddWithValue("@Beds", beds);
                    cmd.Parameters.AddWithValue("@Floor", floor);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Room Updated Successfully");
                LoadRooms();
                dashboard.RefreshDashboardData();
                ClearFields();
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
                    MessageBox.Show("Select a room first!");
                    return;
                }

                int roomId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["RoomId"].Value);

                using (SqlConnection conn = DB.GetConnection())
                {
                    conn.Open();

                    string query = "DELETE FROM Rooms WHERE RoomId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", roomId);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Room Deleted Successfully");
                LoadRooms();
                dashboard.RefreshDashboardData();
                ClearFields();
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

                    string query = @"SELECT * FROM Rooms
                                     WHERE RoomNumber LIKE @search
                                     OR RoomType LIKE @search";

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

                txtRoomNumber.Text = row.Cells["RoomNumber"].Value.ToString();
                comboRoomType.Text = row.Cells["RoomType"].Value.ToString();
                txtCapacity.Text = row.Cells["Capacity"].Value.ToString();
                txtAvailableBeds.Text = row.Cells["AvailableBeds"].Value.ToString();
                txtFloorNo.Text = row.Cells["FloorNo"].Value.ToString();
            }
        }

        
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        
        private bool ValidateInput()
        {
            return !(txtRoomNumber.Text == "" ||
                     comboRoomType.Text == "" ||
                     txtCapacity.Text == "" ||
                     txtAvailableBeds.Text == "" ||
                     txtFloorNo.Text == "");
        }

        
        private void ClearFields()
        {
            txtRoomNumber.Clear();
            comboRoomType.SelectedIndex = -1;
            txtCapacity.Clear();
            txtAvailableBeds.Clear();
            txtFloorNo.Clear();
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