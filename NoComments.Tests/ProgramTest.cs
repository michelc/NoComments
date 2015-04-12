using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoComments.Tests
{
    [TestClass]
    public class ProgramTest
    {
        [TestMethod]
        public void Remove_Monoline_Comment_And_Newline_At_Start_Of_Line()
        {
            // A monoline comment that starts at the beginning of the line
            var sql = "Ligne1\n--Ligne2\nLigne3";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne3";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Remove_Monoline_Comment_Inside_Line()
        {
            // A monoline comment that starts in the middle of the line
            var sql = "Ligne1\nLigne--2\nLigne3";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne\nLigne3";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Keep_Monoline_Comment_Inside_Literal_String()
        {
            // A monoline comment that is part of a literal string
            var sql = "Ligne1\nLigne2'bla --bla bla'\nLigne3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Keep_Monoline_Comment_Inside_Literal_String_With_Escaped_Quote()
        {
            // A monoline comment that is part of a literal string containing escaped quote
            var sql = "Ligne1\nLigne2'bl''a --bla bla'\nLigne3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Keep_Monoline_Comment_Inside_Multiline_Literal_String()
        {
            // A monoline comment that is part of a literal string spanning on several lines
            var sql = "Ligne1\nLigne2'bla bla --bla\nLigne3'";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Keep_Monoline_Comment_With_Bad_Starting()
        {
            // A monoline comment that starts with "- -" instead of "--"
            var sql = "Ligne1\n- -Ligne2\nLigne3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Remove_Multiline_Comment_And_Newline_At_Start_Of_Line()
        {
            // A multiline comment that starts at the beginning of the line
            var sql = "Ligne1\n/*Ligne2*/\nLigne3";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne3";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Remove_Multiline_Comment_Inside_Line()
        {
            // A multiline comment that starts in the middle of the line
            var sql = "Ligne1\nLigne/*2*/\nLigne3";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne\nLigne3";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Remove_Multiline_Comment_With_No_Ending_To_The_End()
        {
            // A multiline comment with start but no end
            var sql = "Ligne1\n/*Ligne2\nLigne3";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Remove_Multiline_Comment_And_Newline_On_Several_Lines()
        {
            // A multiline comment that continues on several lines
            var sql = "Ligne1\n/*Ligne2\nLigne3*/\nLigne4";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne4";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Remove_Multiline_Comment_On_Several_Lines()
        {
            // A multiline comment that continues on several lines
            var sql = "Ligne1\nLigne/*2\nLigne3*/\nLigne4";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne\nLigne4";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Keep_Multiline_Comment_Inside_Literal_String()
        {
            // A multiline comment that is part of a literal string
            var sql = "Ligne1\nLigne2'bla /*bla*/ bla'\nLigne3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Keep_Multiline_Comment_Inside_Literal_String_With_Escaped_Quote()
        {
            // A multiline comment that is part of a literal string containing escaped quote
            var sql = "Ligne1\nLigne2'bl''a /*bla*/ bla'\nLigne3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Keep_Multiline_Comment_Inside_Multiline_Literal_String()
        {
            // A multiline comment that is part of a literal string spanning on several lines
            var sql = "Ligne1\nLigne2'bla bla /*bla\nLigne*/'3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Keep_Multiline_Comment_With_Bad_Starting()
        {
            // A multiline comment that starts with "/ *" instead of "/*"
            var sql = "Ligne1\n/ *Ligne2*/\nLigne3";

            var actual = Program.NoSqlComments(sql);

            Assert.AreEqual(sql, actual);
        }

        [TestMethod]
        public void Remove_Multiline_Comment_With_Bad_Ending_To_The_End()
        {
            // A multiline comment that ends with "* /" instead of "*/"
            var sql = "Ligne1\n/*Ligne2* /\nLigne3";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Do_Not_Start_String_Inside_Monoline_Comment()
        {
            // A monoline comment with a quote inside
            var sql = "Ligne1\n--Ligne'2\nLigne='3--'";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne='3--'";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Do_Not_Start_String_Inside_Multiline_Comment()
        {
            // A multiline comment with a quote inside
            var sql = "Ligne1\n/*Ligne'2\nLigne3*/\nLigne='4--'";

            var actual = Program.NoSqlComments(sql);
            var expected = "Ligne1\nLigne='4--'";

            Assert.AreEqual(expected, actual);
        }
    }
}
