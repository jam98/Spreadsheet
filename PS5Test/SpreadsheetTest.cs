using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace PS5Test
{
    
    
    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {

        #region Test Class Junk
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
        #endregion



        #region GetCellValue

        /// <summary>
        ///A test for GetCellValue
        ///</summary>
        [TestMethod()]
        public void GetCellValueTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            string name;
            string content;
            // Try a value of a string from string
            name = "A1";
            content = "Hello";
            s.SetContentsOfCell(name, content);
            string expected1 = content;
            string actual1 = (string)s.GetCellValue(name);
            Assert.AreEqual(expected1, actual1);

            // Try a value of a double from a double
            name = "B1";
            content = "3";
            s.SetContentsOfCell(name, content);
            double expected2 = 3;
            double actual2 = (double)s.GetCellValue(name);
            Assert.AreEqual(expected2, actual2);

            // Try a value of a double from a Formula
            name = "C1";
            content = "=3+1";
            s.SetContentsOfCell(name, content);
            double expected3 = 4;
            double actual3 = (double)s.GetCellValue(name);
            Assert.AreEqual(expected3, actual3);

            // Try all possible FormulaError
            name = "D1";
            content = "=1/0";
            s.SetContentsOfCell(name, content);
            object actual4 = s.GetCellValue(name);
            Assert.IsInstanceOfType(actual4, typeof(FormulaError));

            // Try chaining several formulas together then changing the value of cell. Test before and after.
            s.SetContentsOfCell("E1", "=F1 + 1");
            s.SetContentsOfCell("F1", "=G1 + 1");
            s.SetContentsOfCell("G1", "1");
            Assert.AreEqual(3, (double)s.GetCellValue("E1"));
            Assert.AreEqual(2, (double)s.GetCellValue("F1"));

            s.SetContentsOfCell("G1", "2");
            Assert.AreEqual(4, (double)s.GetCellValue("E1"));
            Assert.AreEqual(3, (double)s.GetCellValue("F1"));

            // Try switching a cell that has a formula depending on it to string.
            s.SetContentsOfCell("J1", "This is a string");
            s.SetContentsOfCell("K1", "=J1");
            Assert.IsInstanceOfType(s.GetCellValue("K1"), typeof(FormulaError));
        }
        #endregion



        #region GetSavedVersion

        /// <summary>
        ///A test for GetSavedVersion
        ///</summary>
        [TestMethod()]
        public void GetSavedVersionTest()
        {
            // Returns the version information of the spreadsheet saved in the named file.
            string filename = "GetSavedVersionTest.xml";
            string version = "onesandcaps";

            AbstractSpreadsheet s2 = new Spreadsheet(x => (Regex.IsMatch(x, @"^[a-zA-Z]+[1]$")) ? (true) : (false), y => y.ToUpper(), version);
            s2.Save(filename);
            AbstractSpreadsheet s = new Spreadsheet();

            string actual = s.GetSavedVersion(filename);
            string expected = version;
            Assert.AreEqual(expected, actual);
        }
        #endregion



        #region Save

        /// <summary>
        ///A test for Save
        ///</summary>
        [TestMethod()]
        public void SaveTest()
        {
            // Write a spreadsheet to a file, then read the spreadsheet back in and compare
            string input1 = "a1";
            string input2 = "B1";
            string input3 = "C1";
            string content1 = "hello";
            string content2 = "3";
            string content3 = "=b1 + 2";
            string filename = "SaveTest.xml";
            string version = "onesandcaps";
            AbstractSpreadsheet s = new Spreadsheet(x => (Regex.IsMatch(x, @"^[a-zA-Z]+[1]$")) ? (true) : (false), y => y.ToUpper(), version);
            s.SetContentsOfCell(input1, content1);
            s.SetContentsOfCell(input2, content2);
            s.SetContentsOfCell(input3, content3);
            s.Save(filename);
            AbstractSpreadsheet p = new Spreadsheet(filename, x => (Regex.IsMatch(x, @"^[a-zA-Z]+[1]$")) ? (true) : (false), y => y.ToUpper(), version);
            Assert.IsTrue(
                new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(
                new HashSet<string>(p.GetNamesOfAllNonemptyCells()))
                );

            // Try a file that doesn't exist.
            try { AbstractSpreadsheet q = new Spreadsheet("nothere", x => true, y => y, "yes"); Assert.Fail(); }
            catch (SpreadsheetReadWriteException) { };

            // If the version of the saved spreadsheet does not match the version parameter to the constructor, an exception should be thrown.  
            AbstractSpreadsheet w = new Spreadsheet(x => true, y => y, "one");
            w.Save("NotSameVersion.xml");
            try { AbstractSpreadsheet z = new Spreadsheet("NotSameVersion.xml", x => true, y => y, "two"); Assert.Fail(); }
            catch (SpreadsheetReadWriteException) { };
        }
        #endregion



        #region SetContentsOfCell

        /// <summary>
        /// Tests for SetContentsOfCell
        ///</summary>
        //ALL TYPES OF INPUTS
        [TestMethod()]
        public void SetContentsOfCellDouble()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }

        [TestMethod()]
        public void SetContentsOfCellString()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }

        [TestMethod()]
        public void SetContentsOfCellFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(new Formula("3"), f);
            Assert.AreNotEqual(new Formula("2"), f);
        }

        // Make sure not to set a cell to a formula when that formula contains a circular dependency.
        // In other words check for circular dependencies /first/, then assign the cell content.
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellFormulaCircularUnset()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A2", "15");
                s.SetContentsOfCell("A3", "30");
                s.SetContentsOfCell("A2", "=A3*A1");
            }
            catch (CircularException e)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw e;
            }
        }

        // Must reset empty cells to previous state.
        [TestMethod()]
        public void SetContentsOfCellStringReset()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }
        #endregion



        #region Changed
        [TestMethod()]
        public void Changed()
        {
            // Make sure it starts false
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.Changed);

            // Modify a file, see if changed is true; Use SetContentsOfCell
            s.SetContentsOfCell("Z7", "1.5");
            Assert.IsTrue(s.Changed);

            // Save a file after modifying, see if it switches back to false
            s.Save("changed_test.xml");
            Assert.IsFalse(s.Changed);
        }
        #endregion



        #region IsValid
        [TestMethod()]
        public void IsValid()
        {
            AbstractSpreadsheet s = new Spreadsheet( x => (Regex.IsMatch(x, @"^[a-zA-Z]+[1]$"))?(true):(false), y => y.ToUpper(), "onesandcaps");
            // Try only allowing 1's as digit, try valid and invalid
            // Try multiple if not all ways to enter a cell name, definitely try several invalid formula variables

            //VALID
            s.SetContentsOfCell("dseWd1", "3");
            s.SetContentsOfCell("A1", "2");
            s.SetContentsOfCell("B1", "3");
            s.SetContentsOfCell("AA1", "3");
            s.SetContentsOfCell("BB1", "3");
            s.SetContentsOfCell("CC1", "3");
            s.SetContentsOfCell("DD1", "3");
            s.SetContentsOfCell("D1", "Again");
            s.GetCellContents("Z1");
            s.GetCellContents("AA1");

            //INVALID
            try {s.GetCellContents("Z23423"); Assert.Fail();} catch(InvalidNameException){};
            try {s.SetContentsOfCell("Z7", "1.5"); Assert.Fail();} catch(InvalidNameException){};
            try {s.SetContentsOfCell("Z77", "hello"); Assert.Fail();} catch(InvalidNameException){};
            try {s.SetContentsOfCell("Z7", "=3*5"); Assert.Fail();} catch(InvalidNameException){};
            try {s.SetContentsOfCell("asD21", "2"); Assert.Fail();} catch(InvalidNameException){};
            try {s.SetContentsOfCell("F1", "=asdf2334+z+$"); Assert.Fail();} catch(FormulaFormatException){};
            try {s.SetContentsOfCell("f22", "=asdf2334+z+$"); Assert.Fail();} catch{};
        }
        #endregion



        #region Normalize
        [TestMethod()]
        public void Normalize()
        {
            AbstractSpreadsheet s = new Spreadsheet(x => (Regex.IsMatch(x, @"^[a-zA-Z]+[1]$")) ? (true) : (false), y => y.ToUpper(), "onesandcaps");
            // Try only allowing 1's as digit, try normalize with valid and invalid and see if conversion works.
            // Try multiple if not all ways to enter a cell name
            // Try strange but valid variables in formula, convert with some normalize method.
            // Test normalizing things then testing them against IsValid.

            //FORMULA NORMALIZE
            s.SetContentsOfCell("A1", "=aa1+bb1");
            Assert.IsTrue(new HashSet<string>(((Formula)s.GetCellContents("A1")).GetVariables()).SetEquals(new HashSet<string>(){"AA1", "BB1"}));

            try { s.SetContentsOfCell("B1", "=a23+b45"); Assert.Fail(); }
            catch (FormulaFormatException) { };

            s.SetContentsOfCell("C1", "=asdfasd1+lksadhf1");
            Assert.IsTrue(new HashSet<string>(((Formula)s.GetCellContents("C1")).GetVariables()).SetEquals(new HashSet<string>() 
                { "asdfasd1".ToUpper(), "lksadhf1".ToUpper() }));

            s.SetContentsOfCell("AA1", "1");
            try { s.SetContentsOfCell("D1", "=AA4332+3"); Assert.Fail(); }
            catch (FormulaFormatException) { };
        }
        #endregion
    }
}
