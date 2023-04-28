using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using project.Models;
using project_CulturalProperties.Logics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project_CulturalProperties
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtSearch.Text))
            {
                await Commons.ShowMessageAsync("검색", "검색할 문화재를 입력하세요.");
            }

            try
            {
                CltrlPrprt(TxtSearch.Text);
            }
            catch (Exception ex)
            {

                await Commons.ShowMessageAsync("오류", $"오류발생 : {ex.Message}");
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch_Click(sender, e);
            }
        }

        // Open API
        private async void CltrlPrprt(string Name)
        {


            // API 받아오기
            string encoding_Name = HttpUtility.UrlEncode(Name, Encoding.UTF8);
            string openApiUrl = $"https://apis.data.go.kr/6260000/BusanTblClthrtStusService/getTblClthrtStusInfo?" +
                                "serviceKey=Q3R5dAv8J37EqouPg97nKYcw2aCt2RsYHZpjzKERhKAminzU%2FKlCYlr59sCIBSNROEHCIMixAkdLEW7eRo8xpA%3D%3D" +
                                "&pageNo=1&numOfRows=10&resultType=json";
            string result = string.Empty;

            WebRequest req = null;
            WebResponse res = null;
            StreamReader reader = null;

            try
            {
                req = WebRequest.Create(openApiUrl);    // URL을 넣어 객체를 생성
                res = await req.GetResponseAsync();     // 요청한 결과를 비동기 응답에 할당
                reader = new StreamReader(res.GetResponseStream());
                result = reader.ReadToEnd();    // json결과 텍스트로 저장


                Debug.WriteLine(result);
            }
            catch (Exception ex)
            {

                await Commons.ShowMessageAsync("오류", $"OpenAPI 조회 오류 {ex.Message}");
            }

            var jsonResult = JObject.Parse(result);
            //var status = Convert.ToInt32(jsonResult["status"]);
            var resultCode = Convert.ToString(jsonResult["getTblClthrtStusInfo"]["header"]["resultCode"]);

            try
            {
                if (resultCode == "00")
                {
                    var data = jsonResult["getTblClthrtStusInfo"]["body"]["items"]["item"];
                    var json_array = data as JArray;

                    var culturalProperties = new List<CulturalProperties>();
                    foreach (var cltrpp in json_array)
                    {
                        culturalProperties.Add(new CulturalProperties
                        {
                            cultHeritNm = Convert.ToString(cltrpp["cultHeritNm"]),
                            addr = Convert.ToString(cltrpp["addr"]),
                            organManage = Convert.ToString(cltrpp["organManage"]),
                            number = Convert.ToString(cltrpp["number"]),
                            dates = Convert.ToDateTime(cltrpp["dates"]),
                            era = Convert.ToString(cltrpp["era"]),
                            kind = Convert.ToString(cltrpp["kind"]),
                            mainAgent = Convert.ToString(cltrpp["main"]),
                            istalledYear = Convert.ToString(cltrpp["installedYear"]),
                            majorContents = Convert.ToString(cltrpp["majorContents"]),
                            lat = Convert.ToDouble(cltrpp["lat"]),
                            lng = Convert.ToDouble(cltrpp["lng"])
                        });
                    }
                    this.DataContext = culturalProperties;
                    StsResult.Content = $"OpenAPI {culturalProperties.Count}건 조회 완료";
                }
            }
            catch (Exception ex)
            {

                await Commons.ShowMessageAsync("오류", $"JSON 처리 오류 {ex.Message}");
            }
        }

        // Row 더블클릭 시 새창에 문화재 위치 출력
        private void GrdResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            var selItem = GrdResult.SelectedItem as CulturalProperties;

            var mapWindow = new MapWindow(selItem.lat, selItem.lng);
            mapWindow.Owner = this;
            mapWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            mapWindow.ShowDialog();
        }
    }
}
