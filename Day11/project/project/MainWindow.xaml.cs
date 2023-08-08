using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using project_CulturalProperties.Logics;
using project_CulturalProperties.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace project_CulturalProperties
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool isFavorite = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // API 받아오기
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
            var status = Convert.ToInt32(jsonResult["status"]);

            var resultCode = Convert.ToString(jsonResult["getTblClthrtStusInfo"]["header"]["resultCode"]);

            try
            {
                if (resultCode == "00")
                {
                    var data = jsonResult["getTblClthrtStusInfo"]["body"]["items"]["item"];
                    var json_array = data as JArray;

                    var culturalProperties = new List<Cultural_Properties>();
                    foreach (var cltrpp in json_array)
                    {
                        culturalProperties.Add(new Cultural_Properties
                        {
                            cultHeritNm = Convert.ToString(cltrpp["cultHeritNm"]),
                            addr = Convert.ToString(cltrpp["addr"]),
                            organManage = Convert.ToString(cltrpp["organManage"]),
                            number = Convert.ToString(cltrpp["number"]),
                            dates = Convert.ToDateTime(cltrpp["dates"]),
                            era = Convert.ToString(cltrpp["era"]),
                            kind = Convert.ToString(cltrpp["kind"]),
                            mainAgent = Convert.ToString(cltrpp["main"]),
                            installedYear = Convert.ToString(cltrpp["installedYear"]),
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

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BtnSearch_Click(sender, e);
            }
        }

        // DB에 저장
        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (GrdResult.Items.Count == 0)
            {
                await Commons.ShowMessageAsync("오류", "문화재를 선택 후 저장하세요");
                return;
            }
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    var query = @"INSERT INTO `cultural properties`
                                             (cultHeritNm,
                                             addr,
                                             organManage,
                                             number,
                                             dates,
                                             era,
                                             kind,
                                             mainAgent,
                                             installedYear,
                                             majorContents,
                                             lat,
                                             lng)
                                          VALUES
                                             (@cultHeritNm ,
                                             @addr ,
                                             @organManage ,
                                             @number ,
                                             @dates ,
                                             @era ,
                                             @kind ,
                                             @mainAgent ,
                                             @installedYear ,
                                             @majorContents ,
                                             @lat ,
                                             @lng )";

                    var insRes = 0;
                    foreach (var temp in GrdResult.SelectedItems)
                    {
                        if (temp is Cultural_Properties)
                        {
                            var item = temp as Cultural_Properties;

                            MySqlCommand cmd = new MySqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@cultHeritNm", item.cultHeritNm);
                            cmd.Parameters.AddWithValue("@addr", item.addr);
                            cmd.Parameters.AddWithValue("@organManage", item.organManage);
                            cmd.Parameters.AddWithValue("@number", item.number);
                            cmd.Parameters.AddWithValue("@dates", item.dates);
                            cmd.Parameters.AddWithValue("@era", item.era);
                            cmd.Parameters.AddWithValue("@kind", item.kind);
                            cmd.Parameters.AddWithValue("@mainAgent", item.mainAgent);
                            cmd.Parameters.AddWithValue("@installedYear", item.installedYear);
                            cmd.Parameters.AddWithValue("@majorContents", item.majorContents);
                            cmd.Parameters.AddWithValue("@lat", item.lat);
                            cmd.Parameters.AddWithValue("@lng", item.lng);

                            insRes += cmd.ExecuteNonQuery();
                        }
                    }
                    await Commons.ShowMessageAsync("저장", "DB저장 성공!!");
                    StsResult.Content = $"DB저장 {insRes}건 성공";
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"이미 저장되었습니다. 즐겨찾기를 확인하세요. \n{ex.Message}");
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtSearch.Focus();
        }
        
        // 지도
        private void GrdResult_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(isFavorite)
            {
                var selItem = GrdResult.SelectedItem as Fav;

                var mapWindow = new MapWindow(selItem.lat, selItem.lng);
                mapWindow.Owner = this;
                mapWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                mapWindow.ShowDialog();
            }
            else
            {
                var selItem = GrdResult.SelectedItem as Cultural_Properties;

                var mapWindow = new MapWindow(selItem.lat, selItem.lng);
                mapWindow.Owner = this;
                mapWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                mapWindow.ShowDialog();
            }
            
        }

        #region < 즐겨찾기 저장목록 불러오기 >
        private async void BtnFavSearch_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = null;
            TxtSearch.Text = string.Empty;

            List<Fav> list = new List<Fav>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Commons.myConnString))
                {
                    conn.Open();
                    var query = @"SELECT cultHeritNm,
                                         addr,
                                         organManage,
                                         number,
                                         dates,
                                         era,
                                         kind,
                                         mainAgent,
                                         installedYear,
                                         majorContents,
                                         lat,
                                         lng
                                    FROM `cultural properties`";
                    
                    var cmd = new MySqlCommand(query, conn);
                    var adapter = new MySqlDataAdapter(cmd);
                    var dSet = new DataSet();
                    adapter.Fill(dSet, "Fav");

                    foreach (DataRow dr in dSet.Tables["Fav"].Rows)
                    {
                        list.Add(new Fav
                        {
                            cultHeritNm = Convert.ToString(dr["cultHeritNm"]),
                            addr = Convert.ToString(dr["addr"]),
                            organManage = Convert.ToString(dr["organManage"]),
                            number = Convert.ToString(dr["number"]),
                            dates = Convert.ToDateTime(dr["dates"]),
                            era = Convert.ToString(dr["era"]),
                            kind = Convert.ToString(dr["kind"]),
                            mainAgent = Convert.ToString(dr["mainAgent"]),
                            installedYear = Convert.ToString(dr["installedYear"]),
                            majorContents = Convert.ToString(dr["majorContents"]),
                            lat = Convert.ToDouble(dr["lat"]),
                            lng = Convert.ToDouble(dr["lng"])
                        });
                    }
                    this.DataContext = list;
                    isFavorite = true;
                    StsResult.Content = $"즐겨찾기 {list.Count}건 조회완료";
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB조회 오류 {ex.Message}");
            }
        }

        #endregion

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (isFavorite == false)
            {
                await Commons.ShowMessageAsync("오류", "즐겨찾기만 삭제할 수 있습니다");
                return;
            }

            if (GrdResult.SelectedItems.Count == 0)
            {
                await Commons.ShowMessageAsync("오류", "삭제할 항목을 선택하세요");
                return;
            }
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection (Commons.myConnString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    var query = "DELETE FROM `cultural properties` WHERE cultHeritNm = @cultHeritNm";
                    var delRes = 0;

                    foreach (Fav item in GrdResult.SelectedItems)
                    {
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@cultHeritNm", item.cultHeritNm);

                        delRes += cmd.ExecuteNonQuery();
                    }

                    if (delRes == GrdResult.SelectedItems.Count)
                    {
                        await Commons.ShowMessageAsync("삭제", "DB삭제 성공");
                        StsResult.Content = $"즐겨찾기 {delRes}건 삭제완료";
                    }
                    else
                    {
                        await Commons.ShowMessageAsync("삭제", "DB삭제 일부 성공");
                    }
                }
            }
            catch (Exception ex)
            {
                await Commons.ShowMessageAsync("오류", $"DB삭제 오류 {ex.Message}");
            }

            BtnFavSearch_Click(sender, e);
        }
    }
}
