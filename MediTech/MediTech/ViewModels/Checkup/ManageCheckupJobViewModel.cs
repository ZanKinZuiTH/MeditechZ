using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageCheckupJobViewModel : MediTechViewModelBase
    {
        CheckupJobContactModel modelCheckupJobContact;

        public void AssingModel(int checkupJobContactUID)
        {
            var modelData = DataService.Checkup.GetCheckupJobContactByUID(checkupJobContactUID);
            modelCheckupJobContact = modelData;
            AssignModelToProperties();
        }


        public void AssignModelToProperties()
        {

        }
    }
}
