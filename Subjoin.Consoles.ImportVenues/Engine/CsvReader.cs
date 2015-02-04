using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Subjoin.Consoles.ImportVenues.Engine
{
    public class CsvReader
    {
        public static IEnumerable<T> ReadLines<T>(string path, Func<string, T> transformer)
        {
            return File.ReadAllLines(path).Select(transformer.Invoke);
        }
    }
}