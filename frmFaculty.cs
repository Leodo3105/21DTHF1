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

using System.Data.Entity.Migrations;

namespace Lab04
{
    public partial class frmFaculty : Form
    {
        public frmFaculty()
        {
            InitializeComponent();
        }
        StudentDBContext db = new StudentDBContext();
        private void frmFaculty_Load(object sender, EventArgs e)
        {
            try
            {
                
                List<Student> listStudent = db.Students.ToList();
                List<Faculty> listFaculties = db.Faculties.ToList();
                //FillFacultyCombobox(listFaculties);
                BindGrid(listFaculties);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BindGrid(List<Faculty> listFaculties)
        {
            dgvFaculty.Rows.Clear();
            foreach (var item in listFaculties)
            {
                int index = dgvFaculty.Rows.Add();
                dgvFaculty.Rows[index].Cells["dgvFacultyID"].Value = item.FacultyID;
                dgvFaculty.Rows[index].Cells["dgvFacultyName"].Value = item.FacultyName;
                dgvFaculty.Rows[index].Cells["dgvTotalProfesser"].Value = item.TotalProfessor;
            }
        }
        private int GetSelectedRow(string facultyID)
        {
            for (int i = 0; i < dgvFaculty.Rows.Count; i++)
            {
                if (dgvFaculty.Rows[i].Cells["dgvFacultyID"].Value.ToString() == facultyID)
                    return i;
            }
            return -1;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
            try
            {
                //checkInfomation();
                int selectedRow = GetSelectedRow(txtFacultyID.Text);
                if (selectedRow == -1)
                {
                    Faculty s = new Faculty();
                    s.FacultyID = int.Parse(txtFacultyID.Text);
                    s.FacultyName = txtFacultyName.Text;
                    s.TotalProfessor = int.Parse(txtTotalProfessor.Text);
                    db.Faculties.AddOrUpdate(s);
                    db.SaveChanges();
                    BindGrid(db.Faculties.ToList());
                    MessageBox.Show("Thêm mới dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    Faculty update = db.Faculties.FirstOrDefault(p => p.FacultyID == int.Parse(txtFacultyID.Text));
                    if (update == null)
                    {
                        MessageBox.Show("Không tìm thấy mã khoa cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    update.FacultyName = txtFacultyName.Text;
                    update.TotalProfessor = int.Parse(txtTotalProfessor.Text);
                    db.SaveChanges();
                    BindGrid(db.Faculties.ToList());
                    MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvFaculty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dgvFaculty.Rows[e.RowIndex];

                    txtFacultyID.Text = row.Cells["dgvFacultyID"].Value.ToString();
                    txtFacultyName.Text = row.Cells["dgvFacultyName"].Value.ToString();
                    txtTotalProfessor.Text = row.Cells["TotalProfesser"].Value.ToString();
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
                Faculty delete = db.Faculties.FirstOrDefault(p => p.FacultyID == int.Parse(txtFacultyID.Text));
                if (delete == null)
                {
                    MessageBox.Show("Không tìm thấy mã khoa cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                db.Faculties.Remove(delete);
                db.SaveChanges();
                BindGrid(db.Faculties.ToList());
                MessageBox.Show("Xóa khoa thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}
