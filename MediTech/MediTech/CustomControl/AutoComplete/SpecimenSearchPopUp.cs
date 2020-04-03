using MediTech.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.CustomControl
{
    public class SpecimenSearchPopUp
    {
        public ObservableCollection<Column> Columns { get; private set; }
        public SpecimenSearchPopUp()
        {
            Columns = new ObservableCollection<Column>() {
                new Column() { FieldName = "Name", Header = "Name" },
                new Column() { FieldName = "Code", Header = "Code" },
                new Column() { FieldName = "SpecimenType", Header = "Specimen Type" },
                new Column() { FieldName = "IsVolumeCollectionReqd", Header = "IsVolumne CollectionReqd" },
                new Column() { FieldName = "VolumeCollected", Header = "Volume Collected" }
            };
        }
    }
}
