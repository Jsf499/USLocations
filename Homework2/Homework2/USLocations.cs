using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

/**
 * Author: Jacob Freedman
 * 
 * Report:
 * I have successfully completed all requirements for this project
 */
namespace USLocations
{
    using zip = Tuple<int, string, string, double, double, string, int, Tuple<int, long>>;
    
    public class USLocations
    {
        
   
        // This constructor will initiate the loading of the TSV file.
        // The constructor must return very quickly, perhaps before all 
        // of the zip code information has been completely loaded. Tasks
        // will be needed to do this.

            //       zipcode   ,City,   State,  Lat,   Lon,  LocText,TaxReturns,  Pop,TotWages
        public List<Tuple<int, string, string, double, double, string, int, Tuple<int, long>>> zipInfo =
               new List<Tuple<int, string, string, double, double, string, int, Tuple<int, long>>>();
        public USLocations()
        {
          

        }
        public void Helper(string fileName)
        {
            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Homework2.MainPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("Homework2.zipcodes.tsv");
                System.IO.StreamReader file = new System.IO.StreamReader(stream);
                string line;
                String[] spearator = { "\t" };
                int rowCount = 0;
                line = file.ReadLine();
                while (!file.EndOfStream)
                {
                   
                    if (rowCount == 0) { rowCount++; }
                    else
                    {
                        line = file.ReadLine();
               
                        String[] strlist = line.Split(spearator, StringSplitOptions.None);
                        for (int i = 0; i < strlist.Length; i++)
                        {
                            if (strlist[1].Equals(""))
                            {
                                strlist[1] = "0";
                            }
                            if (strlist[6].Equals(""))
                            {
                                strlist[6] = "0";
                            }
                            if (strlist[7].Equals(""))
                            {
                                strlist[7] = "0";
                            }
                            if (strlist[16].Equals(""))
                            {
                                strlist[16] = "0";
                            }
                            if (strlist[17].Equals(""))
                            {
                                strlist[17] = "0";
                            }
                            if (strlist[18].Equals(""))
                            {
                                strlist[18] = "0";
                            }
                        }
                        zipInfo.Add(new Tuple<int, string, string, double, double, string, int, Tuple<int, long>>
                            (int.Parse(strlist[1]), strlist[3], strlist[4], double.Parse(strlist[6]), double.Parse(strlist[7]), strlist[13], int.Parse(strlist[16]), new Tuple<int, long>(int.Parse(strlist[17]), long.Parse(strlist[18])))
                            );
                    }

                }

                file.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        // This method will return the number of miles between two zip codes.
        // Since zipcodes can appear multiple times, the location is based
        // on the first record that has a matching zipcode.
        // This method will need to wait, if it is called before the zipcodes
        // have been completely loaded.
        // 
        // Look up "Haversine" formula to do this one.
        public  double Distance(int zip1, int zip2)
        {
            double rMile = 3960;
            double lat1 = 0, lat2 = 0, long1 = 0, long2 = 0;
            bool comp1 = false, comp2 = false;
           foreach(zip x in zipInfo) {
                if(x.Item1 == zip1)
                {
                    if (!comp1)
                    {
                        lat1 = x.Item4;
                        long1 = x.Item5;
                        comp1 = true;
                    }
                }
                if(x.Item1 == zip2)
                {
                    if (!comp2)
                    {
                        lat2 = x.Item4;
                        long2 = x.Item5;
                        comp2 = true;
                    }
                }
                if(comp1 && comp2)
                {
                    break;
                }
            }
            double flat = lat2 - lat1;
            double flong = long2 - long1;
            double dLat = (flat * Math.PI) / 180.0;
            double dLon = (flong * Math.PI) / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos((lat1*Math.PI)/180.0) * Math.Cos((lat2*Math.PI)/180.0) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = rMile * c;

            return d;
        }
        // This method will return all the common names for a particular
        // zip code. The sequence of items in the list will match the order
        // seen in the data file.
        // This method will need to wait, if it is called before the zipcodes
        // have been completely loaded.
        public List<string> Lookup(int zip)
        {
            List<string> names = new List<string>();
           
            foreach (zip x in zipInfo) 
            {
                if(x.Item1 == zip)
                {    
                    names.Add(x.Item2 + ", " + x.Item3);
                }
            }
          
            return names;
        }
        public  List<string> TaxNearInput(int amount)
        {
      
            List<string> list = new List<string>();
            List<int> Inlist = new List<int>();
           
            foreach (zip x in zipInfo)
            {
                double tax;
                if (x.Item7 == 0)
                {
                     tax = x.Rest.Item2 / 1;
                }
                else
                {
                     tax = x.Rest.Item2 / x.Item7;
                }
               
                if(tax > (amount-100) && tax < (amount+100) && !Inlist.Contains((int)x.Item1))
                {
                    list.Add(x.Item1.ToString() + " " + x.Item2.ToUpper() + " " + x.Item3.ToUpper() + " " + tax.ToString());
                    Inlist.Add((int)x.Item1);
                }
               
            }
            list.Sort();
            return list;

        }

        public List<string> NearLocation(string city, string state)
        {

            List<string> list = new List<string>();
            List<int> Inlist = new List<int>();

            foreach (zip x in zipInfo)
            {
               
                double tax;
                if (x.Item7 == 0)
                {
                    tax = x.Rest.Item2 / 1;
                }
                else
                {
                    tax = x.Rest.Item2 / x.Item7;
                   
                }
                //tax > (amount - 100) && tax < (amount + 100) && !Inlist.Contains((int)x.Item1) &&
                if (city.ToUpper() == x.Item2.ToUpper() && state.ToUpper() == x.Item3.ToUpper())
                {
                   
                    list.Add(x.Item1.ToString() + " " + x.Item2.ToUpper() + " " + x.Item3.ToUpper() + " " + tax.ToString());
                    Inlist.Add((int)x.Item1);
                }
            }
            list.Sort();
            return list;

        }

        // The methods below are for 570J students only
        /**
     * Returns a map that is keyed to state name. The values in the map are the
         * set of city names that reside in that particular state. The map looks
         * like: "AL" --> { "MONTGOMERY", "MOBILE", ... } "AK" --> { "ANCHORAGE",
     * "BARROW", ...} ...
     
        public IDictionary<string, ISet<string>> GetCityNames()
        {   // 
            /**
             * Returns the city names that appear in both of the given states.
                 * "OH" and "MI" would yield {OXFORD, FRANKLIN, ... }
             
            public ISet<string> GetCommonCityNames(string state1, string state2)
            {
                /**
                 * Returns all zipcodes that are within a specified distance from a
                 * particular zipcode.
                 
                public ISet<int> GetZipCodesCloseTo(int zipCode, double miles)
                {
                    /**
                     * Ranked list of states, where the ranking is ascending order of number of
                     * zipcodes. In the event of a tie, use the state's alphabetical ordering.
                     
                    public List<string> MostZipCodes()
                    {
                    }
                  */

    }
}
