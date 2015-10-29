# Crawler
Crawler is a C# Class to crawl directories to get files from them. Useful, and still in Beta


**Turning Debug Log File Off**
```cs
  Crawl.__DEBUG__ = false;
```


**Clearing the Debug Log File**
```cs
  Crawl.ClearLog();
```


**Instantiating The Crawl**
```cs
  Crawl myCrawl = new Crawl(string directory);
```

**Storing the Files from Crawl**
```cs
  List<FileDef> myList = myCrawl.Files;
```
