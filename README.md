# Crawler
Crawler is a C# Class to crawl directories to get files from them. Useful, and still in Beta


**Turning Debug Log File Off**
```c-like
  Crawl.__DEBUG__ = false;
```


**Clearing the Debug Log File**
```c-like
  Crawl.ClearLog();
```


**Instantiating The Crawl**
```c-like
  Crawl myCrawl = new Crawl(string directory);
```

**Storing the Files from Crawl**
```c-like
  List<FileDef> myList = myCrawl.Files;
```
