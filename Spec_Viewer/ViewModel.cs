using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Spec_Viewer.Model;

namespace Spec_Viewer
{
    public class ViewModel : INotifyPropertyChanged
    {
        private string _serial;
        private string _saveId;
        private DataTable _specDataTable;

        public DataTable SpecDataTable
        { 
            get => _specDataTable; 
            set 
            {
                _specDataTable=value;
                RaisePropertyChanged("SpecDataTable");
            }
        }

        public string Serial
        {
            get => _serial;
            set
            {
                _serial = value;
                RaisePropertyChanged("Serial");
            }
        }
        public string SaveId
        {
            get => _saveId;
            set
            {
                _saveId = value;
                RaisePropertyChanged("SaveId");
            }
        }


        public ViewModel()
        {
            SetSpecDAtaGrid();
        }

        private void SetSpecDAtaGrid()
        {
            SpecDataTable = DataBase.GetSpec(SaveId, Serial);
        }


        public ICommand SearchSpecCommand
        {
            get { return new RelayCommand(argument => SetSpecDAtaGrid()); }
        }

        #region INotify Property handler

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
