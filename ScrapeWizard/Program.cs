using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

//Creating a httpClient to make http calls
HttpClient httpClient = new HttpClient();
string basePage = await httpClient.GetStringAsync("https://books.toscrape.com/");

//Getting all the links from the page
List<string> cleanedBasePage = findHrefs(basePage);

//Loggin to test
Console.WriteLine(string.Join(",", cleanedBasePage.ToArray()));

//Function for finding links
static List<string> findHrefs(string html)
{
    string hrefPattern = @"href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>[^>\s]+))";
    List<string> foundLinks = new List<string>();
    try
    {
        Match regexMatch = Regex.Match(html, hrefPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
        while (regexMatch.Success)
        {
            foundLinks.Add(regexMatch.Groups[1].Value);
            regexMatch = regexMatch.NextMatch();
        }
    }
    catch (RegexMatchTimeoutException)
    {
        Console.WriteLine("The matching operation timed out.");
    }
    return foundLinks;
}