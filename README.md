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

```cs
  Crawl myCrawl = new Crawl(string directory,bool deep);
```

```cs
  Crawl myCrawl = new Crawl(string directory, string extensions);
```

```cs
  Crawl myCrawl = new Crawl(string directory,bool deep, string extensions);
```

**Storing the Files from Crawl**
```cs
  List<FileDef> myList = myCrawl.Files;
```


**Upcoming Changes**

```cs
  List<DirDef> myDirs = myCrawl.Directories;
```

```cs
  List<TreeView> myTree = myCrawl.Tree;
```
