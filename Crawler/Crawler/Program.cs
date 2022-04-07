using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Crawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            bool work = true;
            while (work) {
                Console.WriteLine("If you want to stop enter 0");
                Console.Write("Enter URI:  ");
                String input = Console.ReadLine();

                if (String.Equals(input, "0"))
                {
                    work = false;
                    continue;
                }

                string webUrl = input;
                if (!Uri.IsWellFormedUriString(webUrl, UriKind.Absolute))
                {
                    Console.WriteLine("Wrong Uri format!");
                    continue;
                }
                    

                HttpClient client = new HttpClient();

                try
                {
                    HttpResponseMessage response = null;
                    try
                    {
                        response = await client.GetAsync(webUrl);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error while downloading the page");
                        Environment.Exit(-1);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        string info = await response.Content.ReadAsStringAsync();
                        Regex re = new Regex(@"\w+[@]\w+([.]\w+)+");
                        MatchCollection col = re.Matches(info);
                        HashSet<string> set = new HashSet<string>();
                        foreach (Match match in col)
                        {
                            set.Add(match.Value);
                        }
                        if (set.Count != 0)
                        {
                            foreach (string s in set)
                            {
                                Console.WriteLine(s);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Email addresses not found");
                        }

                    }
                }
                finally { client.Dispose(); } 
            }
        }
    }
}