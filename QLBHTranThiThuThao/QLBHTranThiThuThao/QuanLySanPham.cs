using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBHTranThiThuThao
{
    public partial class QuanLySanPham : Form
    {
        public QuanLySanPham()
        {
            InitializeComponent();
        }
        string chuoiKN;
        int maSP;
        private void QuanLySanPham_Load(object sender, EventArgs e)
        {
            chuoiKN = ConfigurationManager.ConnectionStrings["cnstr"].ConnectionString;
            gVSanPham.DataSource = LayDSSP();

            cbLoaiSP.DataSource = LayDSLSP();
            cbLoaiSP.DisplayMember = "CategoryName"; // thuộc tính hiển thị
            cbLoaiSP.ValueMember = "CategoryID"; // thuộc tính lưu trữ

            cbNCC.DataSource = LayDSNCC();
            cbNCC.DisplayMember = "CompanyName";
            cbNCC.ValueMember = "SupplierID";
        }

        private DataTable LayDSSP()
        {
            string query = "Select * from products";
            SqlConnection conn = new SqlConnection(chuoiKN);
            SqlCommand com = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            return dataSet.Tables[0];

        }

        private DataTable LayDSLSP()
        {
            string query = "Select CategoryID, CategoryName from Categories";
            SqlConnection conn = new SqlConnection(chuoiKN);
            SqlCommand com = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            return dataSet.Tables[0];

        }

        private DataTable LayDSNCC()
        {
            string query = "Select SupplierID, CompanyName from Suppliers";
            SqlConnection conn = new SqlConnection(chuoiKN);
            SqlCommand com = new SqlCommand(query, conn);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            return dataSet.Tables[0];

        }

        private void ThemSP(string tenSP, double donGia, int sl, int maNCC, int maLSP)
        {
            string query = String.Format("insert into Products(ProductName,UnitsInStock,UnitPrice,SupplierID,CategoryID)" +
                "values(N'{0}', {1}, {2}, {3},{4})", tenSP, sl, donGia, maNCC, maLSP);
            SqlConnection conn = new SqlConnection(chuoiKN);
            SqlCommand com = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                com.ExecuteNonQuery();
                MessageBox.Show("them thanh cong");
            }
            catch (SqlException exx)
            {
                MessageBox.Show(exx.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void btThem_Click(object sender, EventArgs e)
        {
            ThemSP(txtTenSP.Text, double.Parse(txtDonGia.Text), int.Parse(txtSoLuong.Text),
             int.Parse(cbNCC.SelectedValue.ToString()), int.Parse(cbLoaiSP.SelectedValue.ToString()));

            gVSanPham.DataSource = null; //xóa thông tin chung hiện tại
            gVSanPham.DataSource = LayDSSP(); //hiển thị "thông tin chung" mới

        }

        private void gVSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Lay dong duoc chon: gVSanPham.Rows[e.RowIndex]
            txtTenSP.Text = gVSanPham.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();

            txtDonGia.Text = gVSanPham.Rows[e.RowIndex].Cells["UnitPrice"].Value.ToString();
            txtSoLuong.Text = gVSanPham.Rows[e.RowIndex].Cells["UnitsInStock"].Value.ToString();
            cbNCC.SelectedValue = gVSanPham.Rows[e.RowIndex].Cells["SupplierID"].Value.ToString();
            cbLoaiSP.SelectedValue = gVSanPham.Rows[e.RowIndex].Cells["CategoryID"].Value.ToString();

            maSP = int.Parse(gVSanPham.Rows[e.RowIndex].Cells["ProductID"].Value.ToString());

        }
        //xoa

        private void XoaSP(int maSP)
        {
            string query = String.Format("delete from products where ProductID={0}", maSP);
            SqlConnection conn = new SqlConnection(chuoiKN);
            SqlCommand com = new SqlCommand(query, conn);
            try
            {
                conn.Open();
                com.ExecuteNonQuery();
                MessageBox.Show("xoa thanh cong");
            }
            catch (SqlException exx)
            {
                MessageBox.Show(exx.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            XoaSP(maSP);

            gVSanPham.DataSource = null; //xóa thông tin chung hiện tại
            gVSanPham.DataSource = LayDSSP(); //hiển thị "thông tin chung" mới
        }
    }
}
