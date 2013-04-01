﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace CodedUITest
{
    /// <summary>
    /// Summary description for CodedUITest
    /// </summary>
    [CodedUITest]
    public class CodedUITest
    {
        public CodedUITest()
        {
        }

        [TestMethod]
        public void CodedUITestMethod1()
        {
            // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
            // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
            //this.UIMap.OpenAndClose();
            //this.UIMap.AllSteps();
            this.UIMap.EnterADouble();
            this.UIMap.AssertMethod1();
            this.UIMap.RecordedMethod1();
            this.UIMap.AssertMethod2();
            this.UIMap.RecordedMethod2();
            this.UIMap.AssertMethod3();
            this.UIMap.RecordedMethod3();
            this.UIMap.AssertMethod4();
            this.UIMap.RecordedMethod4();
            this.UIMap.AssertMethod5();
            this.UIMap.RecordedMethod5();
            this.UIMap.AssertMethod6();
            this.UIMap.RecordedMethod6();
            this.UIMap.AssertMethod7();
            this.UIMap.RecordedMethod7();
            this.UIMap.AssertMethod8();
            this.UIMap.RecordedMethod8();
            this.UIMap.AssertMethod9();
            this.UIMap.RecordedMethod9();
            this.UIMap.AssertMethod10();
            this.UIMap.RecordedMethod10();
            this.UIMap.AssertMethod11();
            this.UIMap.RecordedMethod11();
            this.UIMap.AssertMethod12();
            this.UIMap.RecordedMethod12();
            this.UIMap.AssertMethod13();
            this.UIMap.RecordedMethod13();

        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //    // For more information on generated code, see http://go.microsoft.com/fwlink/?LinkId=179463
        //}

        #endregion

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
        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if ((this.map == null))
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}
