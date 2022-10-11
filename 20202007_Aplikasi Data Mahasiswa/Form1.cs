using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;

namespace _20202007_Aplikasi_Data_Mahasiswa
{
    public partial class DashboardForm : Form
    {
        static string MyConnection = "server=localhost;user id=root;database=simdb;pwd=;Convert Zero Datetime=True";
        MySqlConnection connect = new MySqlConnection(MyConnection);

        public string hobby = null;
        public string gender = null;
        public string imgLocation = null;
        
        public DashboardForm()
        {
            InitializeComponent();

            //datagridview error handler
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e) //datagridview error handler
        {
            e.Cancel = true;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            // Mastery
            cbMastery.Items.Add("TMI");
            cbMastery.Items.Add("TMK");
            cbMastery.Items.Add("TPM");
            cbMastery.Items.Add("PM");
            cbMastery.Items.Add("TRM");
            cbMastery.Items.Add("TRMK");

            // Class
            cbClass.Items.Add("3A");
            cbClass.Items.Add("3B");
            cbClass.Items.Add("3C");

            // Date
            labelDate.Text = DateTime.Now.ToString("dd MMMM yyyy");

            // Load Data from DB
            readData();
        }

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "JPG files(.*jpg)|*.jpg|PNG files(.*png)|*.png|All Files(*.*)|*.*";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imgLocation = dialog.FileName.ToString();

