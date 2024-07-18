using System.Xml.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

//Finding all links possible with the linkfinder
LinkFinder linkfinder = new LinkFinder();
List<string> AllLinks = await linkfinder.traverseLinks("index.html", new List<string>());
