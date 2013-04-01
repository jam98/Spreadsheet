using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;

//There is one global change to the specification that will require minor (but important) modifications to your Spreadsheet and Fomula classes.
//They have to do when cell names are valid and when they are treated as equal.

//AbstractSpreadsheet contains a delegate IsValid.  A string s is considered to be a valid cell name if (1) s consists of one or more letters 
//followed by one or more digits and (2) IsValid(s) returns true.  Any place in your code (such as in Spreadsheet and Formula) that needs to judge 
//whether a string is a valid cell name must use this new definition.
    //Implemented by including a check with IsValid in CorrectInput.

//AbstractSpreadsheet also contains a delegate Normalize.  Its job is to convert all cell names into a standard form.  For example, it might 
//convert all the letters in a name to upper case.  Throughout your implementation, when a valid cell name n comes in as a parameter, you should 
//replace it with Normalize(n).  Similarly, when a string representing a formula comes in as a parameter, you should replace each cell name n 
//that it contains with Normalize(n).
    //Implemented by replacing name with the normalized cell name return value of CorrectInput, and replacing any formulas inputted with each
    //of their variables normalized through the use to NormalizeFunction.

//AbstractSpreadsheet now provides a three-argument constructor that your Spreadsheet constructors will need to use.
    //Used in each of the Spreadsheet constructors

//Your zero-argument constructor should create an empty spreadsheet that imposes no extra validity conditions, normalizes every cell name to itself, 
//and has version "default".
    //Implemented by calling the AbstractSpreadSheet constructor and passing in the apropriate parameters.

//You should add a three-argument constructor to the Spreadsheet class.  Just like the zero-argument constructor, it should create 
//an empty spreadsheet.  However, it should allow the user to provide a validity delegate (first parameter), a normalization delegate (second parameter), 
//and a version (third parameter).
    //Implemented

//You should add a four-argument constructor to the Spreadsheet class.  It should allow the user to provide a string representing a path to a 
//file (first parameter), a validity delegate (second parameter), a normalization delegate (third parameter), and a version (fourth parameter).  
//It should read a saved spreadsheet from a file (see the Save method) and use it to construct a new spreadsheet.  The new spreadsheet should use 
//the provided validity delegate, normalization delegate, and version.
//If anything goes wrong when reading the file, the constructor should throw a SpreadsheetReadWriteException with an explanatory message.  
//If the version of the saved spreadsheet does not match the version parameter to the constructor, an exception should be thrown.  
//If any of the names contained in the saved spreadsheet are invalid, an exception should be thrown.  
//If any invalid formulas or circular dependencies are encountered, an exception should be thrown.  
//If there are any problems opening, reading, or closing the file, an exception should be thrown.  
//There are doubtless other things that can go wrong.
    //Implemented

//There is a new abstract method SetContentsOfCell.  The exisiting SetCellContents methods are now protected.
    //Implemented by following the description of SetContentsOfCell in AbstractSpreadsheet to the tee.

//There is a new abstract method GetCellValue. 
    //Implemented

//There is a new abstract property Changed.
    //Implemented by setting it to true whenever a SetCellContents method is used and setting it to false when the spreadsheet 
    //is written to a file.

//There is a new abstract method Save.
    //Implemented according to specification given in AbstractSpreadsheet.

//There is a new abstract method GetSavedVersion.
    //Implemented

//Code Coverage is near 100%
    //CHECK

//Each method and variable has an XML Comment header
    //Implemented

//The variable names form is correct
    //It is indeed.

//Must reset empty cells to previous state
    //Fixed

//Make sure not to set a cell to a formula when that formula contains a circular dependency.
//In other words check for circular dependencies /first/, then assign the cell content.
    //Fixed



namespace SS
{
    /// <summary>
    /// A class implementing AbstractSpreadsheet.
    /// <seealso cref="AbstractSpreadsheet"/>
    /// <include file='Spreadsheet.XML' path="doc/members/member[@name='T:SS.AbstractSpreadsheet']/*" />
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Keeps track of which cells depend on which for their values.
        /// </summary>
        protected DependencyGraph dependencies;
        
        /// <summary>
        /// Associates Cell Names to cells. A single Cell Name will never point to more than one cell.
        /// </summary>
        protected SortedDictionary<string, Cell> cells;

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.Changed"/>
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        /// The zero argument constructor; this creates an empty spreadsheet that imposes no extra validity conditions, 
        /// normalizes every cell name to itself, and has version "default".
        /// </summary>
        public Spreadsheet()
            : this(f => true, s => s, "default") 
        {
            //Nothing here; this simply calls the three argument constructor with the appropriate parameters.
        }

