using System.Runtime.InteropServices.ComTypes;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
if (!Directory.Exists(salesTotalDir))
    Directory.CreateDirectory(salesTotalDir);

var salesFiles = FindFiles(storesDirectory);
var totalSales = CalculateSalesTotal(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"${totalSales.ToString("#.00")}" + Environment.NewLine);

IEnumerable<string> FindFiles(string foldername)
{
    List<string> salesFiles = new List<string>();
    var foundFiles = Directory.EnumerateFiles(foldername, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
            salesFiles.Add(file);
    }
    return salesFiles;

}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    var total = 0.0;
    foreach (var file in salesFiles)
    {
        var salesJson = File.ReadAllText(file);
        SalesData? data = JsonConvert.DeserializeObject<SalesData>(salesJson);
        total += data?.Total ?? 0;
    }

    return total;
}

record SalesData(double Total);