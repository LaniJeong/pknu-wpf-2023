using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wp08_personlinfoApp.Models;

namespace wp08_personlinfoApp.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        // View에서 사용할 멤버변수 선언
        private string inFirstName;
        private string inLastName;
        private string inEmail;
        private DateTime inDate = DateTime.Now;     // 현재 날짜로 셋팅

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
                inLastName = value;
                RaisePropertyChanged(nameof(InLastName));  // "InLastName"
            }
        }

        public string InEmail {
            get => inEmail;
            set
            {
               inEmail = value;
                RaisePropertyChanged(nameof(InEmail));  // "InEmail"
            }
        }

        public DateTime InDate {
            get => inDate;
            set
            {
                inDate = value;
                RaisePropertyChanged(nameof(InDate));  // "InDate"
            }
        }

        // 출력
        public string OutFirstName {
            get => outFirstName;
            set
            {
                outFirstName = value;
                RaisePropertyChanged(nameof(OutFirstName));  // "OutFirstName"
            }
        }
    
        public string OutLastName{
            get => outLastName;
            set
            {
                outLastName = value;
                RaisePropertyChanged(nameof(OutLastName));  // "OutFirstName"
            }
        }
        public string OutEmail {
            get => outEmail;
            set
            {
                outEmail = value;
                RaisePropertyChanged(nameof(OutEmail));  // "OutEmail"
            }
        }
        public string OutDate {
            get => outDate; 
            set
            {
                outDate = value;
                RaisePropertyChanged(nameof(OutDate));  // "OutDate"
            }
        }
        public string OutAdult {
            get => outAdult; 
            set
            {
                outAdult = value;
                RaisePropertyChanged(nameof(OutAdult));  // "OutAdult"
            }
        }
        public string OutBirthday {
            get => outBirthday; 
            set
            {
                outBirthday = value;
                RaisePropertyChanged(nameof(OutBirthday));  // "OutBirthday"
            }
        }
        public string OutZodiac { 
            get => outZodiac;
            set
            {
                outZodiac = value;
                RaisePropertyChanged(nameof(OutZodiac));  // "OutZodiac"
            }
        }

        // 버튼 클릭에 대한 이벤트처리(명령)
        private ICommand proceedCommand;
        public ICommand ProceedCommand
        {
            get
            {
                return proceedCommand ?? (proceedCommand = new RelayCommand<object>(
                    o => Proceed(), o => !string.IsNullOrEmpty(inFirstName) &&
                                         !string.IsNullOrEmpty(inLastName) &&
                                         !string.IsNullOrEmpty(inEmail) &&
                                         !string.IsNullOrEmpty(inDate.ToString())));
            }
        }
        // 버튼클릭시 실제로 처리를 수행하는 메서드
        private void Proceed()
        {
            try
            {
                Person person = new Person(inFirstName, inLastName, inEmail, inDate);

                OutFirstName = person.FirstName;
                OutLastName = person.LastName;
                OutEmail = person.Email;
                OutDate = person.Date.ToString("yyyy년 MM월 dd일");
                OutAdult = person.IsAdult == true ? "성년" : "미성년";
                OutBirthday = person.IsBirthDay == true ? "생일" : "-";
                OutZodiac = person.Zodiac;
                // 계속 진행...
            }
            catch (Exception ex)
            {
                MessageBox.Show($"예외발생 {ex.Message}");
            }
        }
    }
}

