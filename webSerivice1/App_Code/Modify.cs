using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for Modify
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class Modify : System.Web.Services.WebService {

    public Modify () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    
    [WebMethod]
    public string Modyfikacja(string tekst)
    {
        tekst = "NIE " + tekst;
        return tekst;
    }

    [WebMethod]
    public int Dodawanie(int a, int b)
    {
        int wynik = a + b;
        return wynik;
    }

}
