using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class Service : System.Web.Services.WebService
{
    public Service () 
    {

    }

    [WebMethod]
    public string Registration()
    {
        Code2040 cd = new Code2040();
        var result = cd.Registration();
        return result;
    }

    [WebMethod]
    public string Reverse()
    {
        Code2040 cd = new Code2040();
        var result = cd.Reverse();
        return result;
    }

    [WebMethod]
    public string NeedleInHaystake()
    {
        Code2040 cd = new Code2040();
        var result = cd.NeedleInHaystake();
        return result;
    }

    [WebMethod]
    public string PrefixArray()
    {
        Code2040 cd = new Code2040();
        var result = cd.PrefixArray();
        return result;
    }

    [WebMethod]
    public string TimeGame()
    {
        Code2040 cd = new Code2040();
        var result = cd.TimeGame();
        return result;
    }



}