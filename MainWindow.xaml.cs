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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace final
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // conexion a la DB
            string myDBConnection = ConfigurationManager.ConnectionStrings["final.Properties.Settings.finalappConnectionString"].ConnectionString;

            myDBConnectionSQL = new SqlConnection(myDBConnection);

            // cargar los datos al inicio de la app
            LoadGrid();
        }
        SqlConnection myDBConnectionSQL;

        // limpiar datos del
        private void clearData()
        {
            nombre_txt.Clear();
            clase_txt.Clear();
            raza_txt.Clear();
            nivel_txt.Clear();
            search_txt.Clear();
        }
        // cargar el contenido del datagrid
        private void LoadGrid()
        {

            string consult = "SELECT * FROM Datos";

            SqlCommand cmd = new SqlCommand(consult, myDBConnectionSQL);
            DataTable dt = new DataTable();
            myDBConnectionSQL.Open();

            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            dt.Load(sqlDataReader);

            myDBConnectionSQL.Close();

            datagrid.ItemsSource = dt.DefaultView;
        }

        // boton limpiar campos
        private void clearDataBtn_Click(object sender, RoutedEventArgs e)
        {
            clearData();
        }
        // validar campos del formulario
        private bool isValid()
        {
            if (nombre_txt.Text == string.Empty)
            {
                MessageBox.Show("El nombre es obligatorio");
                return false;
            }
            if (clase_txt.Text == string.Empty)
            {
                MessageBox.Show("La clase es obligatoria");
                return false;
            }
            if (raza_txt.Text == string.Empty)
            {
                MessageBox.Show("La raza es obligatoria");
                return false;
            }
            if (nivel_txt.Text == string.Empty)
            {
                MessageBox.Show("El nivel es obligatorio");
                return false;
            }
            return true;
        }
        // boton insertar
        private void inserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isValid())
                {
                    string consult = "INSERT INTO Datos (Nombre, Clase, Raza, Nivel) VALUES (@Nombre, @Clase, @Raza, @Nivel)";

                    SqlCommand cmd = new SqlCommand(consult, myDBConnectionSQL);


                    cmd.Parameters.AddWithValue("@Nombre", nombre_txt.Text);
                    cmd.Parameters.AddWithValue("@Clase", clase_txt.Text);
                    cmd.Parameters.AddWithValue("@Raza", raza_txt.Text);
                    cmd.Parameters.AddWithValue("@Nivel", nivel_txt.Text);

                    myDBConnectionSQL.Open();
                    cmd.ExecuteNonQuery();
                    myDBConnectionSQL.Close();

                    clearData();
                    LoadGrid();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // botón borrar
        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

            string consult = "DELETE FROM Datos WHERE ID = " + search_txt.Text;

            SqlCommand cmd = new SqlCommand(consult, myDBConnectionSQL);

            myDBConnectionSQL.Open();

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ha sido borrado correctamente");
                myDBConnectionSQL.Close();

                clearData();
                LoadGrid();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("No ha sido borrado." + ex.Message);
            }
            finally
            {
                myDBConnectionSQL.Close();
            }
        }
        // boton actualizar
        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            // consulta
            string consult = "UPDATE Datos set Nombre = '" + nombre_txt.Text + "', Clase = '" + clase_txt.Text + "', Raza = '" + raza_txt.Text + "' Nivel = '" + nivel_txt.Text + "' WHERE ID = '"+search_txt.Text+"' ";

            myDBConnectionSQL.Open();
            SqlCommand cmd = new SqlCommand(consult, myDBConnectionSQL);
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Datos actualizados");
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                myDBConnectionSQL.Close();
                clearData();
                LoadGrid();
            }
        }
    }
}
