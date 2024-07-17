

//Creating a httpClient to make http calls
HttpClient httpClient = new HttpClient(); 
var json = await httpClient.GetStringAsync("https://books.toscrape.com/");
Console.Write(json);
