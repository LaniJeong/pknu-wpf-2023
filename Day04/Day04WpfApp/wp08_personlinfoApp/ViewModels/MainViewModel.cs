using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wp08_personlinfoApp.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        // View에서 사용할 멤버변수 선언
        private string inFirstName;
        private string inLastName;
        private string inEmail;
        private DateTime inDate;

        // 결과 출력쪽 변수
        private string outFirstName;
        private string outLastName;
        private string outEmail;
        private string outDate;         // 출력시에는 string(문자열 대체)
        private string outAdult;
        private string outBirthday;
        private string outZodiac;

        // 실제 사용할 속성 
        public string InFirstName {
            get => inFirstName;
            set 
            {
                inFirstName = value;
                RaisePropertyChanged(nameof(InFirstName));  // "inFirstName"
            } 
        }

        public string InLastName {
            get => inLastName;
            set
            {
                InLastName = value;
                RaisePropertyChanged(nameof(InLastName));  // "InLastName"
            }
        }

        public string InEmail {
            get => inEmail;
            set
            {
                InEmail = value;
                RaisePropertyChanged(nameof(InEmail));  // "InEmail"
            }
        }

        public DateTime InDate {
            get => inDate;
            set
            {
                InDate = value;
                RaisePropertyChanged(nameof(InDate));  // "InDate"
            }
        }

        // 출력
        public string OutFirstName {
            get => outFirstName;
            set
            {
                OutFirstName = value;
                RaisePropertyChanged(nameof(OutFirstName));  // "OutFirstName"
            }
        }
    
        public string OutLastName{
            get => outLastName;
            set
            {
                OutLastName = value;
                RaisePropertyChanged(nameof(OutLastName));  // "OutFirstName"
            }
        }
        public string OutEmail {
            get => outEmail;
            set
            {
                OutEmail = value;
                RaisePropertyChanged(nameof(OutEmail));  // "OutEmail"
            }
        }
        public string OutDate {
            get => outDate; 
            set
            {
                OutDate = value;
                RaisePropertyChanged(nameof(OutDate));  // "OutDate"
            }
        }
        public string OutAdult {
            get => outAdult; 
            set
            {
                OutAdult = value;
                RaisePropertyChanged(nameof(OutAdult));  // "OutAdult"
            }
        }
        public string OutBirthday {
            get => outBirthday; 
            set
            {
                OutBirthday = value;
                RaisePropertyChanged(nameof(OutBirthday));  // "OutBirthday"
            }
        }
        public string OutZodiac { 
            get => outZodiac;
            set
            {
                OutZodiac = value;
                RaisePropertyChanged(nameof(OutZodiac));  // "OutZodiac"
            }
        }
    }
}

