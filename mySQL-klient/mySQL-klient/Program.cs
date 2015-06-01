using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace mySQL_klient
{
    class Program
    {        
        static MySqlConnection conn = null;
        static MySqlCommand cmd = null;      
        static MySqlDataReader rdr = null;

        static MySqlDataReader WyslijOdbierz (string komenda)
        {
            try
            {
                conn.Open(); //         <- otwarcie połączenia
                cmd.Connection = conn;
                cmd.CommandText = komenda;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Prepare();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("! Problem z połączeniem !\n" + ex.Message.ToString());
                rdr = null;
            }                              
            
            try
            {
               
                cmd.ExecuteNonQuery();
                if (!(komenda.Contains("create") || komenda.Contains("insert") || komenda.Contains("drop")))
                    rdr = cmd.ExecuteReader();               
            }
            catch (MySqlException ex)
            {
                if ((uint)ex.ErrorCode == 0x80004005)
                    Console.WriteLine("! Błąd ! - Niepoprawne polecenie lub składnia zapytania");
                else
                    Console.WriteLine(ex);
                
                rdr=null;
                conn.Close();
                
            }
            return rdr;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Klient mySQL do bazy na netck.pl - komendy");
			Console.Write("Logowanie:\nPodaj IP serwera: ");
			string bazaIp = Console.ReadLine();
            Console.Write("Nazwa bazy danych: ");
            string bazaNazwa = Console.ReadLine();
            Console.Write("UID do bazy danych: ");
            string bazaUID = Console.ReadLine();
            Console.Write("Hasło bazy danych: ");
            string bazaPASS = Console.ReadLine();
            string ConnectionString = String.Format("server={0}; database={1}; uid={2}; pwd={3}; SSLMode=Preferred", bazaIp, bazaNazwa, bazaUID, bazaPASS);            

            conn = new MySqlConnection(ConnectionString);
            cmd = new MySqlCommand();
            
            while (true)
            {
                Console.WriteLine();
                Console.Write("MySQL> ");
                string komenda = Console.ReadLine();
                if (komenda.Equals("exit"))
                    break;
                WyslijOdbierz(komenda);
                if (rdr != null)
                    if (!rdr.IsClosed)
                {
                    while (rdr.Read())
                    {
                        for (int pole = 0; pole < rdr.FieldCount; pole++)
                        {
                            if (!rdr.IsDBNull(pole))
                                Console.Write(rdr.GetString(pole) + " | ");
                            else
                                Console.Write("-null-" + " | ");
                        }
                        Console.WriteLine();
                    }                   
                    
                }
                if (rdr != null)
                    rdr.Close();
                conn.Close();
             }
            if (rdr != null)
                rdr.Close();
            if (conn != null)
                conn.Close();

           
        }
    }
}
