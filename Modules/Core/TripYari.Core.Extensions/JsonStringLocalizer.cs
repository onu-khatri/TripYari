using Microsoft.Extensions.Localization;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace TripYari.Core.Extensions
{

    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _strings = new();

        public JsonStringLocalizer(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;

            _strings = JsonSerializer.Deserialize<ConcurrentDictionary<string, ConcurrentDictionary<string, string>>>(File.ReadAllText(path));
        }

        public LocalizedString this[string name] => Get(name);

        public LocalizedString this[string name, params object[] arguments] => Get(name, arguments);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _strings.Keys.Select(key => new LocalizedString(key, key));
        }

        private LocalizedString Get(string name, params object[] arguments)
        {
            var localizedString = new LocalizedString(name, name, true, nameof(JsonStringLocalizer));

            if (string.IsNullOrWhiteSpace(name)) return localizedString;

            if (!_strings.TryGetValue(name, out var dictionary)) return localizedString;

            if (!dictionary.TryGetValue(CultureInfo.CurrentCulture.Name, out var value)) return localizedString;

            return new LocalizedString(name, string.Format(value, arguments), false, nameof(JsonStringLocalizer));
        }
    }

}