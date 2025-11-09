using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace CrazyZoo.Logging;

public class JsonLogger : ILogger
{
    private readonly string _path = "log.json";

    public void Log(string message)
    {
        var entry = new { Time = DateTime.Now, Message = message };
        var json = JsonSerializer.Serialize(entry);
        File.AppendAllText(_path, json + Environment.NewLine);
    }
}
