using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Json;

namespace SiDaMingZhu.Services
{
    public class JsonSimpleDataStore<T> : ISimpleDataStore<T>
    {
        private String _fileName;

        public JsonSimpleDataStore(string filename,T target)
        {
            _fileName = filename;

            Item = target;

            LoadItemsFromIsolatedStorage();
        }


        private void LoadItemsFromIsolatedStorage()
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (fileStore.FileExists(_fileName))
                {
                    using (var fileStream = fileStore.OpenFile(_fileName, System.IO.FileMode.Open))
                    {
                        var jsonSerializer = new DataContractJsonSerializer(typeof(T));
                        T item = (T)jsonSerializer.ReadObject(fileStream);
                        Item = item;
                    }
                }
            }
        }

        public void RollBack()
        {
            LoadItemsFromIsolatedStorage();
        }

        public void Commit()
        {
            using (var fileStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStream = fileStore.OpenFile(_fileName, System.IO.FileMode.Create))
                {
                    var jsonSerializer = new DataContractJsonSerializer(typeof(T));
                    jsonSerializer.WriteObject(fileStream, Item);
                }

            }
        }

        public T Item
        {
            get;
            private set;
        }
    }
}
