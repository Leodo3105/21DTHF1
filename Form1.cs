using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab04.Model;

namespace Lab04
{
    public partial class Form1 : Form
    {
        List<Faculty> ListFaculties = new List<Faculty>();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var name = txtFullName.Text;
            var faculty = (int)cmbFaculty.SelectedValue;
            var averageScore = txtAverageScore.Text;
            var studentID = txtStudentID.Text;
            StudentDBContext db = new StudentDBContext();
            Student student = new Student()
            {
                FullName = name,
                FacultyID = faculty,
                AverageScore = double.Parse(averageScore),
                StudentID = studentID,
            };

            db.Students.Add(student);
            db.SaveChanges();
            BindGrid(db.Students.ToList());

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                StudentDBContext db = new StudentDBContext();
                ListFaculties = db.Faculties.ToList();
                List<Student> listStudent = db.Students.ToList();
                FillDataComboBox(ListFaculties);
                BindGrid(listStudent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void BindGrid(List<Student> listStudent)
        {
            dgvStudent.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells["dgvStudentID"].Value = item.StudentID;
                dgvStudent.Rows[index].Cells["dgvFullName"].Value = item.FullName;
                dgvStudent.Rows[index].Cells["dgvFaculty"].Value = item.Faculty.FacultyName;
                dgvStudent.Rows[index].Cells["dgvAverageScore"].Value = item.AverageScore;
            }
        }

        private void FillDataComboBox(List<Faculty> listFacultys)
        {
            this.cmbFaculty.DataSource = listFacultys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            StudentDBContext db = new StudentDBContext();
            var updateStudent = db.Students.SingleOrDefault(c => c.StudentID.Equals(txtStudentID.Text));

            if (updateStudent == null)
            {
                MessageBox.Show("Không tồn tại sinh viên");
                return;
            }
            updateStudent.FullName = txtFullName.Text;
            updateStudent.AverageScore = double.Parse(txtAverageScore.Text);
            updateStudent.FacultyID = (int)cmbFaculty.SelectedValue;
            db.SaveChanges();
            BindGrid(db.Students.ToList());


        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                txtStudentID.Text = dgvStudent.Rows[rowIndex].Cells["dgvStudentID"].Value.ToString();
                txtFullName.Text = dgvStudent.Rows[rowIndex].Cells["dgvFullName"].Value.ToString();
                txtAverageScore.Text = dgvStudent.Rows[rowIndex].Cells["dgvAverageScore"].Value.ToString();
                var khoa = ListFaculties.Single
                    (c => c.FacultyName.Equals(dgvStudent.Rows[rowIndex].Cells["dgvFaculty"].Value.ToString()));
                cmbFaculty.SelectedItem = khoa;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            StudentDBContext db = new StudentDBContext();
            Student student = new Student();
            var deleteStudent = db.Students.SingleOrDefault(c => c.StudentID.Equals(txtStudentID.Text));
            if (deleteStudent != null)
            {
                DialogResult result = MessageBox.Show($"Bạn có đồng ý xóa sinh viên {deleteStudent.FullName}",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    db.Students.Remove(deleteStudent);
                    db.SaveChanges();
                    BindGrid(db.Students.ToList());
                }
            }

        }

        private void btnQuit_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Bạn có đồng ý thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }

        }

        private void quảnLýKhoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFaculty frm = new frmFaculty();
            frm.ShowDialog();
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSearch frm = new frmSearch();
            frm.ShowDialog();
        }
    }
}
