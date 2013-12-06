using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiDaMingZhu.Models;
using System.Collections.ObjectModel;

namespace SiDaMingZhu.Services
{
    public interface ISimpleDataStore<T>
    {
        T Item { get;  }

        void RollBack();

        void Commit();
    }
}
