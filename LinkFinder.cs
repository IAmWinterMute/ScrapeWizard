
using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class LinkFinder()
{

    //List<string> beenHere = new List<string>();
    int maxCounter = 0;


    public async Task<List<string>> traverseLinks(string pageURL, List<string> beenHere)
    {
        Console.WriteLine("At page: " + pageURL);
        maxCounter++;
        if (maxCounter < 100)
        {
            beenHere.Add(pageURL);
            HttpClient httpClient = new HttpClient();
            string page = await httpClient.GetStringAsync("https://books.toscrape.com/" + pageURL);
            List<string> links = findUniqueForwardHrefs(page);
            foreach (string link in links)
            {
                if (beenHere.Find(s => s == link) == null)
                {
                    Console.WriteLine("In beenHere at this time: ");
                    foreach (string url in beenHere) { Console.WriteLine(url); }
                    Console.WriteLine("Traversing next: " + link);
                    Console.WriteLine("");
                    //Thread.Sleep(500);
                    beenHere = await traverseLinks(link, beenHere);
                }
                else
                {
                    Console.WriteLine(link + " is already in the list. Not following.");
                    Console.WriteLine("");
                }
            }
            return beenHere;
        }
        else
        {
            Console.WriteLine("Maxcounter hit:" + pageURL);
            Console.WriteLine("");
        }
        return beenHere;
    }

    //Function for finding links
    static List<string> findUniqueForwardHrefs(string html)
    {
        string hrefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>[^>\s]+))";
        List<string> foundLinks = new List<string>();
        try
        {
            Match regexMatch = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
            while (regexMatch.Success)
            {
                if (foundLinks.Find(s => s == regexMatch.Groups[1].Value) == null)
                {
                    foundLinks.Add(regexMatch.Groups[1].Value);
                }
                regexMatch = regexMatch.NextMatch();
            }
        }
        catch (RegexMatchTimeoutException)
        {
            Console.WriteLine("The matching operation timed out.");
        }
        return foundLinks;
    }

}


