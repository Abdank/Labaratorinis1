using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class _1lab : System.Web.UI.Page
{
    public const int xMax = 100;  //maksimalus x ašies dydis
    public const int yMax = 100;  //maksimalys y ašies dydis
    const string data = "App_Data/U2.txt";      //duomenų failas
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// Spausdina salos rezultatus į Table1
    /// </summary>
    /// <param name="count"> salų skaičius plote</param>
    /// <param name="max"> didžiausios salos plote dydis</param>
    private void Table(string count, string max)
    {

        TableCell Max = new TableCell();
        Max.Text = max;

        TableCell Count = new TableCell();
        Count.Text = count;

        TableRow row = new TableRow();
        row.Cells.Add(Count);
        row.Cells.Add(Max);

        Table1.Rows.Add(row);
    }
    /// <summary>
    /// Spausdina rezultatus į Rezultatai.txt
    /// </summary>
    /// <param name="results"> rezultatai</param>
    /// <param name="Count"> Plotų kiekis</param>
    private void ToFile(int[,]results, int Count)
    {
        string path = Server.MapPath("App_Data/Rezultatai.txt");
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("{0,-15}{1,-15}", "Salų skaičius", "Didž. sala");
            writer.WriteLine("------------------------------");
            for (int i = 0; i < Count; i++)
            {
                writer.WriteLine("{0,-15}{1,-15}", results[i,0], results[i,1]);
            }
        }
    }
    /// <summary>
    /// Spausdina duomenis
    /// </summary>
    /// <param name="results"> rezultatai</param>
    /// <param name="Count"> Plotų kiekis </param>
    private void Print(int[,] results, int Count)
    {
            TableRow row = new TableRow();
            TableCell pavadinimas = new TableCell();
            pavadinimas.Text = "<b>Salų kiekis</b>";
            row.Cells.Add(pavadinimas);
            TableCell kaina = new TableCell();
            kaina.Text = "<b>Didžiausia sala</b>";
            row.Cells.Add(kaina);
            Table1.Rows.Add(row);

        ToFile(results, Count);

        for (int i = 0; i < Count; i++)
        {
            Table(results[i, 0].ToString(), results[i, 1].ToString());
        }
    }
    /// <summary>
    /// Skai
    /// </summary>
    /// <param name="count"> Žemėlapių kiekis</param>
    /// <returns> gražina žemėlapius</returns>
    public int[,] Islands(out int count)
    {
        MapsContainer Maps = Reading();
        PrintDataFile(Maps);
        int[,] results = new int[Maps.Count, 2];
        Do(Maps, ref results);


        count = Maps.Count;
        PrintDataWeb();
        return results;
    }
    /// <summary>
    /// Skaito duomenų faila
    /// </summary>
    /// <returns> gražina žemėlapių konteinerį</returns>
    private static MapsContainer Reading()
    {
        using (StreamReader reader = new StreamReader(@data))
        {
            int count = int.Parse(reader.ReadLine());
            MapsContainer a = new MapsContainer(count);
            for (int i = 0; i < count; i++)
            {

                string[] dims = reader.ReadLine().Split();
                int[] dim = new int[3];
                dim[1] = int.Parse(dims[0]) + 2; // dim[1] - Y ašis ilgis
                dim[2] = int.Parse(dims[1]) + 2; // dim[2] - X ašis ilgis

                int[,] Empty = new int[xMax, yMax];
                int[,] area = new int[xMax, yMax];

                area = Empty;


                for (int j = 1; j < dim[1] - 1; j++)
                {
                    string line = reader.ReadLine();
                    for (int k = 1; k < dim[2] - 1; k++)
                    {
                        area[k, j] = int.Parse(line[k - 1].ToString());
                    }
                }
                Maps map = new Maps(area, dim[2], dim[1]);
                a.AddMap(map);
            }
            return a;
        }
    }
    /// <summary>
    /// Atlieka skaičiavimo veiksmus
    /// </summary>
    /// <param name="Map"> Žemėlapių konteineris</param>
    /// <param name="results"> rezultatų dvimatys masyvas [salų skaičius,2]</param>
    private static void Do(MapsContainer Map, ref int[,] results)
    {
        for (int i = 0; i < Map.Count; i++)
        {
            int[,] Empty = new int[xMax, yMax];
            int[,] area = new int[xMax, yMax];

            area = Empty;
            area = Map.GetMap(i).Map;

            int IslandCount = 0;
            int MaxTwos = 0;

            for (int j = 1; j < Map.GetMap(i).ySize - 1; j++)
            {
                for (int k = 1; k < Map.GetMap(i).xSize - 1; k++)
                {
                    if (area[k, j] == 1)
                    {
                        area[k, j] = 2;
                        ConvertNearbySquares(ref area, k, j);
                        IslandCount++;
                        int Twos = CountTwos(area, Map.GetMap(i).xSize, Map.GetMap(i).ySize);
                        if (MaxTwos < Twos)
                        {
                            MaxTwos = Twos;
                        }
                        ClearTwos(ref area, Map.GetMap(i).xSize, Map.GetMap(i).ySize);
                    }
                }
            }
            results[i, 0] = IslandCount;
            results[i, 1] = MaxTwos;
        }
    }
    /// <summary>
    /// Metodas su rekursija. Suranda visus langelį liečiančius langelius, ir tuos langelius liečiančius langelius ir t.t.
    /// </summary>
    /// <param name="area"> perduodamas žemėlapis, kuriame 0 - vanduo, 1 - dar netikrinta žemė, 2 - jau patikrinta žemė</param>
    /// <param name="xpos"> langelio x koordinate </param>
    /// <param name="ypos"> langelio y koordinate </param>
    private static void ConvertNearbySquares(ref int[,] area, int xpos, int ypos)
    {
        area[xpos, ypos] = 2;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (area[xpos + x, ypos + y] == 1)
                {
                    ConvertNearbySquares(ref area, xpos + x, ypos + y);

                }
            }
        }
    }
    /// <summary>
    /// Kad būtų galima skaičiuot kitos salos dydį, ši sala panaikinama iš žemėlapio masyvo (konteineris išlieka nepakeistas)
    /// </summary>
    /// <param name="Map"> Žemėlapio masyvas</param>
    /// <param name="x"> Žemėlapio didžiausia x koordinate </param>
    /// <param name="y"> Žemėlapio didžiausia y koordinate </param>
    private static void ClearTwos(ref int[,] Map, int x, int y)
    {
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                if (Map[j, i] == 2)
                    Map[j, i] = 0;
            }
        }
    }
    /// <summary>
    /// Suskaičiuoja visus dvejetus dvimatyje masyve, t.y. suskaičiuoja salos dydį
    /// </summary>
    /// <param name="Map"> žemėlapio dvimatis masyvas </param>
    /// <param name="x"> žemėlapio didžiausia x koordinatė </param>
    /// <param name="y"> žemėlapio didžiausia y koordinatė </param>
    /// <returns></returns>
    private static int CountTwos(int[,] Map, int x, int y)
    {
        int count = 0;
        {
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if (Map[j, i] == 2)
                        count++;
                }
            }
        }
        return count;
    }
    /// <summary>
    /// Paspaudus Button1 atlieka skaičiavimus ir spausdinimą
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        int Count;
        string path = Server.MapPath(data);
        int[,] results = Islands(out Count);
        Print(results, Count);
    }
    /// <summary>
    /// spausdina duomenis ekrane
    /// </summary>
    private void PrintDataWeb()
    {
        string[] lines = File.ReadAllLines(Server.MapPath("App_Data/U2.txt"));
        foreach (string line in lines)
        {
            TableRow row = new TableRow();
            TableCell Line = new TableCell();
            Line.Text = line;
            row.Cells.Add(Line);

            Table2.Rows.Add(row);

        }
    }
    /// <summary>
    /// spausdina duomenis faile
    /// </summary>
    /// <param name="Map"></param>
    private void PrintDataFile(MapsContainer Map)
    {
        string path = Server.MapPath("App_Data/Duomenys.txt");
        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.WriteLine("Salų kiekis: {0,10}", Map.Count);
            for (int i = 0; i < Map.Count; i++)
            {
                writer.WriteLine("n ir m reikšmės: {0,5}{1,5}", Map.GetMap(i).ySize, Map.GetMap(i).xSize);
                for (int j = 1; j < Map.GetMap(i).ySize-1; j++)
                {
                    writer.Write("Stačiakampio duomenys: "); 
                    for (int k = 1; k < Map.GetMap(i).xSize-1; k++)
                    {
                        writer.Write(Map.GetMap(i).Map[k, j]);
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}