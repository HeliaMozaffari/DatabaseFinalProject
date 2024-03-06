using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Configuration;

namespace DataBaseFinalProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OracleConnection con = null;

        public MainWindow()
        {
            this.setConnection();
            InitializeComponent();
        }
        private void updateDataGrid()
        {
            
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * from COMM_Customers";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            myDataGrid.ItemsSource = dt.DefaultView;
            dr.Close();
        }
        private void setConnection()
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["customers"].ConnectionString;
            con = new OracleConnection(ConnectionString);
            try
            {
                con.Open();
            }
            catch(Exception exp) { }
            
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            this.updateDataGrid();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            DataRowView dr = dg.SelectedItem as DataRowView;
            if(dr != null)
            {
                firstName_txt.Text = dr["FirstName"].ToString();
                lastName_txt.Text = dr["LastName"].ToString();
                phone_txt.Text = dr["Phoneno"].ToString();
                email_txt.Text = dr["Email"].ToString();
                

            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_btn_Click(object sender, RoutedEventArgs e)
        {
            delete_btn.IsEnabled = false;

        }

        private void update_btn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void reset_btn_Click(object sender, RoutedEventArgs e)
        {
            firstName_txt.Text = "";
            lastName_txt.Text = "";
            email_txt.Text = "";
            phone_txt.Text = "";

        }
        private void AUD(String sql_stmt, int state)
        {
            string msg = "";
            OracleCommand cmd = con.CreateCommand();
            cmd.CommandText = sql_stmt;
            cmd.CommandType = CommandType.Text;

            switch (state)
            {
                case 0:
                    msg = "Row Inserted Successfully.";
                    break;
                case 1:
                    msg = "Row updated successfully.";
                    cmd.Parameters.Add("FIRSTNAME", OracleDbType.Varchar2,30).Value= firstName_txt.Text;
                    cmd.Parameters.Add("LASTNAME", OracleDbType.Varchar2, 30).Value = lastName_txt.Text;
                    cmd.Parameters.Add("Email", OracleDbType.Varchar2,30).Value = email_txt.Text;
                    break;
                case 2:
                    msg = "Row deleted Successfully.";
                    break;
            }
            try
            {
                int n = cmd.ExecuteNonQuery();
                if(n > 0)
                {
                    MessageBox.Show(msg);
                    this.updateDataGrid();
                }

            }
            catch (Exception exp) { }
        }
    }
}
