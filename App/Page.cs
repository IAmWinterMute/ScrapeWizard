using System.Text.RegularExpressions;

public class Page
{
    public string baseURL { get; }
    public string pageURL { get; }
    public List<string>? linksInPage { get; set;}
    public string? content { get; set;}
    //public string path[10] { get; }

    public Page(string baseURL, string pageURL)
    {
        this.baseURL = baseURL;
        this.pageURL = pageURL;
    }

   
}