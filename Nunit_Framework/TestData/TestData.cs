using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nunit_Framework
{
    public class TestData
    {
        public const string URL = "http://capability.demojoomla.com:81/administrator/index.php";
        public static string USERNAME = "capability01";
        public static string PASSWORD = "12345678";
        public static string INVUSERNAME = "abc";
        public static string INVPASSWORD = "abc";
        public static string ERRORINVALIDACC = "Username and password do not match or you do not have an account yet.";
        public static string ARTICLETITLEDEFAULT = "Article_Title";
        public static string ARTICLECATEGORYDEFAULT = "Uncategorised";
        public static string ARTICLETEXTDEFAULT = "Article_Text";
    }
}