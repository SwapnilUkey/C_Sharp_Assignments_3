using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Assignment_3
{
    public partial class Mad4RoadForm : Form
    {
        // Variables Required for System Password and dir location and for creating UniCode
        string PasswordString = "2Fast4U#";
        string DirLocation = System.AppDomain.CurrentDomain.BaseDirectory , TxtFileName = "Data.txt";
        string UniChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int PassWordAttempts = 0, Attemptsremaining = 3;

        // Const Declaration for Intrestrates abd Years
        const decimal IR40One = 6.00m, IR40Three = 6.50m, IR40Five = 7.00m, IR40Seven = 7.50m;
        const decimal IRA40One = 8.00m, IRA40Three = 8.50m, IRA40Five = 9.00m, IRA40Seven = 9.00m;
        const decimal IRA80One = 8.50m, IRA80Three = 8.75m, IRA80Five = 9.10m, IRA80Seven = 9.25m;
        const int Year1 = 1, Year3 = 3, Year5 = 5, Year7 = 7, OneYear = 12;

        // Variable for calculating Total Repayment and total interst and principal
        decimal TotalRepaymentForOne, TotalRepaymentForThree, TotalRepaymentForFive, TotalRepaymentForSeven = 0m;
        decimal MonthOne, MonthThree, MonthFive, MonthSeven = 0m;
        decimal TotalInt1, TotalInt3, TotalInt5, TotalInt7 = 0m;
        int SelectedYear = 0 , UniKeyLen = 6 ,CountOrder = 0 ;

        // Variable used for Switch Box -- ListBox 
        decimal SelectedMonthRate1, SelectedMonthRate2, SelectedMonthRate3, SelectedMonthRate4 = 0.00m, SelectedMonInt = 0.00m, SelectedTotalIntrest = 0.00m, TotalRepayment = 0.00m;
        decimal SelectedMonthRate = 0.00m, InvestmentValue = 0m;

        // Investor Data variable
        string InvestorName = "", PostalCode = "", Email = "", UniKeyString = "";
        int PhoneNumber;

        public Mad4RoadForm()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {

            // checking password 
            if (PasswordTextBox.Text.Equals(PasswordString))
            {
                LoginGroupBox.Visible = false;
                InvestmentGroupBox.Visible = true;
                InvestmentTextBox.Focus();
                InvestorDetailsGroupBox.Visible = true;
                SearchTransactionGroupBox.Visible = true;
                SummaryGroupBox.Visible = true;
                CompPicBox.Visible = true;
            }
            else
            {
                // putting tab on how many times did user input the password
                PassWordAttempts++;
                Attemptsremaining--;
                if (PassWordAttempts > 2)
                {
                    MessageBox.Show("You have reached your limit", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Please Enter Correct Password You have " + Attemptsremaining + " Attempts", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PasswordTextBox.Focus();
                }
            }
        }

        // compound intrest calculaiton function
        public decimal TotalRepaymentCal(decimal Principal, decimal Rate, int Year)
        {
            decimal TotalRepayment = Principal * (decimal)Math.Pow((double)(1 + Rate/100),(double)Year);
            TotalRepayment = Math.Round(TotalRepayment, 2);
            return TotalRepayment;
        }

        // Total interst calculation function
        public decimal TotalIntrest(decimal Totalrepayment, decimal Principal)
        {
            decimal TotalIntrest = Totalrepayment - Principal;
            return TotalIntrest;
        }

        // Monthly intresty calculation function 
        public decimal MonthlyIntrest(decimal TotalIntrest,int Year)
        {
            decimal MonthlyInt = Math.Round(TotalIntrest / (Year * OneYear),2);
            return MonthlyInt;
        }

        // Fuction for Updating List box
        public void UpdateTRlistBox(decimal InvestmentValue)
        {
            if (InvestmentValue < 40000 && InvestmentValue > 10000)
            {
                TRlistBox.Items.Add("    " + Year1 + " Year             " + IR40One     + " %            "   + MonthOne.ToString("C", new CultureInfo("en-GB"))      +   "          "  + TotalInt1.ToString("C", new CultureInfo("en-GB")) + "        " + TotalRepaymentForOne.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year3 + " Year             " + IR40Three   + " %            "   + MonthThree.ToString("C", new CultureInfo("en-GB"))    +   "          "  + TotalInt3.ToString("C", new CultureInfo("en-GB")) + "       " + TotalRepaymentForThree.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year5 + " Year             " + IR40Five    + " %            "   + MonthFive.ToString("C", new CultureInfo("en-GB"))     +   "          "  + TotalInt5.ToString("C", new CultureInfo("en-GB")) + "       " + TotalRepaymentForFive.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year7 + " Year             " + IR40Seven   + " %            "   + MonthSeven.ToString("C", new CultureInfo("en-GB"))    +   "          "  + TotalInt7.ToString("C", new CultureInfo("en-GB")) + "       " + TotalRepaymentForSeven.ToString("C", new CultureInfo("en-GB"))) ;
            }
            if (InvestmentValue > 40000 && InvestmentValue < 80000)
            {
                TRlistBox.Items.Add("    " + Year1 + " Year             " + IRA40One    + " %            "  +  MonthOne.ToString("C", new CultureInfo("en-GB"))     +   "           " + TotalInt1.ToString("C", new CultureInfo("en-GB")) + "        " + TotalRepaymentForOne.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year3 + " Year             " + IRA40Three  + " %            "  +  MonthThree.ToString("C", new CultureInfo("en-GB"))   +   "           " + TotalInt3.ToString("C", new CultureInfo("en-GB")) + "        " + TotalRepaymentForThree.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year5 + " Year             " + IRA40Five   + " %            "  +  MonthFive.ToString("C", new CultureInfo("en-GB"))    +   "           " + TotalInt5.ToString("C", new CultureInfo("en-GB")) + "        " + TotalRepaymentForFive.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year7 + " Year             " + IRA40Seven  + " %            "  +  MonthSeven.ToString("C", new CultureInfo("en-GB"))   +   "           " + TotalInt7.ToString("C", new CultureInfo("en-GB")) + "        " + TotalRepaymentForSeven.ToString("C", new CultureInfo("en-GB")));

            }
            if (InvestmentValue > 80000 && InvestmentValue < 100000)
            {
                TRlistBox.Items.Add("    " + Year1 + " Year             " + IRA80One    + " %            " + MonthOne.ToString("C", new CultureInfo("en-GB"))       + "           " + TotalInt1.ToString("C", new CultureInfo("en-GB")) +   "         " + TotalRepaymentForOne.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year3 + " Year             " + IRA80Three  + " %            " + MonthThree.ToString("C", new CultureInfo("en-GB"))     + "           " + TotalInt3.ToString("C", new CultureInfo("en-GB")) +   "        " + TotalRepaymentForThree.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year5 + " Year             " + IRA80Five   + " %            " + MonthFive.ToString("C", new CultureInfo("en-GB"))      + "           " + TotalInt5.ToString("C", new CultureInfo("en-GB")) +   "        " + TotalRepaymentForFive.ToString("C", new CultureInfo("en-GB")));
                TRlistBox.Items.Add("    " + Year7 + " Year             " + IRA80Seven  + " %            " + MonthSeven.ToString("C", new CultureInfo("en-GB"))     + "           " + TotalInt7.ToString("C", new CultureInfo("en-GB")) +   "        " + TotalRepaymentForSeven.ToString("C", new CultureInfo("en-GB")));
            }
        }

        private void DisplayButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Checking The condition for user input 
                InvestmentValue = decimal.Parse(InvestmentTextBox.Text);
                try
                {
                    if (InvestmentValue < 40000 && InvestmentValue >= 10000)
                    {
                        TotalRepaymentForOne = TotalRepaymentCal(InvestmentValue,IR40One,Year1);
                        TotalRepaymentForThree = TotalRepaymentCal(InvestmentValue, IR40Three, Year3);
                        TotalRepaymentForFive = TotalRepaymentCal(InvestmentValue, IR40Five, Year5);
                        TotalRepaymentForSeven = TotalRepaymentCal(InvestmentValue, IR40Seven, Year7);

                        SelectedMonthRate1 = IR40One;
                        SelectedMonthRate2 = IR40Three;
                        SelectedMonthRate3 = IR40Five;
                        SelectedMonthRate4 = IR40Seven;

                    }
                    else if (InvestmentValue >= 40000 && InvestmentValue < 80000)
                    {
                        TotalRepaymentForOne = TotalRepaymentCal(InvestmentValue, IRA40One, Year1);
                        TotalRepaymentForThree = TotalRepaymentCal(InvestmentValue, IRA40Three, Year3);
                        TotalRepaymentForFive = TotalRepaymentCal(InvestmentValue, IRA40Five, Year5);
                        TotalRepaymentForSeven = TotalRepaymentCal(InvestmentValue, IRA40Seven, Year7);

                        SelectedMonthRate1 = IRA40One;
                        SelectedMonthRate2 = IRA40Three;
                        SelectedMonthRate3 = IRA40Five;
                        SelectedMonthRate4 = IRA40Seven;
                    }
                    else if (InvestmentValue >= 80000 && InvestmentValue <= 99999)
                    {
                        TotalRepaymentForOne = TotalRepaymentCal(InvestmentValue, IRA80One, Year1);
                        TotalRepaymentForThree = TotalRepaymentCal(InvestmentValue, IRA80Three, Year3);
                        TotalRepaymentForFive = TotalRepaymentCal(InvestmentValue, IRA80Five, Year5);
                        TotalRepaymentForSeven = TotalRepaymentCal(InvestmentValue, IRA80Seven, Year7);

                        SelectedMonthRate1 = IRA80One;
                        SelectedMonthRate2 = IRA80Three;
                        SelectedMonthRate3 = IRA80Five;
                        SelectedMonthRate4 = IRA80Seven;
                    }

                    if (InvestmentValue <= 10000 || InvestmentValue >= 100000)
                    {
                        MessageBox.Show("PLease Enter value greater than $ 10000 and less than $ 100000", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        InvestmentTextBox.Focus();
                        DisplayButton.Enabled = true;
                    }
                    else
                    {

                        // Calculating total intrest
                        TotalInt1 = TotalIntrest(TotalRepaymentForOne, InvestmentValue);
                        TotalInt3 = TotalIntrest(TotalRepaymentForThree, InvestmentValue);
                        TotalInt5 = TotalIntrest(TotalRepaymentForFive, InvestmentValue);
                        TotalInt7 = TotalIntrest(TotalRepaymentForSeven, InvestmentValue);
                        
                        // Calculating monthly Intrest
                        MonthOne = MonthlyIntrest(TotalInt1, Year1);
                        MonthThree = MonthlyIntrest(TotalInt3, Year3);
                        MonthFive = MonthlyIntrest(TotalInt5, Year5);
                        MonthSeven = MonthlyIntrest(TotalInt7, Year7);

                        UpdateTRlistBox(InvestmentValue);
                        DisplayButton.Enabled = false;
                    }
                }
                catch
                {
                    DisplayButton.Enabled = true;
                    //
                }
            }
            catch
            {
                // Showing Message box if user put invalid input
                MessageBox.Show("PLease Enter valid Input", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InvestmentTextBox.Focus();
            }
        }

        // Fuction for generating unique key fro transaction id
        public string GenerateUniKey()
        {
            Random rnd = new Random();
            string UniKey = "";
            for (int UniLoop = 0; UniLoop < UniKeyLen ; UniLoop++)
            {
                int number = rnd.Next(1, UniChar.Length);
                char SingleUni = UniChar[number];
                UniKey += SingleUni;
            }
            return UniKey;
        }

        private void ProccedButton_Click(object sender, EventArgs e)
        {
            try
            {
                // geting data from user which option they have choosen
                decimal.Parse(InvestmentTextBox.Text);
                int SelectedOptionIndex = 0;
                if ((TRlistBox.SelectedIndex != -1))
                {
                    SelectedOptionIndex = TRlistBox.SelectedIndex;
                    switch (SelectedOptionIndex)
                    {
                        case 0:
                            SelectedYear = Year1; SelectedMonthRate = SelectedMonthRate1; SelectedMonInt = MonthOne; SelectedTotalIntrest = TotalInt1; TotalRepayment = TotalRepaymentForOne;
                            break;
                        case 1:
                            SelectedYear = Year3; SelectedMonthRate = SelectedMonthRate2; SelectedMonInt = MonthThree; SelectedTotalIntrest = TotalInt3; TotalRepayment = TotalRepaymentForThree;
                            break;
                        case 2:
                            SelectedYear = Year5; SelectedMonthRate = SelectedMonthRate3; SelectedMonInt = MonthFive; SelectedTotalIntrest = TotalInt5; TotalRepayment = TotalRepaymentForFive;
                            break;
                        case 3:
                            SelectedYear = Year7; SelectedMonthRate = SelectedMonthRate4; SelectedMonInt = MonthSeven; SelectedTotalIntrest = TotalInt7; TotalRepayment = TotalRepaymentForSeven;
                            break;
                    }

                    //MessageBox.Show(GenerateUniKey(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    UniKeyString = GenerateUniKey();
                    TranKeyTextBox.Text = UniKeyString;
                    InvestorDetailsGroupBox.Enabled = true;
                    SubmitButton.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Please Choose appropriate Option from list", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TRlistBox.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Please Enter Numeric Values in Investment Box", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                InvestmentTextBox.Focus();
            }
        }

        // function fro Validating the email
        public bool EmailValidation(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        // function for updating  the txt file 
        public void UpdateTxtfile()
        {
            string FileLoaction = Path.Combine(DirLocation, TxtFileName);
            if (!File.Exists(FileLoaction))
            {
                using (StreamWriter writer = File.AppendText(FileLoaction))
                {
                    //writer.WriteLine("");
                    writer.WriteLine(UniKeyString);
                    writer.WriteLine(Email);
                    writer.WriteLine(InvestorName);
                    writer.WriteLine(PhoneNumber);
                    writer.WriteLine(PostalCode);
                    writer.WriteLine(InvestmentValue);
                    writer.WriteLine(SelectedMonInt);
                    writer.WriteLine(TotalRepayment);
                    writer.WriteLine(SelectedYear * OneYear);
                    writer.WriteLine(SelectedMonthRate);
                    writer.Close();
                }
            }
            else
            {
                using (StreamWriter writer = File.AppendText(FileLoaction))
                {
                    //writer.WriteLine();
                    writer.WriteLine(UniKeyString);
                    writer.WriteLine(Email);
                    writer.WriteLine(InvestorName);
                    writer.WriteLine(PhoneNumber);
                    writer.WriteLine(PostalCode);
                    writer.WriteLine(InvestmentValue);
                    writer.WriteLine(SelectedMonInt);
                    writer.WriteLine(TotalRepayment);
                    writer.WriteLine(SelectedMonthRate);
                    writer.WriteLine(SelectedYear * OneYear);
                    writer.Close();
                }
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                try 
                {
                    // Getting user input for investor name
                    InvestorName = InvNameTextBox.Text;
                    if (!(InvestorName.Length > 4))
                    {
                        throw new Exception("");
                    }

                    try
                    {
                        PostalCode = PostCodeTextBox.Text;
                        if (!(PostalCode.Length > 4 && PostalCode.Length < 7))
                        {
                            throw new Exception("");
                        }
                        try
                        {
                            PhoneNumber = int.Parse(PNumTextBox.Text);
                            if (PNumTextBox.Text.Length != 8)
                            {
                                throw new Exception("");
                            }

                            try
                            {
                                // calling email validation
                                Email = EmailTextBox.Text;
                                if (!EmailValidation(Email))
                                {
                                    throw new Exception("");
                                }
                                try
                                {
                                    // checking for confirmation from user 
                                    DialogResult BookConfYesNo = MessageBox.Show(
                                        "Following are the Booking Information" + "\n" +
                                            "Year         :  " + SelectedYear + "\n" +
                                            "Monthly Intrest Rate    :  " + SelectedMonthRate + "\n" +
                                            "Monthly Intrest  :  " + SelectedMonInt.ToString("C", new CultureInfo("en-GB")) + "\n" +
                                            "Total Intrest  :  " + SelectedTotalIntrest.ToString("C", new CultureInfo("en-GB")) + "\n" +
                                            "Total Repayment  :  " + TotalRepayment.ToString("C", new CultureInfo("en-GB")) + "\n"
                                           + "Press Yes To Confirm Booking OR Press No to go back"
                                            , "Boking Information Confrimation", MessageBoxButtons.YesNo);
                                    if (BookConfYesNo == DialogResult.Yes)
                                    {
                                        // if order is selected yes and year is greater than five years
                                        CountOrder = CountOrder + 1;
                                        if ((SelectedYear > 3))
                                        {
                                            MessageBox.Show(
                                                "Congratulations !!!!!!!!!!!!!!!" + "\n" +
                                                " You Recieve Free AA Road Side Assistance For the duration of the loan" + "\n" + "\n" +
                                                "Following are the Booking Information" + "\n" +
                                                "Year         :  " + SelectedYear + "\n" +
                                                "Monthly Intrest Rate    :  " + SelectedMonthRate + "\n" +
                                                "Monthly Intrest  :  " + SelectedMonInt.ToString("C", new CultureInfo("en-GB")) + "\n" +
                                                "Total Intrest  :  " + SelectedTotalIntrest.ToString("C", new CultureInfo("en-GB")) + "\n" +
                                                "Total Repayment  :  " + TotalRepayment.ToString("C", new CultureInfo("en-GB")) + "\n"
                                               + "Press Yes To Confirm Booking OR Press No to go back"
                                                , "Boking Information Confrimation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show(
                                                "Following are the Booking Information" + "\n" +
                                                "Year         :  " + SelectedYear + "\n" +
                                                "Monthly Intrest Rate    :  " + SelectedMonthRate + "\n" +
                                                "Monthly Intrest  :  " + SelectedMonInt.ToString("C", new CultureInfo("en-GB")) + "\n" +
                                                "Total Intrest  :  " + SelectedTotalIntrest.ToString("C", new CultureInfo("en-GB")) + "\n" +
                                                "Total Repayment  :  " + TotalRepayment.ToString("C", new CultureInfo("en-GB")) + "\n"
                                               + "Press Yes To Confirm Booking OR Press No to go back"
                                                , "Boking Information Confrimation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        UpdateTxtfile();

                                        SearchTransactionGroupBox.Enabled = true;
                                        SummaryGroupBox.Enabled = true;
                                        SummaryLabel.Enabled = true;
                                        InvNameTextBox.Clear();
                                        PostCodeTextBox.Clear();
                                        PNumTextBox.Clear();
                                        EmailTextBox.Clear();
                                        TranKeyTextBox.Clear();
                                        SubmitButton.Enabled = false;
                                        TRlistBox.Items.Clear();
                                        InvestmentTextBox.Clear();
                                        DisplayButton.Enabled = true;
                                        InvestmentTextBox.Focus();

                                    }
                                    else if (BookConfYesNo == DialogResult.No)
                                    {
                                        ;
                                    }
                                }
                                catch
                                {
                                    ;
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Email is Not Valid", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                EmailTextBox.Focus();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Phone Number is Not Valid it should be 8 digit", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            PNumTextBox.Focus();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Post Code should be five to six text", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        PostCodeTextBox.Focus();
                    }
                }
                catch
                {
                    MessageBox.Show("Name should be more than five characthers", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    InvNameTextBox.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Some Crazzzy Stuff happennnnnn!!!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        // Investor Details search with email id
        public void InvestorDataSearchEmail(string SearchText)
        {
            try
            {
                string FileLoaction = Path.Combine(DirLocation, TxtFileName);
                StreamReader InvestorDataReader = new StreamReader(FileLoaction);
                String PreviousLineText = "";
                bool MessageFound = false;
                using (InvestorDataReader)
                {
                    string CurrentLinetext = InvestorDataReader.ReadLine();
                    while (!InvestorDataReader.EndOfStream)
                    {
                        bool FoundDataset = SearchText.Equals(CurrentLinetext, StringComparison.CurrentCultureIgnoreCase);
                        if (FoundDataset && EmailValidation(SearchText))
                        {
                            if (EmailRadioButton.Checked)
                            {
                                SearchOutputListBox.Items.Add(PreviousLineText);
                                SearchOutputListBox.Items.Add(SearchText);
                                for (int Counter = 0; Counter < 8; Counter++)
                                {
                                    SearchOutputListBox.Items.Add(InvestorDataReader.ReadLine());
                                }
                                //break;
                            }
                            MessageFound = true;
                            CurrentLinetext = InvestorDataReader.ReadLine();
                        }
                        else
                        {
                            PreviousLineText = CurrentLinetext;
                            CurrentLinetext = InvestorDataReader.ReadLine();
                        }
                    }
                    if (!MessageFound)
                    {
                        MessageBox.Show(SearchText + " Email id is Not available In Data", "Information", MessageBoxButtons.OK);
                    }
                    InvestorDataReader.Close();

                }
            }
            catch (FileNotFoundException errorMsg)
            {
                MessageBox.Show("Error, " + errorMsg.Message);
            }
        }

        // Search Function for getting data from Uni traansaction id 
        public void InvestorDataSearchUniCode(string SearchText)
        {
            try
            {
                string FileLoaction = Path.Combine(DirLocation, TxtFileName);
                StreamReader InvestorDataReader = new StreamReader(FileLoaction);
                bool MessageFound = false;
                using (InvestorDataReader)
                {
                    string CurrentLinetext = InvestorDataReader.ReadLine();
                    while(CurrentLinetext != null)
                    {
                        bool FoundDataset = SearchText.Equals(CurrentLinetext, StringComparison.CurrentCultureIgnoreCase);
                        if (FoundDataset && !EmailValidation(SearchText))
                        {
                            if (TransactionRadioButton.Checked)
                            {
                                SearchOutputListBox.Items.Add("Transaction Id : " + CurrentLinetext);
                                SearchOutputListBox.Items.Add("Email          : " + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Investor Nmae  : " + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Phone Number   : " + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Postal Code    : " + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Principal      : £" + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Monthly Intrest: £" + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Total Repayment: £" + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Intrest Rate   : " + InvestorDataReader.ReadLine());
                                SearchOutputListBox.Items.Add("Total Month    : " + InvestorDataReader.ReadLine());
                                
                                MessageFound = true;
                                break;
                            }
                        }
                        else
                        {
                            CurrentLinetext = InvestorDataReader.ReadLine(); 
                        }


                    }
                if (!MessageFound)
                {
                    MessageBox.Show(SearchText + " Transaction Id is Not Available In Data", "Information", MessageBoxButtons.OK);
                }
                InvestorDataReader.Close();

                }
            }
            catch (FileNotFoundException errorMsg)
            {
                // error Message if file not found
                MessageBox.Show("Error, " + errorMsg.Message);
            }
        }


        private void SearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Search button condition according to radio button check
                SearchOutputListBox.Items.Clear();
                if (TransactionRadioButton.Checked)
                {
                    InvestorDataSearchUniCode(SearchTextBox.Text);
                }
                else if (EmailRadioButton.Checked)
                {
                    InvestorDataSearchEmail(SearchTextBox.Text);
                }
                else
                {
                    MessageBox.Show("Please select Email or Transaction Number to search !", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

            }
        }

        // function for getting all lines in txt file to calculate order
        public int LineCounterForTxt()
        {
            int lineCounter = 0;
            string FileLoaction = Path.Combine(DirLocation, TxtFileName);
            using (var reader = new StreamReader(FileLoaction))
            {
                while (reader.ReadLine() != null)
                {
                    lineCounter++;
                }
                return lineCounter;
            }
        }

        private void SummaryButton_Click(object sender, EventArgs e)
        {
            // initalizing the vaiable fro calculating summary
            double SumPrincipal = 0, SumMonthInt = 0, SumTotalRepay = 0 , SumAvegMonth = 0;
            int TotalOrders = (LineCounterForTxt() / 10);
            // calculating summary from txt file
            StreamReader TransactionFile = File.OpenText(Path.Combine(DirLocation, TxtFileName));
            while (!TransactionFile.EndOfStream)
            {
                for (int i = 0; i <= 4; i++)
                {
                    TransactionFile.ReadLine();
                }
                double SumTotalPrincipal = double.Parse(TransactionFile.ReadLine());
                SumPrincipal += SumTotalPrincipal;
                SumPrincipal = Math.Round(SumPrincipal, 2);

                double SumTotalMonInt = double.Parse(TransactionFile.ReadLine());
                SumMonthInt += SumTotalMonInt;
                SumMonthInt = Math.Round(SumMonthInt, 2);

                double TotRepay = double.Parse(TransactionFile.ReadLine());
                SumTotalRepay += TotRepay;
                SumTotalRepay = Math.Round(SumTotalRepay);

                double TotDuration = double.Parse(TransactionFile.ReadLine());
                SumAvegMonth += TotDuration;
                SumAvegMonth = Math.Round(SumAvegMonth);

                TransactionFile.ReadLine();
            }
            TransactionFile.Close();

            // adding data in summary in list box
            SummaryListBox.Items.Add("Total Orders                :" + TotalOrders);
            SummaryListBox.Items.Add("Total Principal           :" + SumPrincipal.ToString("C", new CultureInfo("en-GB")));
            SummaryListBox.Items.Add("Average Principal         :" + (SumPrincipal / TotalOrders).ToString("C", new CultureInfo("en-GB")));
            SummaryListBox.Items.Add("Total Monthly Intrest     :" + SumMonthInt.ToString("C", new CultureInfo("en-GB")));
            SummaryListBox.Items.Add("Average Monthly Intrest   :" + (SumMonthInt / TotalOrders).ToString("C", new CultureInfo("en-GB")));
            SummaryListBox.Items.Add("Total Repayment           :" + (SumTotalRepay).ToString("C", new CultureInfo("en-GB")));
            SummaryListBox.Items.Add("Total Repayment           :" + (SumTotalRepay / TotalOrders).ToString("C", new CultureInfo("en-GB")));
            SummaryListBox.Items.Add("Average Month             :" + (SumAvegMonth / TotalOrders));
            SummaryButton.Enabled = false; 

        }

        private void Mad4RoadForm_Load(object sender, EventArgs e)
        {
            // if there is no file in location and having no data in it 
            // Summary and search will not enable 
            string FileLoaction = Path.Combine(DirLocation, TxtFileName);
            if (!File.Exists(FileLoaction))
            {
                SummaryGroupBox.Enabled = false;
                SearchTransactionGroupBox.Enabled = false;
            }
            else
            {
                if (new FileInfo(FileLoaction).Length > 0)
                {
                    SummaryGroupBox.Enabled = true;
                    SearchTransactionGroupBox.Enabled = true;
                }
            }

        }

        private void SumClearButton_Click(object sender, EventArgs e)
        {
            SummaryListBox.Items.Clear();
            SummaryButton.Enabled = true;
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            TRlistBox.Items.Clear();
            InvestmentTextBox.Clear();
            InvestmentTextBox.Focus();
            DisplayButton.Enabled = true;
        }

        private void ClearSearchButton_Click(object sender, EventArgs e)
        {
            EmailRadioButton.Checked = false;
            TransactionRadioButton.Checked = false;
            SearchTextBox.Clear();
            SearchOutputListBox.Items.Clear();
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void InvestmentTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
