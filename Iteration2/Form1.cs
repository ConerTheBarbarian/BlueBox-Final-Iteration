using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// This is for the use of SQL
using System.Data.SqlClient;

namespace Iteration2
{
    public partial class Form1 : Form
    {
        // Testing SQL Connection
        //MessageBox.Show(Walton_DB.OpenConnection().ToString());

        string connectionstring = "Data Source=essql1.walton.uark.edu; Initial Catalog=ISYS4283Team05; User ID=ISYS4283Team05; Password=WK20ogc$"; //trusted_connection = True;
        SqlConnection connection;
        // How you are gonna get there
        SqlCommand command;
        // What do you want, how a transaction is done in SQL server
        SqlDataReader datareader;
        // Allows us to read data from SQL server

        // Bool for login
        bool isCustomerLogin = false;
        bool isEmployeeLogin = false;

        // Variables needed for public use throughout the system
        // ID variables to save the ID of the user logged in
        int customerID;
        int employeeID;

        // Variables to save the Names of the person logged in
        string customerName;
        string employeeName;

        // Saves what screen the employee was trying to access before being rerouted to the login screen
        int employeeScreen;

        // Saves the ID's of the selected row (used for row deletion)
        int selectedEmployee;
        int selectedUnit;
        int selectedReturnTitle;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Changing color of panels to their control colors
            pnlMainMenu.BackColor = SystemColors.Control;
            pnlMovies.BackColor = SystemColors.Control;
            pnlEmployee.BackColor = SystemColors.Control;
            pnlTransaction.BackColor = SystemColors.Control;
            pnlLogin.BackColor = SystemColors.Control;
            pnlSignUp.BackColor = SystemColors.Control;
            pnlEmployeeModifier.BackColor = SystemColors.Control;
            pnlUnitManagement.BackColor = SystemColors.Control;
            pnlEmployeeLogin.BackColor = SystemColors.Control;
            pnlUnitModifier.BackColor = SystemColors.Control;
            pnlAddEmployee.BackColor = SystemColors.Control;
            pnlAddUnit.BackColor = SystemColors.Control;
            pnlEditPersInfo.BackColor = SystemColors.Control;
            pnlReturn.BackColor = SystemColors.Control;

            // Make some buttons invisible on the main screen to the user until logged in
            btnMainLogOut.Visible = false;
            btnMainEditInfo.Visible = false;

            // ID labels set to invisible for programming purposes
            lblEmpModID.Visible = false;
            lblUnitModID.Visible = false;

            // Name labels that show the name of the user logged in
            lblMainUser.Visible = false;

            // When system starts, all "Remove" Buttons are disabled until something is selected
            btnEmpRemove.Enabled = false;
            btnUnitRemove.Enabled = false;
            btnReturnTitle.Enabled = false;

            // Only show main menu on startup
            returnToMainMenu();

            // Load customer sign up membership type combobox
            cmbSignUpMemb.Items.Add("Standard");
            cmbSignUpMemb.Items.Add("Premium");
            cmbEditPersMem.Items.Add("Standard");
            cmbEditPersMem.Items.Add("Premium");

            // Load data grids present throughout the program
            loadTitleDataGrid();
            loadEmployeeDataGrid();
            loadUnitManagementDataGrid();
            
        }

        private void loadTitleDataGrid()
        {
            // Connect to a source for the Movie/Games Data Grid
            connection = new SqlConnection(connectionstring);
            connection.Open();
            var sql2 = "SELECT Title_ID, DVD_Title, Price, Type, Genre, Studio, Rating FROM Titles";
            var da = new SqlDataAdapter(sql2, connection);
            // Sql data adapter is another way to connect to sql server
            var ds = new DataSet();
            // Dataset is the equivalent is basically an excel spreadsheet
            da.Fill(ds);
            // Filling the dataset
            dgMovieView.DataSource = ds.Tables[0];
        }

        private void loadEmployeeDataGrid()
        {
            // Connect to a source for the Employee Data Grid using the same method as above
            connection = new SqlConnection(connectionstring);
            connection.Open();
            var sql3 = "SELECT * FROM Employees";
            var da2 = new SqlDataAdapter(sql3, connection);
            var ds2 = new DataSet();
            da2.Fill(ds2);
            dgEmployeeEmployees.DataSource = ds2.Tables[0];
        }

        private void loadUnitManagementDataGrid()
        {
            // Connect to a source for the Unit management data grid
            connection = new SqlConnection(connectionstring);
            connection.Open();
            var sql4 = "SELECT * FROM Titles";
            var da3 = new SqlDataAdapter(sql4, connection);
            // Sql data adapter is another way to connect to sql server
            var ds3 = new DataSet();
            // Dataset is the equivalent is basically an excel spreadsheet
            da3.Fill(ds3);
            // Filling the dataset
            dgUnitManagement.DataSource = ds3.Tables[0];
        }
 
