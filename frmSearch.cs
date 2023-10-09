using Lab04.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04
{
    public partial class frmSearch : Form
    {
        public static StudentDBContext db = new StudentDBContext();
        List<Student> listStudent = db.Students.ToList();
        public frmSearch()
        {
            InitializeComponent();
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            try
            {
                List<Faculty> listFaculties = db.Faculties.ToList();
                FillFacultyCombobox(listFaculties);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFacultyCombobox(List<Faculty> listFaculties)
        {
            this.cmbFaculty.DataSource = listFaculties;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có đồng ý thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            foreach (var item in listStudent)
            {
                if (txtStudentID.Text.Contains(item.StudentID) || txtFullName.Text.Contains(item.FullName) || cmbFaculty.Text.Contains(item.Faculty.FacultyName))
                {
                    BindGrid(item);
                }
            }
            txtResult.Text = dgvSearch.Rows.Count.ToString();
        }

        private void BindGrid(Student item)
        {
            int index = dgvSearch.Rows.Add();
            dgvSearch.Rows[index].Cells["dgvStudentID"].Value = item.StudentID;
            dgvSearch.Rows[index].Cells["dgvFullName"].Value = item.FullName;
            dgvSearch.Rows[index].Cells["dgvFaculty"].Value = item.Faculty.FacultyName;
            dgvSearch.Rows[index].Cells["dgvAverageScore"].Value = item.AverageScore;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dgvSearch.Rows.Clear();
        }
    }
}