        /// <summary>
        /// The three argument constructor; this allows the user to provide a validity delegate, a normalization delegate, 
        /// and a version (third parameter).
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            //Setup data structures that keep track of which cells depend on which, and the association between cells and cell names.
            dependencies = new DependencyGraph();
            cells = new SortedDictionary<string, Cell>();
            Changed = false;
        }

        /// <summary>
        /// The four argument constructor; this allows the user to provide a string representing a path to a file (first parameter), 
        /// a validity delegate (second parameter), a normalization delegate (third parameter), and a version (fourth parameter).  
        /// This constructor then reads a saved spreadsheet from the file and uses it to construct a new spreadsheet.  The new spreadsheet
        /// uses the provided validity delegate, normalization delegate, and version.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="isValid"></param>
        /// <param name="normalize"></param>
        /// <param name="version"></param>
        public Spreadsheet(string filepath, Func<string, bool> isValid, Func<string, string> normalize, string version) 
            : this(isValid, normalize, version)
        {
            //This will create a Spreadsheet like normal.
            //Check that the file version is equal to the current spreadsheet version.
            if (Version != GetSavedVersion(filepath))
                throw new SpreadsheetReadWriteException(String.Format("Spreadsheet version entered ({0}) doesn't match the version of the spreadsheet "
                    + "contained in the file ({1}).", Version, GetSavedVersion(filepath)));

            //For each cell in the file, use SetContentsOfCell to read in.
            try
            {
                using (XmlReader reader = XmlReader.Create(filepath))
                {
                    string name = null;
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    break;

                                case "cell":
                                    break;

                                case "name":
                                    reader.Read();
                                    name = CorrectInput(reader.Value);
                                    break;
                                case "contents":
                                    if (name == null)
                                        throw new SpreadsheetReadWriteException("File wasn't written correctly.");

                                    reader.Read();
                                    SetContentsOfCell(name, reader.Value);
                                    name = null;
                                    break;
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
            Changed = false;
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.GetSavedVersion"/>
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    reader.ReadToFollowing("spreadsheet");
                    return reader["version"];
                }
            }
            catch (Exception e)
            {
                
                throw new SpreadsheetReadWriteException(e.Message);
            }
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.Save"/>
        /// </summary>
        /// <param name="filename"></param>
        public override void Save(string filename)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach(string name in cells.Keys)
                    {
                        object c = GetCellContents(name);

                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", name);
                        writer.WriteElementString("contents", (c.GetType() == typeof(Formula)) ? ("="+c.ToString()) : c.ToString());
                        writer.WriteEndElement();
                    }
                    
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
            Changed = false;
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.GetNamesOfAllNonemptyCells"/>
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            return cells.Keys;
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.GetCellContents"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            name = CorrectInput(name);
            return GetCell(name).Contents;
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.GetCellValue"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override object GetCellValue(string name)
        {
            name = CorrectInput(name);
            return GetCell(name).Value;
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.SetContentsOfCell"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            // Private Variable
            ISet<string> set;

            // If content is null, throws an ArgumentNullException.
            // Otherwise, if name is null or invalid, throws an InvalidNameException.
            name = CorrectInput(name, content);

            // Otherwise, if content parses as a double, the contents of the named
            // cell becomes that double.
            double d;
            if (Double.TryParse(content, out d))
                set = SetCellContents(name, d);

            // Otherwise, if content begins with the character '=', an attempt is made
            // to parse the remainder of content into a Formula f using the Formula
            // constructor.  
            else if (content.Length > 0 && content[0] == '=')
            {
                // There are then three possibilities:
                //   (1) If the remainder of content cannot be parsed into a Formula, a 
                //       SpreadsheetUtilities.FormulaFormatException is thrown.
                //       Be sure to check the validity of and normalize any variables.
                //   (2) Otherwise, if changing the contents of the named cell to be f
                //       would cause a circular dependency, a CircularException is thrown.
                Formula f = new Formula(content.Substring(1));
                f = NormalizeFormula(f);
                try
                {
                    foreach (string v in f.GetVariables())
                        CorrectInput(v);
                }
                catch (InvalidNameException)
                {
                    throw new FormulaFormatException(
                        String.Format("One or more variables in the formula '{0}' contained in cell {1} aren't valid.", f.ToString(), name));
                }

                //   (3) Otherwise, the contents of the named cell becomes f.
                set = SetCellContents(name, f);
            }

            // Otherwise, the contents of the named cell becomes content.
            else
                set = SetCellContents(name, content);

            // Recalculate the values of any cell dependent on the named cell, including the named cell itself.
            CalculateCellValues(name);

            //Remove any name associations to cell, reset to how it was before the cell was added.
            if (content == "")
            {
                cells.Remove(name);
            }

            // If an exception is not thrown, the method returns a set consisting of
            // name plus the names of all other cells whose value depends, directly
            // or indirectly, on the named cell.
            // For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
            // set {A1, B1, C1} is returned.
            return set;
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.SetCellContents(String, double)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        protected override ISet<String> SetCellContents(String name, double number)
        {
            // If name is null or invalid, throws an InvalidNameException.
            name = CorrectInput(name);

            // Otherwise, the contents of the named cell becomes number.
            dependencies.ReplaceDependees(name, new HashSet<string>());
            GetCell(name).Contents = number;
            Changed = true;        

            // The method returns a set consisting of name plus the names of all other 
            // cells whose value depends, directly or indirectly, on the named cell.
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.SetCellContents(String, String)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        protected override ISet<String> SetCellContents(String name, String text)
        {
            // If text is null, throws an ArgumentNullException.
            // Otherwise, if name is null or invalid, throws an InvalidNameException.
            name = CorrectInput(name, text);

            // Otherwise, the contents of the named cell becomes text.
            Changed = true;
            dependencies.ReplaceDependees(name, new HashSet<string>());
            GetCell(name).Contents = text;
            // The method returns a set consisting of name plus the names of all other 
            // cells whose value depends, directly or indirectly, on the named cell.
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.SetCellContents(String, Formula)"/>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        protected override ISet<String> SetCellContents(String name, Formula formula)
        {
            // If formula parameter is null, throws an ArgumentNullException.
            // Otherwise, if name is null or invalid, throws an InvalidNameException.
            name = CorrectInput(name, formula);
            formula = NormalizeFormula(formula);

            // Otherwise, if changing the contents of the named cell to be the formula would cause a 
            // circular dependency, throws a CircularException.
            IEnumerable<string> previous = dependencies.GetDependees(name);
            try 
	        {	        
                dependencies.ReplaceDependees(name, formula.GetVariables());
                GetCellsToRecalculate(name);
	        }
            catch (CircularException)
	        {
                dependencies.ReplaceDependees(name, previous);
                throw new CircularException();
	        };

            //foreach (string v in formula.GetVariables())
            //    foreach (string d in GetCellsToRecalculate(v))
            //        if (name.Equals(d))
            //        {
            //            dependencies.ReplaceDependees(name, previous);
            //            throw new CircularException();
            //        }

            // Otherwise, the contents of the named cell becomes formula.
            Changed = true;
            GetCell(name).Contents = formula;

            // The method returns a Set consisting of name plus the names of all other 
            // cells whose value depends, directly or indirectly, on the named cell.
            return new HashSet<string>(GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Recalculates the value of any named cell in set.
        /// </summary>
        /// <param name="name"></param>
        protected void CalculateCellValues(string name)
        {
            foreach (string s in GetCellsToRecalculate(name))
            {
                //Fetch the cell corresponding to s.
                Cell c = GetCell(s);

                //If the cell's contents are a formula, set the value to the evaluation of the formula.
                if (c.Contents.GetType() == typeof(Formula))
                    c.Value = ((Formula)c.Contents).Evaluate(delegate(string var)
                        { 
                            object value = GetCellValue(var); 
                            if (value.GetType() != typeof(Double)) 
                                throw new ArgumentException(); 
                            return (double)value; 
                        });
                //Otherwise, set the value to the cell's contents. This is the outcome whether the contents are a double or a string.
                else
                    c.Value = c.Contents;
            }
        }

        /// <summary>
        /// An implementation of the abstract method in AbstractSpreadsheet.
        /// <seealso cref="AbstractSpreadsheet.GetDirectDependents"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            name = CorrectInput(name);
            return dependencies.GetDependents(name);
        }

        /// <summary>
        /// If obj is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the normalized version of name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected string CorrectInput(string name, object obj)
        {
            Exception e = new InvalidNameException();
            if (obj == null)
                throw new ArgumentNullException();
            if (name == null)
                throw e;
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
                throw e;
            if (!IsValid(name))
                throw e;

            name = Normalize(name);

            return name;
        }

        /// <summary>
        /// Overloaded function for above, allows checking of just name rather than both objects. This is used when a class that can't be negative
        /// initially is given as a parameter (such as double).
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string CorrectInput(string name)
        {
            return CorrectInput(name, "Not Null");
        }

        /// <summary>
        /// Returns the formula with all the variables replaced with their normalized counterpart.
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        protected Formula NormalizeFormula(Formula f)
        {
            IEnumerable<string> e = f.GetVariables();
            foreach (string s in e)
            {
                f = new Formula(Regex.Replace(f.ToString(), s, Normalize(s)));
            }

            return f;
        }

        /// <summary>
        /// Returns the cell associated to name. If there is no cell associated with name, returns null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected Cell GetCell(string name)
        {
            name = CorrectInput(name);
            //Checks to see if there's a cell associated with name. If not, creates an empty cell and associates to name.
            if (!cells.ContainsKey(name))
            {
                cells[name] = new Cell("", "");
            }
            
            //Gets the cell associated with name.
            return cells[name];
        }
        
        /// <summary>
        /// A cell object represents the state of a single cell in a spreadsheet, as explained in the AbstractSpreadsheet class.
        /// If Contents or Value are set to anything other than a String, Double or FormulaError, throws an ArgumentException.
        /// </summary>
        protected class Cell
        {
            /// <summary>
            /// Sets up a new cell.
            /// </summary>
            /// <param name="c"></param>
            /// <param name="v"></param>
            public Cell(object c, object v)
            {
                Contents = c;
                Value = v;
            }

            //Properties
            /// <summary>
            /// The content property of a cell.
            /// </summary>
            public object Contents { get; set; } 

            /// <summary>
            /// The value of a cell.
            /// </summary>
            public object Value { get; set; }
        }
    }
}
