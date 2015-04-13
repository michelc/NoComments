using System;
using System.Text;

namespace NoComments
{
    public class Program
    {
        static void Main(string[] args)
        {
            var sql = @"/* Demo */
                        SELECT *
                             , 'a -- false comment' AS Test1
                             , 'another /* false comment */' AS Test2
                        FROM   OneTable -- Or Customers?
                        WHERE  Caption LIKE 'C%' /* Customers! */
                        -- AND    ID <> 3";

            //sql = "--this is a test\r\nselect stuff where substaff like '--this comment should stay' --this should be removed\r\n";

            Console.WriteLine("Before:");
            Console.WriteLine("-------");
            Console.WriteLine(sql.Replace("                        ", ""));

            Console.WriteLine();

            sql = NoSqlComments(sql);

            Console.WriteLine("After:");
            Console.WriteLine("------");
            Console.WriteLine(sql.Replace("                        ", ""));

            Console.ReadLine();
        }

        public static string NoSqlComments(string sql)
        {
            var result = new StringBuilder();

            bool multi_starting = false;
            bool multi_ending = false;
            bool in_multi = false;
            bool mono_starting = false;
            bool in_mono = false;
            bool in_string = false;

            foreach (var c in sql)
            {
                // Check if potential patterns are completed
                if (multi_starting)
                {
                    multi_starting = false;
                    if (c == '*')
                    {
                        in_multi = true;
                        result.Length--;
                        result = new StringBuilder(result.ToString().TrimEnd());
                    }
                }
                if (multi_ending)
                {
                    multi_ending = false;
                    if (c == '/')
                    {
                        in_multi = false;
                        continue;
                    }
                }
                if (mono_starting)
                {
                    mono_starting = false;
                    if (c == '-')
                    {
                        in_mono = true;
                        result.Length--;
                        result = new StringBuilder(result.ToString().TrimEnd());
                    }
                }

                // Check if current char starts a new pattern
                switch (c)
                {
                    case '/':
                        // Maybe a new multi-line comment
                        multi_starting = !in_string && !in_multi && !in_mono;
                        break;
                    case '-':
                        // Maybe a new mono-line comment
                        mono_starting = !in_string && !in_multi && !in_mono;
                        break;
                    case '\'':
                        // Start or end a literal string
                        // (except when quote happens inside a comment)
                        if (!in_multi && !in_mono) in_string = !in_string;
                        break;
                    case '\n':
                    case '\r':
                        // Newline => ends potential mono-line comment
                        in_mono = false;
                        break;
                    case '*':
                        // Maybe the end of a multi-line comment
                        if (in_multi) multi_ending = true;
                        break;
                    default:
                        break;
                }

                // Add char when it's outside comment
                if (!in_multi && !in_mono) result.Append(c);
            }

            return result.ToString().Trim();
        }
    }
}
