using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ASM1MVC
{
    public class Database
    {
        public MySqlConnection connectDB;
        public Database()
        {
            //string myConnectionString = "server=127.0.0.1;uid=web1;pwd=123456;database=asm1;port=3306";
            string myConnectionString = "server=sql6.freesqldatabase.com;uid=sql6584435;pwd=uPRr3RF6GL;database=sql6584435;port=3306";
            connectDB = new MySqlConnection(myConnectionString);
        }
    }
}
