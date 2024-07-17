using System.Xml.Linq;

//Creating a httpClient to make http calls
HttpClient httpClient = new HttpClient(); 
string basePage = await httpClient.GetStringAsync("https://books.toscrape.com/");

//Trying to parse the page with XDoc so I can use Linq queries.
XDocument doc = XDocument.Parse(basePage);
Console.Write(doc);
