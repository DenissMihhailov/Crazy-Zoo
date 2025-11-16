using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrazyZoo.Logging;

public class XmlLogger : ILogger
{
    private readonly string _path = "log.xml";

    private static readonly object _lock = new();

    public void Log(string message)
    {
        lock (_lock)
        {
            var doc = XDocument.Load(_path);

            var entry = new XElement("Log",
                new XElement("Time", DateTime.Now),
                new XElement("Message", message));

            doc.Root.Add(entry);
            doc.Save(_path);
        }
    }
}
