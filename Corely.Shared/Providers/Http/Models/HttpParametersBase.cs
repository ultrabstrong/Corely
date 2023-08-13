﻿using Corely.Shared.Extensions;

namespace Corely.Shared.Providers.Http.Models
{
    public abstract class HttpParametersBase : IHttpParameters
    {
        private readonly Dictionary<string, string> _parameters;
        private readonly Dictionary<string, string> _tempParameters;

        public HttpParametersBase()
            : this(new Dictionary<string, string>())
        {
        }

        public HttpParametersBase(string key, string value)
            : this(new Dictionary<string, string>() { { key, value } })
        {
        }

        public HttpParametersBase(Dictionary<string, string> parameters)
            : this(parameters, new Dictionary<string, string>())
        {
        }

        public HttpParametersBase(Dictionary<string, string> parameters, Dictionary<string, string> tempParameters)
        {
            _parameters = parameters;
            _tempParameters = tempParameters;
        }

        public string CreateParameters()
        {
            List<string> paramsToInclude = new();

            paramsToInclude.AddRange(
                _parameters.Select(CreateParameters));

            paramsToInclude.AddRange(
                _tempParameters.Select(CreateParameters));

            _tempParameters.Clear();

            return string.Join('&', paramsToInclude);
        }

        private string CreateParameters(KeyValuePair<string, string> keyValuePair)
        {
            return $"{keyValuePair.Key.UrlEncode()}={keyValuePair.Value.UrlEncode()}";
        }

        public void AddParameters(params (string key, string value)[] keyValuePairs)
        {
            AddValidParameters(_parameters.Add, keyValuePairs);
        }

        public void AddTempParameters(params (string key, string value)[] keyValuePairs)
        {
            AddValidParameters(_tempParameters.Add, keyValuePairs);
        }

        private void AddValidParameters(
            Action<string, string> addToDictionary,
            params (string key, string value)[] keyValuePairs)
        {
            foreach (var (key, value) in keyValuePairs)
            {
                ValidateParameter(key, value);
                addToDictionary(key, value);
            }
        }

        public abstract void ValidateParameter(string key, string value);

        public void RemoveParameters(params string[] keys)
        {
            RemoveParameters(_parameters.Remove, keys);
        }

        public void RemoveTempParameters(params string[] keys)
        {
            RemoveParameters(_tempParameters.Remove, keys);
        }

        private void RemoveParameters(
            Func<string, bool> removeFromDictonary,
            params string[] keys)
        {
            foreach (var key in keys)
            {
                ; removeFromDictonary(key);
            }
        }

        public string GetParameterValue(string key)
        {
            return _parameters[key];
        }

        public string GetTempParameterValue(string key)
        {
            return _tempParameters[key];
        }

        public bool HasParameters()
        {
            return _parameters.Count > 0;
        }

        public bool HasTempParameters()
        {
            return _tempParameters.Count > 0;
        }

        public int GetParameterCount()
        {
            return _parameters.Count;
        }

        public int GetTempParameterCount()
        {
            return _tempParameters.Count;
        }

    }
}
