using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Appointment_Manager
{
    public partial class EditCustomersScreen : Form
    {
        private Customer customer = new Customer();
        private readonly List<Customer> customerList;

        public EditCustomersScreen(ref List<Customer> customerList)
        {
            InitializeComponent();
            Text = "Add Customer";
            lblCustomer.Text = "Add Customer";
            StartPosition = FormStartPosition.CenterScreen;
            this.customerList = customerList;
            SetInputLimits();
            btnApply.Text = "Add";
        }
        
        public EditCustomersScreen(ref List<Customer> customerList, Customer customer)
        {
            InitializeComponent();
            Text = "Edit Customer";
            lblCustomer.Text = "Edit Customer";
            StartPosition = FormStartPosition.CenterScreen;
            this.customerList = customerList;
            this.customer = customer;
            txtAddress1.Text = customer.Address;
            txtAddress2.Text = customer.Address2;
            txtCity.Text = customer.City;
            txtCountry.Text = customer.Country;
            txtName.Text = customer.CustomerName;
            txtPhoneNumber.Text = customer.Phone;
            txtPostalCode.Text = customer.PostalCode;
            SetInputLimits();
            btnApply.Text = "Update";
        }
        
        private void SetInputLimits()
        {
            txtAddress1.MaxLength = 50;
            txtAddress2.MaxLength = 50;
            txtCity.MaxLength = 50;
            txtCountry.MaxLength = 50;
            txtName.MaxLength = 50;
            txtPhoneNumber.MaxLength = 20;
            txtPostalCode.MaxLength = 10;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            bool stop = false;
            
            foreach (Control tBox in this.Controls.OfType<TextBox>())
            {
                if(tBox != txtAddress2)
                    stop = string.IsNullOrEmpty(tBox.Text);
                if (stop)
                    break;
            }
            if (!stop)
            {
                customer.Active = cbxActive.Checked;
                customer.Address = txtAddress1.Text;
                customer.Address2 = txtAddress2.Text;
                customer.City = txtCity.Text;
                customer.Country = txtCountry.Text;
                customer.CustomerName = txtName.Text;
                customer.Phone = txtPhoneNumber.Text;
                customer.PostalCode = txtPostalCode.Text;
                customer.CityID = customerList.Where(cust => cust.City == txtCity.Text).Select(cust => cust.CityID).DefaultIfEmpty(0).FirstOrDefault();
                customer.CountryID = customerList.Where(cust => cust.Country == txtCountry.Text).Select(cust => cust.CountryID).DefaultIfEmpty(0).FirstOrDefault();

                if (customer.CustomerID == 0)
                {
                    customer.Add();
                }
                else
                {
                    customer.Update();
                }
                Close();
            }
            else
            {
                MessageBox.Show("Please fill out all fields.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => Close();
    }
}
