using System;
using System.Linq;
using System.Text.Json;

namespace UI.Helpers.Extensions
{
    public static class JsonExtensions
    {
        public static string AsJson(this object o, bool indented = false) =>
            JsonSerializer.Serialize(o, new()
            {
                WriteIndented = indented
            });
    }
}
