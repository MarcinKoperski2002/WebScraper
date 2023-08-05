using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper;

public class CanalTV
{
    private int LP;
    private string NameCanal;
    private string Href;

    public CanalTV (int lp, string nameCanal, string href)
    {
        LP = lp;
        NameCanal = nameCanal;
        Href = href;
    }

    public int lp
    {
        get { return LP; }
        set { LP = value; }
    }

    public string nameCanal
    {
        get { return NameCanal; }
        set { NameCanal = value; }
    }

    public string href
    {
        get { return Href; }
        set { Href = value; }
    }
}