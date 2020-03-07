using System;
using System.Collections.Generic;
using System.Text;
using USLocations;
using System.Threading.Tasks;
namespace Zipcode
{
    class ZipcodeDriver
    {
        public static void Main(string[] args)
        {
            USLocations.USLocations location = new USLocations.USLocations();
            location.Helper("zipcodes.tsv");
            bool run = true;
            while (run)
            {
                String[] spearator = { " " };
                Console.Write("zipcodes>");
                string input = Console.ReadLine();
                String[] strlist = input.Split(spearator, StringSplitOptions.None);
             
              
                    int zip = int.Parse(strlist[1]);
                    List<string> loc = location.Lookup(zip);
                    foreach(string s in loc)
                    {
                        Console.WriteLine(s);
                    }
            
            }
        }
        //run distance from uslocations, print that and convert to kilometers and print too
    }
}
