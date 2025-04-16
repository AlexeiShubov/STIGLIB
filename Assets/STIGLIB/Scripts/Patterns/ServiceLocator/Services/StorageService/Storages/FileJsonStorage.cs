using System;

namespace STIGRADOR
{
    public class FileJsonStorage : JsonStorageBase
    {
        protected override void SaveJson(string key, string json)
        {
            throw new NotImplementedException();
        }

        protected override string GetJson(string key)
        {
            throw new NotImplementedException();
        }
    }
}