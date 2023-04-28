using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
using TCSCaseStudy.Models;

namespace TCSCaseStudy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        

        public MainWindow()
        {
            string authorizationtoken = "fa114107311259f5f33e70a5d85de34a2499b4401da069af0b1d835cd5ec0d56";

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authorizationtoken);
            client.BaseAddress = new Uri("https://gorest.co.in/public-api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
            InitializeComponent();
        }

        private void btnLoadEmployees_Click(object sender, RoutedEventArgs e)
        {
            this.GetEmployees();
        }

        private async void GetEmployees()
        {
            var response = await client.GetStringAsync("users");
            //JsonSerializerSettings settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };            
            //List<Employee> employees = new List<Employee>();
            //employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            //var employees = (DataTable)JsonConvert.DeserializeObject(response, typeof(DataTable), settings);
            //dynamic employees = JsonConvert.DeserializeObject(response, typeof(Employee), settings);
            wrapperEmployee employees = JsonConvert.DeserializeObject<wrapperEmployee>(response);
            dgEmployee.DataContext = employees.data;
        }

        private async void SaveEmployee(Employee employee)
        {
            await client.PostAsJsonAsync("users", employee);
        }

        private async void UpdateEmployee(Employee employee)
        {
            await client.PutAsJsonAsync("users/"+ employee.id, employee);
        }
        private async void DeleteEmployee(int employeeid)
        {
            await client.DeleteAsync("users/" + employeeid);
        }

        private void btnSaveEmployee_Click(object sender, RoutedEventArgs e)
        {
            var employee = new Employee()
            {
                id = Convert.ToInt32(txtEmployeeId.Text),
                name = txtName.Text.ToString(),
                email = txtEmail.Text.ToString(),
                gender = txtGender.Text.ToString(),
                status = txtStatus.Text.ToString()
            };

            if(employee.id == 0)
            {
                this.SaveEmployee(employee);
            }
            else
            {
                this.UpdateEmployee(employee);
            }

            txtEmployeeId.Text = 0.ToString();
            txtName.Text = "";
            txtEmail.Text = "";
            txtGender.Text = "";
            txtStatus.Text = "";
        }

        void btnEditEmployee(object sender, RoutedEventArgs e)
        {
            Employee employee = ((FrameworkElement)sender).DataContext as Employee;
            txtEmployeeId.Text = employee.id.ToString();
            txtName.Text = employee.name;
            txtEmail.Text = employee.email;
            txtGender.Text = employee.gender;
            txtStatus.Text = employee.status;
        }

        void btnDeleteEmployee(object sender, RoutedEventArgs e)
        {
            Employee employee = ((FrameworkElement)sender).DataContext as Employee;
            this.DeleteEmployee(employee.id);
        }
    }
}
