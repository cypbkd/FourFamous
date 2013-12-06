using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiDaMingZhu.Models;
using System.Collections.ObjectModel;

namespace SiDaMingZhu.Services
{
    public interface IDataStore<T>
    {
        ObservableCollection<T> Items { get; }

        void RollBack();

        void Commit();
    }
}
