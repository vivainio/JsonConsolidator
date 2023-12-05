using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;

var allDocs = new List<string>();
var alreadySeen = new HashSet<string>();

void FeedLine(string id, string line)
{
    var wasNew = alreadySeen.Add(id);
    if (!wasNew)
        return;
    allDocs.Add(line);
}

IEnumerable<string> ReadFile(string gzippedFilePath)
{
    using (FileStream fileStream = new FileStream(gzippedFilePath, FileMode.Open, FileAccess.Read))
    using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
    using (StreamReader reader = new StreamReader(gzipStream))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }    
}


void FeedFile(string fname) {
    var got = ReadFile(fname);
    foreach (var line in got)
    {
        var idcontainer = JsonSerializer.Deserialize<OnlyId>(line);
        FeedLine(idcontainer.id, line); 
    }
}

for (var i=0; i < 100; i++)
{
    var sw = Stopwatch.StartNew();
    // repeating the same file, only the first run adds ids. The rest are skipped
    FeedFile("/r/JsonConsolidator/docs/1.json.gz");
    Console.WriteLine(sw.ElapsedMilliseconds + "ms");
    
}

Console.WriteLine($"Lines: {allDocs.Count}");
Console.ReadLine();

class OnlyId
{
    public string id { get; set; }
}
