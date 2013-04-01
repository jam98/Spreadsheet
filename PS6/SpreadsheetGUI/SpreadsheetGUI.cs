using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SS;
using System.Text.RegularExpressions;
using SpreadsheetUtilities;
using System.IO;


namespace SpreadsheetGUI
{
    public partial class SSGUI : Form
    {
        //Several class-wide variables.
        private Spreadsheet _sheet;
        private string filepath;



        //A SSGUI constructor without any arguments. This simply creates a new spreadsheet.
        public SSGUI()
        {
            InitializeComponent();

            //The validity and normalization functions
            Func<string, bool> isValid = delegate(string s)
            { return Regex.IsMatch(s, @"[a-zA-Z][1-9][0-9]?"); };
            Func<string, string> normalize = delegate(string s)
            { return s.ToUpper(); };

            //Link with actual spread sheet
            _sheet = new Spreadsheet(isValid, normalize, "ps6");

            //Display the first selection correctly.
            DisplaySelection(SSPanel);
        }



        //A constructor that takes a file path and attempts to construct a spreadsheet from it. Things may still go wrong.
        public SSGUI(string file)
        {
            InitializeComponent();

            //The validity and normalization functions
            Func<string, bool> isValid = delegate(string s)
            { return Regex.IsMatch(s, @"[a-zA-Z][1-9][0-9]?"); };
            Func<string, string> normalize = delegate(string s)
            { return s.ToUpper(); };

            //Link with actual spread sheet
            _sheet = new Spreadsheet(file, isValid, normalize, "ps6");

            //Set the filepath to the file the spreadsheet opened
            filepath = file;

            //Loads the values into panel
            LoadPanelValues(new HashSet<string>(_sheet.GetNamesOfAllNonemptyCells()));

            //Display the first selection correctly.
            DisplaySelection(SSPanel);
        }



        //Called when the Help button is pressed. This simply displays a popup with information.
        private void ShowHelp(object sender, EventArgs e)
        {
            MessageBox.Show("To open a new spreadsheet, select File > New.\n"
                +"To open an existing spreadsheet, select File > Open.\n"
                +"To save a spreadsheet, select File > Save.\n"
                +"To save a spreadsheet to a new location, select File > Save As...\n"
                +"To close a spreadsheet, select File > Close.\n"
                +"\n"
                +"To select a new cell, click the box in it's row and column.\n"
                +"\n"
                +"To edit a cell's contents, click the Content Text Box in the upper right of the spreadsheet, edit, then press "
                    +"the enter key.\n"
                +"\n"
                +"Content formats this spreadsheet accepts:\n"
                +"      Strings (Hello world, Type here, Etc...)\n"
                +"      Doubles (2, 1.0, .031)\n"
                +"      Formulas (=2+3, =A1 * (10 + 2), =J31/u7 + a1 * 3)"
                );
        }



        //Open a new spreadsheet.
        private void LaunchNewSpreadsheet(object sender, EventArgs e)
        {
            // Tell the application context to run the form on the same
            // thread as the other forms.
            SSApplicationContext.getAppContext().RunForm(new SSGUI());
        }



        //Display the appropriate Cell, Value and Content in each text box.
        private void DisplaySelection(SS.SpreadsheetPanel sender)
        {
            string cell = SelectionToCell(sender);
            string contents = _sheet.GetCellContents(cell).ToString();
            string value = _sheet.GetCellValue(cell).ToString();
            CellTextbox.Text = cell;
            ContentsTextbox.Text = contents;
            ValueTextbox.Text = value;
        }



        //Called when the user enters anything into the content textbox. This tends to spam itself.
        private void ContentTextInput(object sender, KeyPressEventArgs e)
        {
            //We only care if the enter key is hit while editing the context textbox, so we just return if any other
            //key is pressed.
            if (e.KeyChar != (char)Keys.Enter)
                return;

            //Based on the current selection, fetch the cell name and set it's contents to what's currently in the ContentTextBox.
            //Then reload the values for all cells affected by the change.
            try { LoadPanelValues(_sheet.SetContentsOfCell(SelectionToCell(SSPanel), ContentsTextbox.Text)); }
            //We need to define many of the exceptions the above code may through, as it pretty much uses everything
            //that might break.
            catch (FormulaFormatException)
            {
                MessageBox.Show("An incorrectly formatted cell was included in the formula, please try again.");
            }
            catch (CircularException)
            {
                MessageBox.Show("A circular dependency was discovered, please try again.");
            }
            catch (Exception)
            {
                MessageBox.Show("Something abnormal went wrong when trying to set the cell's contents.");
            }
            //Reloads the selection Textboxes
            DisplaySelection(SSPanel);
        }



