using SS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace PS4Test
{
    
    
    /// <summary>
    ///This is a test class for SpreadsheetTest and is intended
    ///to contain all SpreadsheetTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpreadsheetTest
    {


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


        // Try not having any nonempty cells
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest1()
        {
            Spreadsheet target = new Spreadsheet();
            List<string> actual = new List<string>(target.GetNamesOfAllNonemptyCells());
            HashSet<string> expected = new HashSet<string>();
            Assert.AreEqual(actual.Count, 0);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Try one nonempty cell
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest2()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", 1.ToString());
            List<string> actual = new List<string>(target.GetNamesOfAllNonemptyCells());
            HashSet<string> expected = new HashSet<string>() { "A1" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Try multiple nonempty cells
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsTest3()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", 1.ToString());
            target.SetContentsOfCell("B1", 2.ToString());
            List<string> actual = new List<string>(target.GetNamesOfAllNonemptyCells());
            HashSet<string> expected = new HashSet<string>() { "A1", "B1" };
            Assert.AreEqual(actual.Count, 2);
            Assert.IsTrue(expected.SetEquals(actual));
        }


        //Try with name null
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTest1()
        {
            Spreadsheet target = new Spreadsheet();
            string name = null;
            object actual = target.GetCellContents(name);
        }

        //Try several invalid names. 
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTest2()
        {
            Spreadsheet target = new Spreadsheet();
            string name = "a";
            object actual = target.GetCellContents(name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTest3()
        {
            Spreadsheet target = new Spreadsheet();
            string name = "a1a";
            object actual = target.GetCellContents(name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTest4()
        {
            Spreadsheet target = new Spreadsheet();
            string name = "aa";
            object actual = target.GetCellContents(name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTest5()
        {
            Spreadsheet target = new Spreadsheet();
            string name = "1";
            object actual = target.GetCellContents(name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsTest6()
        {
            Spreadsheet target = new Spreadsheet();
            string name = "a 1";
            object actual = target.GetCellContents(name);
        }

        //Make sure that changing the returned value doesn't actually change a cell's contents.
        [TestMethod]
        public void GetCellContentsTest8()
        {
            Spreadsheet target = new Spreadsheet();
            string name = "A1";
            target.SetContentsOfCell(name, 1.0.ToString());
            double contents = (double) target.GetCellContents(name);
            double expected = contents;
            contents = 2.0;
            Assert.AreEqual(expected, (double)target.GetCellContents(name));
        }

        //FOR EACH
        // Try setting a cell's contents twice to see if multiple cells with the same name are created.
        [TestMethod]
        public void SetContentsOfCellTest1()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", "1");
            target.SetContentsOfCell("A1", 2.ToString());
            List<string> actual = new List<string>(target.GetNamesOfAllNonemptyCells());
            HashSet<string> expected = new HashSet<string>() { "A1" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Try null names.
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest2()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell(null, "1");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest3()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell(null, 2.ToString());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest4()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell(null, new Formula("1+2").ToString());
        }

        // Try invalid names. 
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest5()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("a1a", "Hello");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest6()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("a", .0003.ToString());
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetContentsOfCellTest7()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("2a", new Formula("3*(1+2)").ToString());
        }

        // Try null strings/formulas.
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellTest8()
        {
            Spreadsheet target = new Spreadsheet();
            string s = null;
            target.SetContentsOfCell("A1", s);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetContentsOfCellTest9()
        {
            Spreadsheet target = new Spreadsheet();
            string f = null;
            target.SetContentsOfCell("A1", f);
        }

        // Try several perfectly valid operations.
        [TestMethod]
        public void SetContentsOfCellTest10()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("bass23", "1");
            target.SetContentsOfCell("Z9", 2.ToString());
            target.SetContentsOfCell("Z9", new Formula("(2.003+(4+5))*.4").ToString());
            Assert.IsTrue(true);
        }

        // Check that this returns a set, and that the set contains cells who's value depends on this cell directly and indirectly
        [TestMethod]
        public void SetContentsOfCellTest12()
        {
            Spreadsheet target = new Spreadsheet();
            SortedSet<string> actual1 = new SortedSet<string>(target.SetContentsOfCell("A1", "=B2 + C3"));
            SortedSet<string> actual2 = new SortedSet<string>(target.SetContentsOfCell("B2", "=D4-1"));
            SortedSet<string> actual3 = new SortedSet<string>(target.SetContentsOfCell("D4", "=F5"));
            SortedSet<string> actual4 = new SortedSet<string>(target.SetContentsOfCell("E6", 2.9.ToString()));
            SortedSet<string> expected1 = new SortedSet<string>() { "A1" };
            SortedSet<string> expected2 = new SortedSet<string>() { "B2", "A1"};
            SortedSet<string> expected3 = new SortedSet<string>() { "D4", "B2", "A1" };
            SortedSet<string> expected4 = new SortedSet<string>() { "E6" };
            Assert.AreEqual(actual1.Count, 1);
            Assert.AreEqual(actual2.Count, 2);
            Assert.AreEqual(actual3.Count, 3);
            Assert.AreEqual(actual4.Count, 1);
            Assert.IsTrue(expected1.SetEquals(actual1));
            Assert.IsTrue(expected2.SetEquals(actual2));
            Assert.IsTrue(expected3.SetEquals(actual3));
            Assert.IsTrue(expected4.SetEquals(actual4));
        }
 
        // Try setting up a dependency, changing the dependent cell to not depend on any, and check if the dependency still exists.
        [TestMethod]
        public void SetContentsOfCellTest13()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", new Formula("B2").ToString());
            target.SetContentsOfCell("A1", "check");
            SortedSet<string> actual = new SortedSet<string>(target.SetContentsOfCell("B2", "mate"));
            SortedSet<string> expected = new SortedSet<string>() { "B2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Try creating several circular dependencies directly, indirectly and extremely indirectly.
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellTestFormula1()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", "=A1");
        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellTestFormula2()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", "=B1");
            target.SetContentsOfCell("B1", "=A1");
        }
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetContentsOfCellTestFormula3()
        {
            Spreadsheet target = new Spreadsheet();
            target.SetContentsOfCell("A1", "=B1");
            target.SetContentsOfCell("B1", "=C1");
            target.SetContentsOfCell("C1", "=D1");
            target.SetContentsOfCell("D1", "=A1");
        }

        // Check Cells exist for several various Cell Names, and that any cell being accessed for the first time has an empty string in its contents.
        [TestMethod]
        public void GetCellContentsTestLast1()
        {
            Spreadsheet target = new Spreadsheet();
            string name1 = "A1";
            string name2 = "A1";
            string name3 = "A1";
            string expected1 = "";
            string expected2 = "";
            string expected3 = "";
            Assert.AreEqual(expected1, (string)target.GetCellContents(name1));
            Assert.AreEqual(expected2, (string)target.GetCellContents(name2));
            Assert.AreEqual(expected3, (string)target.GetCellContents(name3));
        }
    }
}