        private void loadReturnTitleDataGrid()
        {
            // Connect to a source for the Return Title data grid
            connection = new SqlConnection(connectionstring);
            connection.Open();
            var sql5 = "SELECT * FROM Rentals WHERE Customer_ID = '" + customerID + "'";
            var da4 = new SqlDataAdapter(sql5, connection);
            // Sql data adapter is another way to connect to sql server
            var ds4 = new DataSet();
            // Dataset is the equivalent is basically an excel spreadsheet
            da4.Fill(ds4);
            // Filling the dataset
            dgReturn.DataSource = ds4.Tables[0];
        }
            

        private string getCustomerName()
        {
            connection = new SqlConnection(connectionstring);
            connection.Open();
            var sql = "SELECT First_Name FROM Customers WHERE Customer_Id = " + customerID;
            command = new SqlCommand(sql, connection);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                customerName = datareader[0].ToString().Trim();
            }
            return customerName;
        }
        private void returnToMainMenu()
        {
            // Take user back to main menu
            pnlMainMenu.Visible = true;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlMainMenu.Dock = DockStyle.Fill;
        }

        private void returnToMovieMenu()
        {
            // Pulling up movie menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = true;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlMovies.Dock = DockStyle.Fill;
        }

        private void returnToEmployeeMenu()
        {
            // Takes user to employee menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = true;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlEmployee.Dock = DockStyle.Fill;
        }


        private void returnToTransactionMenu()
        {
            // Takes user to transaction menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = true;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlTransaction.Dock = DockStyle.Fill;
        }

        private void returnToLogin()
        {
            // Takes user to the login menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = true;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlLogin.Dock = DockStyle.Fill;
        }

        private void returnToSignUp()
        {
            // Takes user to the sign up menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = true;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlSignUp.Dock = DockStyle.Fill;
        }

        private void returnToEmployeeModifier()
        {
            // Takes user to the employee modifier menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = true;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlEmployeeModifier.Dock = DockStyle.Fill;
        }

        private void returnToEmployeeLogin()
        {
            // Takes user to the Employee login screen
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = true;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlEmployeeLogin.Dock = DockStyle.Fill;
        }

        private void returnToUnitManagement()
        {
            // Takes user to the unit management menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = true;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlUnitManagement.Dock = DockStyle.Fill;
        }

        private void returnToUnitMod()
        {
            // takes the user to the unit modification menu
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = true;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlUnitModifier.Dock = DockStyle.Fill;
        }

        private void returnToAddEmp()
        {
            // Takes the user to the add employee screen
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = true;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlAddEmployee.Dock = DockStyle.Fill;
        }

        private void returnToAddUnit()
        {
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = true;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = false;

            pnlAddUnit.Dock = DockStyle.Fill;
        }

        private void returnToEditPersInfo()
        {
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = true;
            pnlReturn.Visible = false;


            pnlEditPersInfo.Dock = DockStyle.Fill;
        }

        private void returnToReturnScreen()
        {
            pnlMainMenu.Visible = false;
            pnlEmployee.Visible = false;
            pnlMovies.Visible = false;
            pnlTransaction.Visible = false;
            pnlLogin.Visible = false;
            pnlSignUp.Visible = false;
            pnlEmployeeModifier.Visible = false;
            pnlEmployeeLogin.Visible = false;
            pnlUnitManagement.Visible = false;
            pnlUnitModifier.Visible = false;
            pnlAddEmployee.Visible = false;
            pnlAddUnit.Visible = false;
            pnlEditPersInfo.Visible = false;
            pnlReturn.Visible = true;

            pnlReturn.Dock = DockStyle.Fill;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

        private void moviesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            returnToMovieMenu();
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
         
        }

        private void btnMainBrowse_Click(object sender, EventArgs e)
        {
            returnToMovieMenu();
        }

        private void btnTransBack_Click(object sender, EventArgs e)
        {
            returnToMovieMenu();
        }

        private void dgMovieView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Clear lables if there are already things in them
            lblTransMovieTitle.Text = "";
            lblTransYear.Text = "";
            lblTransReleaseDate.Text = "";
            lblTransRentPrice.Text = "";
            lblTransType.Text = "";
            lblTransGenre.Text = "";
            lblTransStudio.Text = "";
            lblTransStatus.Text = "";
            lblTransSound.Text = "";
            lblTransVersions.Text = "";
            lblTransRating.Text = "";
            lblTransAspect.Text = "";
            lblTransUPC.Text = "";
            lblTransDirector.Text = "";
            dgTransCast.DataSource = null;
            dgTransCast.Rows.Clear();

            // When a user selects a movie, it pulls up all movie information
            connection = new SqlConnection(connectionstring);
            connection.Open();
            string sql = "SELECT DVD_Title, Year, DVD_ReleaseDate, Price, Type, Genre, Studio, Status, Sound, Versions, Rating, Aspect, UPC FROM Titles WHERE Title_ID = '" + dgMovieView.CurrentRow.Cells[0].Value.ToString() + "'";
            command = new SqlCommand(sql, connection);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                lblTransMovieTitle.Text = datareader[0].ToString();
                lblTransYear.Text = datareader[1].ToString();
                lblTransReleaseDate.Text = datareader[2].ToString();
                lblTransRentPrice.Text = datareader[3].ToString();
                lblTransType.Text = datareader[4].ToString();
                lblTransGenre.Text = datareader[5].ToString();
                lblTransStudio.Text = datareader[6].ToString();
                lblTransStatus.Text = datareader[7].ToString();
                lblTransSound.Text = datareader[8].ToString();
                lblTransVersions.Text = datareader[9].ToString();
                lblTransRating.Text = datareader[10].ToString();
                lblTransAspect.Text = datareader[11].ToString();
                lblTransUPC.Text = datareader[12].ToString();
            }
            
            // Show the Director of the title the user selected
            connection = new SqlConnection(connectionstring);
            connection.Open();
            string sql2 = "SELECT D.Director_Name FROM Directors D JOIN Director_List L ON D.Director_ID = L.Director_ID WHERE Title_ID = '" + dgMovieView.CurrentRow.Cells[0].Value.ToString() + "'";
            command = new SqlCommand(sql2, connection);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                lblTransDirector.Text = datareader[0].ToString();
            }

            // Fill in the cast list
            connection = new SqlConnection(connectionstring);
            connection.Open();
            string sql3 = "SELECT A.Actor_Name FROM Actors A JOIN Actor_List L ON A.Actor_ID = L.Actor_ID WHERE Title_ID = '" + dgMovieView.CurrentRow.Cells[0].Value.ToString() + "'";
            var da = new SqlDataAdapter(sql3, connection);
            var ds = new DataSet();
            da.Fill(ds);
            dgTransCast.DataSource = ds.Tables[0];
            
           
            returnToTransactionMenu();            
        }

        private void btnMovieBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

        private void btnEmpBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

        private void btnMainLogin_Click(object sender, EventArgs e)
        {
            returnToLogin();
        }

        private void btnMainSignUp_Click(object sender, EventArgs e)
        {
            returnToSignUp();
        }

        private void btnLoginBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

        private void btnSignUpBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

  
        private void dgEmployeeEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedEmployee = Convert.ToInt32(dgEmployeeEmployees.CurrentRow.Cells[0].Value.ToString());
            btnEmpRemove.Enabled = true;
        }

        private void btnEmployeeModBack_Click(object sender, EventArgs e)
        {
            returnToEmployeeMenu();
        }

        private void btnLoginLogin_Click(object sender, EventArgs e)
        {
            // Check if user's login is correct
            connection = new SqlConnection(connectionstring);
            connection.Open();
            // Load customer login
            string sql = "SELECT * FROM Customer_Login WHERE username = '" + tbLoginUsername.Text + "' AND password = '" + tbLoginPassword.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(sql, connection);
            DataTable dtCustomerLogin = new DataTable();
            sda.Fill(dtCustomerLogin);

            // Check for customer login
            if (dtCustomerLogin.Rows.Count == 1)
            {
                MessageBox.Show("Customer has logged in!");
                // Clear textboxes
                tbLoginUsername.Text = "";
                tbLoginPassword.Text = "";
                // Tell the system that a customer is logged in
                isCustomerLogin = true;
                // Tell the system the customer ID of the person logged in
                customerID = Convert.ToInt32(dtCustomerLogin.Rows[0]["CustomerID"].ToString());
               
                btnMainLogOut.Visible = true;
                btnMainEditInfo.Visible = true;
                btnMainLogin.Visible = false;
                // Place the name of the user on the main screen
                lblMainUser.Visible = true;
                lblMainUser.Text = "User: " + getCustomerName();

                // Load return titles data grid
                loadReturnTitleDataGrid();
                returnToMainMenu();
            }
            else
            {
                MessageBox.Show("Username or Password incorrect, please try again.");
            }
        }

        private void btnLoginEmpLogin_Click(object sender, EventArgs e)
        {
            // Load employee login
            connection = new SqlConnection(connectionstring);
            connection.Open();
            string sql = "SELECT * FROM Employee_Login WHERE username = '" + tbLoginEmpUsername.Text + "' AND password = '" + tbLoginEmpPassword.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(sql, connection);
            DataTable dtEmployeeLogin = new DataTable();
            sda.Fill(dtEmployeeLogin);

            if (dtEmployeeLogin.Rows.Count == 1)
            {
                MessageBox.Show("Employee has logged in!");
                tbLoginEmpUsername.Text = "";
                tbLoginEmpPassword.Text = "";
                employeeID = Convert.ToInt32(dtEmployeeLogin.Rows[0]["EmployeeID"].ToString());
                isEmployeeLogin = true;

                // Switch statement to return the user to the screen they intially selected
                switch (employeeScreen)
                {
                    case 1:
                        returnToEmployeeMenu();
                        break;
                    case 2:
                        returnToUnitManagement();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Username or Password incorrect, please try again.");
            }

        }

        private void btnLoginEmpBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

        private void employeeManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the employee screen selector so the login screen knows which screen to go back to
            employeeScreen = 1;
            // Check if employee is logged in
            // if so, skip employee log in screen
            if (isEmployeeLogin == true)
            {
                returnToEmployeeMenu();
            }
            else
            {
                returnToEmployeeLogin();
            }
        }

        private void unitManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            employeeScreen = 2;
            // Check if employee is logged in
            // if so, skip employee log in screen
            if (isEmployeeLogin == true)
            {
                returnToUnitManagement();
            }
            else
            {
                returnToEmployeeLogin();
            }
        }

        private void dgUnitManagement_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedUnit = Convert.ToInt32(dgUnitManagement.CurrentRow.Cells[0].Value.ToString());
            btnUnitRemove.Enabled = true;
        }

        private void btnUnitModBack_Click(object sender, EventArgs e)
        {
            returnToUnitManagement();
        }

        private void btnEmployeeModify_Click(object sender, EventArgs e)
        {
            // Modify employee information
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            string sql = "UPDATE Employees SET First_Name=@Fname, Last_Name=@Lname, Address=@Address, Phone_number=@Phone, Email=@Email, status=@Status, Type=@Type, Hourly_rate=@HourlyRate, Hire_Date=@HireDate WHERE employee_ID=@EID";

            command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Fname", tbEmpModFirstName.Text);
            command.Parameters.AddWithValue("@Lname", tbEmpModLastName.Text);
            command.Parameters.AddWithValue("@Address", tbEmpModStreetAddress.Text);
            command.Parameters.AddWithValue("@Phone", tbEmpModPhone.Text);
            command.Parameters.AddWithValue("@Email", tbEmpModEmail.Text);
            command.Parameters.AddWithValue("@Status", tbEmpModStatus.Text);
            command.Parameters.AddWithValue("@Type", tbEmpModType.Text);
            command.Parameters.AddWithValue("@HourlyRate", tbEmpModHourlyRate.Text);
            command.Parameters.AddWithValue("@HireDate", tbEmpModHireDate.Text);
            command.Parameters.AddWithValue("@EID", lblEmpModID.Text);

            answer = command.ExecuteNonQuery();

            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully modified " + answer + " employee");
            loadEmployeeDataGrid();
            returnToEmployeeMenu();
        }

        private void btnUnitModify_Click(object sender, EventArgs e)
        {
            // Modify Unit information
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            string sql = "UPDATE Titles SET DVD_Title=@Title, Year=@Year, DVD_ReleaseDate=@Release, Price=@Price, Type=@Type, Genre=@Genre, Studio=@Studio, Status=@Status, Sound=@Sound, Versions=@Versions, Rating=@Rating, Aspect=@Aspect, UPC=@UPC WHERE Title_ID=@TID";

            command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Title", tbUnitModTitle.Text);
            command.Parameters.AddWithValue("@Year", tbUnitModYear.Text);
            command.Parameters.AddWithValue("@Release", tbUnitModRelease.Text);
            command.Parameters.AddWithValue("@Price", tbUnitModPrice.Text);
            command.Parameters.AddWithValue("@Type", tbUnitModType.Text);
            command.Parameters.AddWithValue("@Genre", tbUnitModGenre.Text);
            command.Parameters.AddWithValue("@Studio", tbUnitModStudio.Text);
            command.Parameters.AddWithValue("@Status", tbUnitModStatus.Text);
            command.Parameters.AddWithValue("@Sound", tbUnitModSound.Text);
            command.Parameters.AddWithValue("@Versions", tbUnitModVersions.Text);
            command.Parameters.AddWithValue("@Rating", tbUnitModRating.Text);
            command.Parameters.AddWithValue("@Aspect", tbUnitModAspect.Text);
            command.Parameters.AddWithValue("@UPC", tbUnitModUPC.Text);
            command.Parameters.AddWithValue("@TID", lblUnitModID.Text);

            answer = command.ExecuteNonQuery();

            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully modified " + answer + " unit.");
            // Load both the unit management grid and title data grid, both use the same part of the database and both need to be updated.
            loadUnitManagementDataGrid();
            loadTitleDataGrid();

            returnToUnitManagement();
        }

        private void btnEmpRemove_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            // SQL statement to delete the last selected employee
            string sql = "DELETE FROM Employees WHERE employee_ID=@EID";
            command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("EID", selectedEmployee);
            answer = command.ExecuteNonQuery();
            command.Dispose();

            // Also deletes the employee's Login record from the Employee_Login table
            string sql2 = "DELETE FROM Employee_Login WHERE EmployeeID=@EID";
            command = new SqlCommand(sql2, connection);
            command.Parameters.AddWithValue("EID", selectedEmployee);
            answer = command.ExecuteNonQuery();

            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully Deleted " + answer + " Employee.");
            loadEmployeeDataGrid();

            btnEmpRemove.Enabled = false;
        }

        private void dgEmployeeEmployees_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Clear all text boxes in the event there is already something there
            tbEmpModFirstName.Text = "";
            tbEmpModLastName.Text = "";
            tbEmpModStreetAddress.Text = "";
            tbEmpModPhone.Text = "";
            tbEmpModEmail.Text = "";
            tbEmpModStatus.Text = "";
            tbEmpModType.Text = "";
            tbEmpModHourlyRate.Text = "";
            tbEmpModHireDate.Text = "";

            // When a user clicks on an employee, it brings them to the employee profile.
            // From here, they should be able to modify employee information.
            lblEmpModID.Text = dgEmployeeEmployees.CurrentRow.Cells[0].Value.ToString();
            tbEmpModFirstName.Text = dgEmployeeEmployees.CurrentRow.Cells[1].Value.ToString();
            tbEmpModLastName.Text = dgEmployeeEmployees.CurrentRow.Cells[2].Value.ToString();
            tbEmpModStreetAddress.Text = dgEmployeeEmployees.CurrentRow.Cells[3].Value.ToString();
            tbEmpModPhone.Text = dgEmployeeEmployees.CurrentRow.Cells[4].Value.ToString();
            tbEmpModEmail.Text = dgEmployeeEmployees.CurrentRow.Cells[5].Value.ToString();
            tbEmpModStatus.Text = dgEmployeeEmployees.CurrentRow.Cells[6].Value.ToString();
            tbEmpModType.Text = dgEmployeeEmployees.CurrentRow.Cells[7].Value.ToString();
            tbEmpModHourlyRate.Text = dgEmployeeEmployees.CurrentRow.Cells[8].Value.ToString();
            tbEmpModHireDate.Text = dgEmployeeEmployees.CurrentRow.Cells[9].Value.ToString();
            returnToEmployeeModifier();
        }

        private void btnEmpAdd_Click(object sender, EventArgs e)
        {
            returnToAddEmp();
        }

        private void btnAddEmpBack_Click(object sender, EventArgs e)
        {
            returnToEmployeeMenu();
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            // Insert vales for the new employee into the employees table
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            string sql = "INSERT INTO Employees VALUES (@Fname, @Lname, @Address, @Phone, @Email, @Status, @Type, @HourlyRate, @HireDate)";

            command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Fname", tbAddEmpFirstName.Text);
            command.Parameters.AddWithValue("@Lname", tblAddEmpLastName.Text);
            command.Parameters.AddWithValue("@Address", tbAddEmpAddress.Text);
            command.Parameters.AddWithValue("@Phone", tbAddEmpPhone.Text);
            command.Parameters.AddWithValue("@Email", tbAddEmpEmail.Text);
            command.Parameters.AddWithValue("@Status", tbAddEmpStatus.Text);
            command.Parameters.AddWithValue("@Type", tbAddEmpType.Text);
            command.Parameters.AddWithValue("@HourlyRate", tbAddEmpHourlyRate.Text);
            command.Parameters.AddWithValue("@HireDate", tbAddEmpHireDate.Text);

            answer = command.ExecuteNonQuery();

            // Dispose of the stuff in command, "Clean out the junk"
            command.Dispose();

            // Sort employees by descending order, then select the top result
            string sql2 = "SELECT TOP 1 employee_ID FROM Employees ORDER BY employee_ID DESC";
            command = new SqlCommand(sql2, connection);
            datareader = command.ExecuteReader();
            int recentEmployeeID = 0;
            // assign the employee id to a variable while the datareader reads
            while (datareader.Read())
            {
                 recentEmployeeID = Convert.ToInt32(datareader[0].ToString());
            }

            // Clean out junk from datareader and command
            datareader.Close();
            command.Dispose();

            // Insert the username, password, and employee ID from above into the employee login table
            string sql3 = "INSERT INTO Employee_Login VALUES (@EID, @Username, @Password)";
            command = new SqlCommand(sql3, connection);

            command.Parameters.AddWithValue("@EID", recentEmployeeID);
            command.Parameters.AddWithValue("@Username", tbAddEmpUsername.Text);
            command.Parameters.AddWithValue("@Password", tbAddEmpPassword.Text);

            answer = command.ExecuteNonQuery();

            // Close connection, don't leave your front door open
            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully entered " + answer + " employee.");

            // Clear out all text boxes
            tbAddEmpFirstName.Text = "";
            tblAddEmpLastName.Text = "";
            tbAddEmpAddress.Text = "";
            tbAddEmpPhone.Text = "";
            tbAddEmpEmail.Text = "";
            tbAddEmpStatus.Text = "";
            tbAddEmpType.Text = "";
            tbAddEmpHourlyRate.Text = "";
            tbAddEmpHireDate.Text = "";
            tbAddEmpUsername.Text = "";
            tbAddEmpPassword.Text = "";

            // Refresh employee datagrid
            loadEmployeeDataGrid();
        }

        private void btnUnitAdd_Click(object sender, EventArgs e)
        {
            returnToAddUnit();
        }

        private void btnAddUnitBack_Click(object sender, EventArgs e)
        {
            returnToUnitManagement();
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            // Insert vales for the new employee into the employees table
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            string sql = "INSERT INTO Titles VALUES (@Title, @Year, @Release, @Price, @Type, @Genre, @Studio, @Status, @Sound, @Versions, @Rating, @Aspect, @UPC)";

            command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Title", tbAddUnitTitle.Text);
            command.Parameters.AddWithValue("@Year", tbAddUnitYear.Text);
            command.Parameters.AddWithValue("@Release", tbAddunitRelease.Text);
            command.Parameters.AddWithValue("@Price", tbAddUnitPrice.Text);
            command.Parameters.AddWithValue("@Type", tbAddUnitType.Text);
            command.Parameters.AddWithValue("@Genre", tbAddUnitGenre.Text);
            command.Parameters.AddWithValue("@Studio", tbAddUnitStudio.Text);
            command.Parameters.AddWithValue("@Status", tbAddUnitStatus.Text);
            command.Parameters.AddWithValue("@Sound", tbAddUnitVersions.Text);
            command.Parameters.AddWithValue("@Versions", tbAddUnitVersions.Text);
            command.Parameters.AddWithValue("@Rating", tbAddUnitRating.Text);
            command.Parameters.AddWithValue("@Aspect", tbAddUnitAspect.Text);
            command.Parameters.AddWithValue("@UPC", tbAddUnitUPC.Text);

            answer = command.ExecuteNonQuery();

            // Close connection, don't leave your front door open
            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully entered " + answer + " unit.");

            // Clear out all text boxes
            tbAddUnitTitle.Text = "";
            tbAddUnitYear.Text = "";
            tbAddunitRelease.Text = "";
            tbAddUnitPrice.Text = "";
            tbAddUnitType.Text = "";
            tbAddUnitGenre.Text = "";
            tbAddUnitStudio.Text = "";
            tbAddUnitStatus.Text = "";
            tbAddUnitSound.Text = "";
            tbAddUnitVersions.Text = "";
            tbAddUnitRating.Text = "";
            tbAddUnitAspect.Text = "";
            tbAddUnitUPC.Text = "";

            // Refresh Unit Management datagrid and Movie datagrid
            loadUnitManagementDataGrid();
            loadTitleDataGrid();
        }

        private void btnUnitRemove_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            // SQL statement to delete the last selected unit
            string sql = "DELETE FROM Titles WHERE Title_ID=@EID";
            command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("EID", selectedUnit);
            answer = command.ExecuteNonQuery();
            command.Dispose();

            // Deletes the unit's information from the director list
            string sql2 = "DELETE FROM Director_List WHERE Title_ID=@EID";
            command = new SqlCommand(sql2, connection);
            command.Parameters.AddWithValue("EID", selectedUnit);
            answer = command.ExecuteNonQuery();
            command.Dispose();

            // Deletes the unit's information from the actor list
            string sql3 = "DELETE FROM Actor_List WHERE Title_ID=@EID";
            command = new SqlCommand(sql3, connection);
            command.Parameters.AddWithValue("EID", selectedUnit);
            answer = command.ExecuteNonQuery();

            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully Deleted " + answer + " Unit.");
            loadUnitManagementDataGrid();
            loadTitleDataGrid();

            btnUnitRemove.Enabled = false;
        }

        private void dgUnitManagement_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Clear every text box, if there is already something there
            lblUnitModID.Text = "";
            tbUnitModTitle.Text = "";
            tbUnitModYear.Text = "";
            tbUnitModRelease.Text = "";
            tbUnitModPrice.Text = "";
            tbUnitModType.Text = "";
            tbUnitModGenre.Text = "";
            tbUnitModStudio.Text = "";
            tbUnitModStatus.Text = "";
            tbUnitModSound.Text = "";
            tbUnitModVersions.Text = "";
            tbUnitModRating.Text = "";
            tbUnitModAspect.Text = "";
            tbUnitModUPC.Text = "";

            connection = new SqlConnection(connectionstring);
            connection.Open();
            string sql = "SELECT * FROM Titles WHERE Title_ID = '" + dgUnitManagement.CurrentRow.Cells[0].Value.ToString() + "'";
            command = new SqlCommand(sql, connection);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                lblUnitModID.Text = datareader[0].ToString();
                tbUnitModTitle.Text = datareader[1].ToString();
                tbUnitModYear.Text = datareader[2].ToString();
                tbUnitModRelease.Text = datareader[3].ToString();
                tbUnitModPrice.Text = datareader[4].ToString();
                tbUnitModType.Text = datareader[5].ToString();
                tbUnitModGenre.Text = datareader[6].ToString();
                tbUnitModStudio.Text = datareader[7].ToString();
                tbUnitModStatus.Text = datareader[8].ToString();
                tbUnitModSound.Text = datareader[9].ToString();
                tbUnitModVersions.Text = datareader[10].ToString();
                tbUnitModRating.Text = datareader[11].ToString();
                tbUnitModAspect.Text = datareader[12].ToString();
                tbUnitModUPC.Text = datareader[13].ToString();
            }

            returnToUnitMod();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            // Insert vales for the new customer into the customer table
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            string sql = "INSERT INTO Customers VALUES (@Status, @MembType, @Lname, @Fname, @Phone, @Address, @Zip, @Email, @Join_Date, @Authorized)";

            command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@Status", "Active");
            command.Parameters.AddWithValue("@MembType", cmbSignUpMemb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@Lname", tbSignUpLastName.Text);
            command.Parameters.AddWithValue("@Fname", tbSignUpFirstName.Text);
            command.Parameters.AddWithValue("@Phone", tbSignUpPhone.Text);
            command.Parameters.AddWithValue("@Address", tbSignUpStreetAddress.Text);
            command.Parameters.AddWithValue("@Zip", tbSignUpZipCode.Text);
            command.Parameters.AddWithValue("@Email", tbSignUpEmail.Text);
            command.Parameters.AddWithValue("@Join_Date", DateTime.Now);
            command.Parameters.AddWithValue("@Authorized", "");


            answer = command.ExecuteNonQuery();

            // Dispose of the stuff in command, "Clean out the junk"
            command.Dispose();

            // Sort Customers by descending order, then select the top result
            string sql2 = "SELECT TOP 1 Customer_Id FROM Customers ORDER BY Customer_Id DESC";
            command = new SqlCommand(sql2, connection);
            datareader = command.ExecuteReader();
            int recentCustomerID = 0;
            // assign the customer id to a variable while the datareader reads
            while (datareader.Read())
            {
                recentCustomerID = Convert.ToInt32(datareader[0].ToString());
            }

            // Clean out junk from datareader and command
            datareader.Close();
            command.Dispose();

            // Insert the username, password, and customer ID from above into the customer login table
            string sql3 = "INSERT INTO Customer_Login VALUES (@CID, @Username, @Password)";
            command = new SqlCommand(sql3, connection);

            command.Parameters.AddWithValue("@CID", recentCustomerID);
            command.Parameters.AddWithValue("@Username", tbSignUpUsername.Text);
            command.Parameters.AddWithValue("@Password", tbSignUpPassword.Text);

            answer = command.ExecuteNonQuery();

            // Close connection, don't leave your front door open
            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully signed up " + answer + " customer.");

            // Clear out all text boxes
            tbSignUpFirstName.Text = "";
            tbSignUpLastName.Text = "";
            tbSignUpPhone.Text = "";
            tbSignUpStreetAddress.Text = "";
            tbSignUpZipCode.Text = "";
            tbSignUpEmail.Text = "";
            tbSignUpUsername.Text = "";
            tbSignUpPassword.Text = "";
            cmbSignUpMemb.SelectedIndex = -1;
        }

        private void btnMainLogOut_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Are you sure you want to log out?","Log Out",MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                isCustomerLogin = false;
                customerID = 0;
                btnMainLogin.Visible = true;
                btnMainLogOut.Visible = false;
                btnMainEditInfo.Visible = false;
                lblMainUser.Visible = false;
                MessageBox.Show(getCustomerName() + " has logged out.");
            }
            else
            {

            }

        }

        private void btnMainEditInfo_Click(object sender, EventArgs e)
        {
            // Fill in customer's personal information
            connection = new SqlConnection(connectionstring);
            connection.Open();
            var sql = "SELECT MembType, Last_Name, First_Name, Phone_Number, Street_Address, Zip_Code, Email FROM Customers WHERE Customer_Id = '" + customerID + "'";
            command = new SqlCommand(sql, connection);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                string MembType = datareader[0].ToString().Trim();
                if (MembType == "Standard")
                {
                    cmbEditPersMem.SelectedIndex = 0;
                }
                else if (MembType == "Premium")
                {
                    cmbEditPersMem.SelectedIndex = 1;
                }
                else
                {
                    cmbEditPersMem.SelectedIndex = -1;
                }
                tbEditPersLName.Text = datareader[1].ToString().Trim();
                tbEditPersFName.Text = datareader[2].ToString().Trim();
                tbEditPersPhone.Text = datareader[3].ToString().Trim();
                tbEditPersAddress.Text = datareader[4].ToString().Trim();
                tbEditPersZip.Text = datareader[5].ToString().Trim();
                tbEditPersEmail.Text = datareader[6].ToString().Trim();
            }

            command.Dispose();
            datareader.Close();
            var sql2 = "SELECT username, password FROM Customer_Login WHERE CustomerID = '" + customerID + "'";
            command = new SqlCommand(sql2, connection);
            datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                tbEditPersUsername.Text = datareader[0].ToString();
                tbEditPersPassword.Text = datareader[1].ToString();
            }

            connection.Close();
            command.Dispose();
            datareader.Close();
            returnToEditPersInfo();
        }

        private void btnEditPersEdit_Click(object sender, EventArgs e)
        {
            // Modify customer information
            connection = new SqlConnection(connectionstring);
            connection.Open();
            string sql = "UPDATE Customers SET MembType=@MembType, Last_Name=@Lname, First_Name=@Fname, Phone_Number=@Phone, Street_Address=@Address, Zip_Code=@Zip, Email=@Email WHERE Customer_Id=@CID";

            command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@MembType", cmbEditPersMem.SelectedItem.ToString());
            command.Parameters.AddWithValue("@Lname", tbEditPersLName.Text);
            command.Parameters.AddWithValue("@Fname", tbEditPersFName.Text);
            command.Parameters.AddWithValue("@Phone", tbEditPersPhone.Text);
            command.Parameters.AddWithValue("@Address", tbEditPersAddress.Text);
            command.Parameters.AddWithValue("@Zip", tbEditPersZip.Text);
            command.Parameters.AddWithValue("@Email", tbEditPersEmail.Text);
            command.Parameters.AddWithValue("@CID", customerID);

            command.ExecuteNonQuery();

            command.Dispose();

            // Modify the username, password, and customer ID in the login table
            string sql2 = "UPDATE Customer_Login SET username=@Username, password=@Password WHERE CustomerID=@CID";
            command = new SqlCommand(sql2, connection);

            command.Parameters.AddWithValue("@CID", customerID);
            command.Parameters.AddWithValue("@Username", tbEditPersUsername.Text);
            command.Parameters.AddWithValue("@Password", tbEditPersPassword.Text);

            command.ExecuteNonQuery();

            connection.Close();
            command.Dispose();
            // Update the customer name on the main menu, in case they changed it
            lblMainUser.Text = "User: " + getCustomerName();

            MessageBox.Show("Successfully modified " + getCustomerName() + "'s personal information.");
            returnToMainMenu();
        }

        private void btnEditPersBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }

        private void btnEditPersDelete_Click(object sender, EventArgs e)
        {
            DialogResult = MessageBox.Show("Doing this will delete your profile and log you out." +
                 Environment.NewLine + "Are you sure?", "Delete Customer Profile", MessageBoxButtons.YesNo);
            if (DialogResult == DialogResult.Yes)
            {
                // Delete user from database
                connection = new SqlConnection(connectionstring);
                connection.Open();
                // SQL statement to delete the last selected employee
                string sql = "DELETE FROM Customers WHERE Customer_Id=@CID";
                command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("CID", customerID);
                command.ExecuteNonQuery();
                command.Dispose();

                // Also deletes the employee's Login record from the Employee_Login table
                string sql2 = "DELETE FROM Customer_Login WHERE CustomerID=@CID";
                command = new SqlCommand(sql2, connection);
                command.Parameters.AddWithValue("CID", customerID);
                command.ExecuteNonQuery();

                connection.Close();
                command.Dispose();

                // Log out the user
                isCustomerLogin = false;
                customerID = 0;
                btnMainLogin.Visible = true;
                btnMainLogOut.Visible = false;
                btnMainEditInfo.Visible = false;
                lblMainUser.Visible = false;

                MessageBox.Show("Successfully deleted " + getCustomerName() + "'s profile.");
                returnToMainMenu();
            }
            else
            {
                returnToMainMenu();
            }

            
        }

        private void btnTransRent_Click(object sender, EventArgs e)
        {
            if (isCustomerLogin == true)
            {
                // Insert vales for the new customer into the customer table
                connection = new SqlConnection(connectionstring);
                connection.Open();
                int answer;
                string sql = "INSERT INTO Rentals VALUES (@Customer_ID, @Title_ID, @Date_Rented, @Date_Due)";

                command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Customer_ID", customerID);
                command.Parameters.AddWithValue("@Title_ID", dgMovieView.CurrentRow.Cells[0].Value.ToString());
                command.Parameters.AddWithValue("@Date_Rented", DateTime.Today);
                command.Parameters.AddWithValue("@Date_Due", DateTime.Today.AddDays(3));
                answer = command.ExecuteNonQuery();

                // Close connection, don't leave your front door open
                connection.Close();
                command.Dispose();

                MessageBox.Show(getCustomerName() + " rented " + answer + " title.");
            }
            else
            {
                MessageBox.Show("You must be logged in to rent a movie.");
            }
        }

        private void btnMainReturn_Click(object sender, EventArgs e)
        {
            if (isCustomerLogin == true)
            {
                returnToReturnScreen();
            }
            else
            {
                MessageBox.Show("You must be logged in to return a title.");
            }
        }

        private void btnReturnTitle_Click(object sender, EventArgs e)
        {
            connection = new SqlConnection(connectionstring);
            connection.Open();
            int answer;
            // SQL statement to delete the selected rental
            string sql = "DELETE FROM Rentals WHERE Customer_ID=@CID";
            command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("CID", customerID);
            answer = command.ExecuteNonQuery();

            connection.Close();
            command.Dispose();

            MessageBox.Show("Successfully Deleted " + answer + " Rental.");
            btnUnitRemove.Enabled = false;
            loadReturnTitleDataGrid();
        }

        private void dgReturn_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedReturnTitle = Convert.ToInt32(dgReturn.CurrentRow.Cells[0].Value.ToString());
            btnReturnTitle.Enabled = true;
        }

        private void btnReturnBack_Click(object sender, EventArgs e)
        {
            returnToMainMenu();
        }
    }
}