        //Displays the values for the cells given in the set within panel.
        private void LoadPanelValues(ISet<string> cells)
        {
            //Variables
            int col;
            int row;
            string value;

            try
            {
                foreach (string modcell in cells)
                {
                    CellToSelection(out col, out row, modcell);
                    value = _sheet.GetCellValue(modcell).ToString();
                    SSPanel.SetValue(col, row, value);
                }
            }

            //We need to define many of the exceptions the above code may through, as it pretty much uses everything
            //that might break.
            catch (FormulaFormatException)
            {
                MessageBox.Show("An incorrectly formatted cell was included in the formula, please try again.");
            }
            catch (CircularException)
            {
                MessageBox.Show("A circular dependency was discovered, please try again.");
            }
            catch (Exception)
            {
                MessageBox.Show("Something abnormal went wrong when trying to set the cell's contents.");
            }
        }



        //A helper method to convert the currently selected cell in the panel to the currect cell name for use in Spreadsheet.
        private string SelectionToCell(SpreadsheetPanel panel)
        {
            int row, col;
            string cell;
            panel.GetSelection(out col, out row);
            cell = char.ConvertFromUtf32(col + 65) + (row + 1);
            return cell;
        }



        //The reverse operation to the one above, this takes a cell name given by Spreadsheet and converts it to the numbered
        //column and row, each beginning at zero.
        private void CellToSelection(out int col, out int row, string cell)
        {
            col = char.ConvertToUtf32(cell, 0)-65;
            row = Convert.ToInt32(cell.Substring(1))-1;
        }



        //This is to prevent a spreadsheet from being closed before it's saved.
        private void CloseSheet(object sender, EventArgs e)
        {
            //Checks to see if a spreadsheet has been edited since it was first created or last saved. If so, display warning with the
            //option to either cancel or continue.
            if (_sheet.Changed)
            {
                DialogResult yesorno = MessageBox.Show("This operation may result in the loss of unsaved data. Do you still wish to proceed?",
                    "Warning", MessageBoxButtons.YesNo);
                if (yesorno == DialogResult.No)
                    return;
            }
            
            //Close the spreadsheet.
            Close();
        }



        private void OpenClick(object sender, EventArgs ea)
        {
            DialogResult result = OpenDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = OpenDialog.FileName;
                try
                {
                    //Create a new SSGUI with a new Spreadsheet created from the file.
                    SSGUI newsheet = new SSGUI(file);
                    SSApplicationContext.getAppContext().RunForm(newsheet);
                }

                //Once again, we need to catch any exceptions we may incounter in trying to open the spreadsheet.
                catch (SpreadsheetReadWriteException e)
                {
                    MessageBox.Show("File failed to open. " + e.Message);
                }
            }
        }



        private void SaveClick(object sender, EventArgs e)
        {
            //Test to see if there's an existing file. If so, save to that without a form to select; otherwise allow the
            //user to select where to save the spreadsheet.
            if (filepath != null)
            {
                SaveSS(filepath);
            }
            else
            {
                SaveAsClick("", new EventArgs());
            }
        }



        private void SaveAsClick(object sender, EventArgs e)
        {
            DialogResult result = SaveAsDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = SaveAsDialog.FileName;
                SaveSS(file);
                filepath = file;
            }
        }



        //Create a new SSGUI with a new Spreadsheet created from the file.
        private void SaveSS(string file)
        {
            try
            {
                _sheet.Save(file);
            }

            //Once again, we need to catch any exceptions we may incounter in trying to open the spreadsheet.
            catch (SpreadsheetReadWriteException e)
            {
                MessageBox.Show("File failed to save. " + e.Message);
            }
        }
    }

    /// <summary>
    /// Keeps track of how many top-level forms are running
    /// </summary>
    class SSApplicationContext : ApplicationContext
    {
        // Number of open forms
        private int formCount = 0;

        // Singleton ApplicationContext
        private static SSApplicationContext appContext;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SSApplicationContext()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext.
        /// </summary>
        public static SSApplicationContext getAppContext()
        {
            if (appContext == null)
            {
                appContext = new SSApplicationContext();
            }
            return appContext;
        }

        /// <summary>
        /// Runs the form
        /// </summary>
        public void RunForm(Form form)
        {
            // One more form is running
            formCount++;

            // When this form closes, we want do the following
            form.FormClosed += (o, e) => { if (--formCount <= 0) ExitThread(); };

            // Run the form
            form.Show();
        }

    }
}
