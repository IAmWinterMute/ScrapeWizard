using System.Text.RegularExpressions;

public class Site
{
    //All the pages for this site
    public List<Page> pages;

    //The base url of the site
    public string baseURL { get; }

    public int concurrentCalls = 0;
    private static readonly object lockObject = new object();

    // Create a class constructor for the Car class
    public Site(string baseURL)
    {
        pages = new List<Page>();
        this.baseURL = baseURL;
    }

    public void startCrawl()
    {
        pages = new List<Page>();
        crawlSite("index.html");
    }

    //Recursive function for crawling all links
    //Logs the found page then follows all links in that page that has not already been visited async.
    private void crawlSite(string pageLink)
    {
        if (pages.Find(s => s.pageURL == pageLink) == null)
        {
            Page newPage = new Page(baseURL, pageLink);
            pages.Add(newPage);
            HttpClient httpClient = new HttpClient();
            lock (lockObject)
            {
                concurrentCalls++;
            }
            //Console.WriteLine("TaskID making http call: " + Task.CurrentId);
            httpClient.GetStringAsync(baseURL + newPage.pageURL).ContinueWith(completeTask =>
            {
                lock (lockObject)
                {
                    concurrentCalls--;
                }
                
                //Preparing page
                newPage.content = completeTask.Result;
                newPage.linksInPage = findUniqueForwardHrefs(completeTask.Result);

                //Calling all links in the page what is not already crawled
                foreach (string link in newPage.linksInPage)
                {
                    if (pages.Find(s => s.pageURL == link) == null)
                    {
                        //foreach (Page url in pages) { Console.WriteLine(url.pageURL); }
                        crawlSite(link);
                    }
                    else
                    {
                        //Console.WriteLine(link + " is already in the list. Not following.");
                        //Console.WriteLine("");
                    }
                }

            },TaskContinuationOptions.RunContinuationsAsynchronously);
        }
        else
        {
            //Console.WriteLine("Was already in list even though is shouldn't. Async clash");
            //Console.WriteLine("");
        }
    }

    //Function for finding links
    private List<string> findUniqueForwardHrefs(string html)
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
