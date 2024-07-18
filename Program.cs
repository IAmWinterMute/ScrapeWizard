using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;


//Create a site
Site books = new Site("https://books.toscrape.com/");
books.startCrawl();

//Run the application for 5 seconds
int quarterSeconds = 0;
while (quarterSeconds <= (6*4))
{
    // Perform other tasks or just sleep to reduce CPU usage
    Task.Delay(250).Wait();
    quarterSeconds++;
    Console.WriteLine("Concurrent calls: " + books.concurrentCalls);
}

//Print result
//foreach (Page page in books.pages) { Console.WriteLine(page.pageURL); }
Console.WriteLine("Found " + books.pages.Count + " pages.");
