using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using wp09_caliburnApp.Models;

namespace wp09_caliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        // Caliburn version업으로 변경
        private string firstName= "ChangKyun";
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                NotifyOfPropertyChange(() => FirstName);        // 속성값이 변경된걸 시스템에 알려주는 역할
                NotifyOfPropertyChange(nameof(CanClearName));
                NotifyOfPropertyChange(nameof(FullName));
            }
        }

        private string lastName = "Im";

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(nameof(CanClearName));
                NotifyOfPropertyChange(() => FullName); // 변화 통보
            }
        }

        public string FullName
        {
            get => $"{LastName}{FirstName}";
        }

        // 콤보박스에 바인딩할 속성
        // 이런 곳에서는 var를 쓸 수 없음
        private BindableCollection<Person> managers = new BindableCollection<Person>();

        public BindableCollection<Person> Managers
        {
            get => managers;
            set => managers = value;
        }

        // 콤보박스에 선택된 값을 지정할 속성
        private Person selectedManager;

        public Person SelectedManager
        {
            get => selectedManager;
            set
            {
                selectedManager = value;
                LastName = selectedManager.LastName;
                FirstName = selectedManager.FirstName;
                NotifyOfPropertyChange(nameof(SelectedManager));
            }
        }

        public MainViewModel()
        {
            // DB를 사용하면 여기서 DB접속 > 데이터 Select까지 해와야 함
            Managers.Add(new Person { FirstName = "Hyunwoo", LastName = "Son" });
            Managers.Add(new Person { FirstName = "Minhyuck", LastName = "Lee" });
            Managers.Add(new Person { FirstName = "Kihyun", LastName = "Yoo" });
            Managers.Add(new Person { FirstName = "Hyungwon", LastName = "Chae" });
            Managers.Add(new Person { FirstName = "Jooheon", LastName = "Lee" });
        }

        public void ClearName()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
        }

        public bool CanClearName
        {
            get => string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(LastName);
        }
    }
}
