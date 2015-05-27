using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mySQLtest
{
    class Program
    {

        static void Nasluch(MySqlDataReader rdr)
        {
            

            while(true)
            {
                while (rdr.Read())
                {
                    Console.WriteLine(rdr.GetString(0));
                }
            }
            
        }

        static void Main(string[] args)
        {
            //string ConnectionString = "server=95.160.59.254; database=c7guardinfo; uid=c7guardian; pwd=angelangel9090;";
            string ConnectionString = "server=31.170.164.36; database=u880241475_guard; uid=u880241475_guan; pwd=6cr08SdqeP; SSLMode=Preferred";

            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlDataReader rdr = null;

            
            

            try
            {
                conn = new MySqlConnection(ConnectionString);
                cmd = new MySqlCommand();
                conn.Open();
                Console.WriteLine("Wersja mySQL: {0}", conn.ServerVersion);
                
                cmd.Connection = conn;
                cmd.CommandText = "SELECT VERSION()";
                cmd.Prepare();
                rdr = cmd.ExecuteReader();
                Task odbior = new Task(delegate { Nasluch(rdr); });
                ////cmd.CommandText = "CREATE TABLE IF NOT EXISTS Procesy (Id INT PRIMARY KEY, ProcId INT, ProcName VARCHAR (25)) ENGINE=INNODB;";
                //cmd.Prepare();
                //cmd.ExecuteNonQuery();
                odbior.Start();
                while (true)
                {
                    cmd.CommandText = Console.ReadLine();
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                }

            }

            catch (MySqlException ex)
            {
                Console.WriteLine("Błąd: {0}", ex.ToString());
            }
            finally
            {
                if (conn != null)
                    conn.Close();
                Console.ReadKey();
            }

            
        }



    }
}
