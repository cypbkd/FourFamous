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
    public class JsonDataStore<T> : IDataStore<T>
    {
        private String _fileName;

        public JsonDataStore(string filename)
        {
            _fileName = filename;

            Items = new ObservableCollection<T>();
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
                        var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<T>));
                        var items = (ObservableCollection<T>)jsonSerializer.ReadObject(fileStream);
                        Items.Clear();
                        foreach (var f in items)
                        {
                            Items.Add(f);
                        }
                    }
                }
                else
                {
                    Items = new ObservableCollection<T>();
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
                    var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<T>));
                    jsonSerializer.WriteObject(fileStream, Items);
                }

            }
        }

        public ObservableCollection<T> Items
        {
            get;
            private set;
        }
    }
}