                    pbPhoto.ImageLocation = imgLocation;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error", "Please Choose the Right File Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panelMenu_Paint(object sender, PaintEventArgs e)
        {
            panelMenu.BackColor = ColorTranslator.FromHtml("#222228");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string hobby = null;
                string gender = null;

                // Hobby
                if (cbSport.CheckState == CheckState.Checked) { hobby += " Sport"; }
                if (cbDance.CheckState == CheckState.Checked) { hobby += " Dance"; }
                if (cbMusic.CheckState == CheckState.Checked) { hobby += " Music"; }
                if (cbFilm.CheckState == CheckState.Checked) { hobby += " Film"; }
                if (cbTravel.CheckState == CheckState.Checked) { hobby += " Travel"; }

                // Gender
                if (rbMan.Checked) { gender = rbMan.Text; }
                if (rbWomen.Checked) { gender = rbWomen.Text; }

                string MyConnection = "server=localhost;user id=root;database=simdb;pwd=";
                MySqlConnection connect = new MySqlConnection(MyConnection);

                // Photo
                byte[] images = null;
                FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(Stream);
                images = br.ReadBytes((int)Stream.Length);

                MySqlCommand cmd;

                connect.Open();

                cmd = connect.CreateCommand();

                //string QuerryInsert = "UPDATE datamahasiswa SET NIM = '" + this.tbNIM.Text +
                //    "',Name = '" + this.tbName.Text +
                //    "',Mastery = '" + this.cbMastery.Text +
                //    "',Class = '" + this.cbClass.Text +
                //    "',Address = '" + this.tbAddress.Text +
                //    "',Hobby = '" + hobby +
                //    "',BirthOfDate = '" + dateTimePicker.Value +
                //    "',Gender = '" + gender +
                //    "',Photo = '" + images +
                //    "' WHERE NIM = '" + this.tbNIM.Text + "'";

                //cmd.CommandText = QuerryInsert;

                string QuerryInsert = "UPDATE datamahasiswa SET NIM=@NIM, Name=@Name, Mastery=@Mastery, Class=@Class, Address=@Address, Hobby=@Hobby, BirthOfDate=@BirthOfDate, Gender=@Gender, Photo=@Photo WHERE NIM=" + tbNIM.Text + ";";
                cmd.CommandText = QuerryInsert;

                cmd.Parameters.AddWithValue("@NIM", tbNIM.Text);
                cmd.Parameters.AddWithValue("@Name", tbName.Text);
                cmd.Parameters.AddWithValue("@Mastery", cbMastery.Text);
                cmd.Parameters.AddWithValue("@Class", cbClass.Text);
                cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                cmd.Parameters.AddWithValue("@Hobby", hobby);
                cmd.Parameters.AddWithValue("@BirthOfDate", dateTimePicker.Value);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Photo", images);


                MessageBox.Show("UPDATE DATA", "Warning !");
                cmd.ExecuteNonQuery();

                readData();

                connect.Close();

                clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            labelTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string MyConnection = "server=localhost;user id=root;database=simdb;pwd=;Convert Zero Datetime=True";
                MySqlConnection connect = new MySqlConnection(MyConnection);

                MySqlCommand cmd;

                connect.Open();

                cmd = connect.CreateCommand();

                string QuerryInsert = "DELETE FROM datamahasiswa WHERE NIM = '" + tbNIM.Text + "';";
                cmd.CommandText = QuerryInsert;

                MessageBox.Show("DELETE DATA", "Warning !");
                cmd.ExecuteNonQuery();

                readData();

                clearAll();

                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                tbNIM.Text = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                tbName.Text = dataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                cbMastery.Text = dataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                cbClass.Text = dataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                tbAddress.Text = dataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                hobby = dataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
                string NIM = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                //pbPhoto.Image = dataGridView.Rows[e.RowIndex].Cells[8].Value.ToString;

                string dataGender = dataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();

                if (dataGender == "Laki-Laki")
                {
                    rbMan.Checked = true;
                    rbWomen.Checked = false;
                }
                else if (dataGender == "Perempuan")
                {
                    rbMan.Checked = false;
                    rbWomen.Checked = true;
                }
                else
                {
                    rbMan.Checked = false;
                    rbWomen.Checked = false;
                }

                string textName = "";
                string MyConnection = "server=localhost;user id=root;database=simdb;pwd=";
                MySqlConnection connect = new MySqlConnection(MyConnection);

                MySqlCommand cmd;

                connect.Open();

                cmd = connect.CreateCommand();

                string QuerryInsert = "SELECT Name,Photo FROM datamahasiswa WHERE NIM = '" + tbNIM.Text + "';";
                cmd.CommandText = QuerryInsert;
                MySqlDataReader DataRead = cmd.ExecuteReader();
                DataRead.Read();

                byte[] images = null;

                if (DataRead.HasRows)
                {
                    try
                    {
                        textName = DataRead[0].ToString();
                        images = (byte[])DataRead[1];
                    }
                    catch
                    {
                        MessageBox.Show("Photo Not Found !", "ERROR");
                    }


                    if (images == null)
                    {
                        pbPhoto.Image = null;
                    }
                    else
                    {
                        MemoryStream mstream = new MemoryStream(images);
                        pbPhoto.Image = Image.FromStream(mstream);
                    }
                }
                else
                {
                    MessageBox.Show("This Data Isn't Available", "ERROR");
                }

                connect.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Read Data from DB
        public void readData()
        {
            MySqlCommand cmd;
            cmd = connect.CreateCommand();
            cmd.CommandText = "SELECT * FROM datamahasiswa";
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView.DataSource = ds.Tables[0].DefaultView;
        }

        public void clearAll()
        {
            // Clear All Textboxes
            tbNIM.Text = "";
            tbName.Text = "";
            cbMastery.Text = "";
            cbClass.Text = "";
            tbAddress.Text = "";

            // Hobby
            cbSport.Checked = false;
            cbDance.Checked = false;
            cbMusic.Checked = false;
            cbFilm.Checked = false;
            cbTravel.Checked = false;

            // Gender
            rbMan.Checked = false;
            rbWomen.Checked = false;

            // Picture Box
            pbPhoto.Image = null;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string MyConnection = "server=localhost;user id=root;database=simdb;pwd=;Convert Zero Datetime=True";
                MySqlConnection connect = new MySqlConnection(MyConnection);

                MySqlCommand cmd;

                connect.Open();

                cmd = connect.CreateCommand();

                string QuerryInsert = "SELECT * FROM datamahasiswa WHERE Name LIKE '%" + tbSearch.Text + "%' OR NIM LIKE '%" + tbSearch.Text + "%' OR Mastery LIKE '%" + tbSearch.Text + "%' OR Class LIKE '%" + tbSearch.Text + "%' OR Address LIKE '%" + tbSearch.Text + "%' OR Hobby LIKE '%" + tbSearch.Text + "%' OR Gender LIKE '%" + tbSearch.Text + "%';";
                cmd.CommandText = QuerryInsert;
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView.DataSource = ds.Tables[0].DefaultView;

                cmd.CommandText = QuerryInsert;

                cmd.ExecuteNonQuery();

                connect.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Hobby
                if (cbSport.CheckState == CheckState.Checked) { hobby += " Sport"; }
                if (cbDance.CheckState == CheckState.Checked) { hobby += " Dance"; }
                if (cbMusic.CheckState == CheckState.Checked) { hobby += " Music"; }
                if (cbFilm.CheckState == CheckState.Checked) { hobby += " Film"; }
                if (cbTravel.CheckState == CheckState.Checked) { hobby += " Travel"; }

                // Gender
                if (rbMan.Checked) { gender = rbMan.Text; }
                if (rbWomen.Checked) { gender = rbWomen.Text; }

                // Save to DB
                string MyConnection = "server=localhost;user id=root;database=simdb;pwd=;Convert Zero Datetime=True";
                MySqlConnection connect = new MySqlConnection(MyConnection);
                MySqlCommand cmd;

                // Photo
                byte[] images = null;
                FileStream Stream = new FileStream(imgLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(Stream);
                images = br.ReadBytes((int)Stream.Length);

                connect.Open();

                cmd = connect.CreateCommand();

                string QuerryInsert = "INSERT INTO datamahasiswa (NIM, Name, Mastery, Class, Address, Hobby, BirthOfDate, Gender, Photo) values (@NIM, @Name, @Mastery, @Class, @Address, @Hobby, @BirthOfDate, @Gender, @Photo)";
                cmd.CommandText = QuerryInsert;

                cmd.Parameters.AddWithValue("@NIM", tbNIM.Text);
                cmd.Parameters.AddWithValue("@Name", tbName.Text);
                cmd.Parameters.AddWithValue("@Mastery", cbMastery.Text);
                cmd.Parameters.AddWithValue("@Class", cbClass.Text);
                cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                cmd.Parameters.AddWithValue("@Hobby", hobby);
                cmd.Parameters.AddWithValue("@BirthOfDate", dateTimePicker.Value);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Photo", images);
                
                MessageBox.Show("INSERT DATA", "Warning !");
                cmd.ExecuteNonQuery();

                // Submit
                MessageBox.Show(
                    "NIM\t\t: " + tbNIM.Text +
                    "\nName\t\t: " + tbName.Text +
                    "\nMastery\t\t: " + cbMastery.Text +
                    "\nClass\t\t: " + cbClass.Text +
                    "\nAddress\t\t: " + tbAddress.Text +
                    "\nHobby\t\t:" + hobby +
                    "\nGender\t\t: " + gender +
                    "\nBirth of Date\t: " + dateTimePicker.Text,
                    "Your Data Have Been Saved !"/*, MessageBoxButtons.OK*/);

                // Clear All Data
                clearAll();

                // Load Data from DB
                readData();

                connect.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
    }
}