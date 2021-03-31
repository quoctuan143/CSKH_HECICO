using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using HECICO_CSKH.Models;
using Xamarin.Forms;
using HECICO_CSKH.Global;
using Newtonsoft.Json;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Net.Http;
using HECICO_CSKH.Dialog;

namespace HECICO_CSKH.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        #region "Field"
        CustomerModel _customer;
        ObservableCollection <   CustomerModel> _listcustomer;
        #endregion

        #region "Contructor"
        public ObservableCollection<ImageModel> ListImage { get; set; }
        public CustomerModel Customer { get => _customer; set => SetProperty(ref _customer,value ); }
        public ObservableCollection<CustomerModel> ListCustomer { get => _listcustomer; set => SetProperty(ref _listcustomer, value); }
        public HomeViewModel()
        {
            ListImage = new ObservableCollection<ImageModel>
            { 
                new ImageModel {ImagePath="http://smart.cpc.vn/HecicoApi/image/hinh1.jpg"},
                new ImageModel {ImagePath="http://smart.cpc.vn/HecicoApi/image/hinh2.jpg"},
                new ImageModel {ImagePath="http://smart.cpc.vn/HecicoApi/image/hinh3.jpg"}
            };
            ListCustomer = new ObservableCollection<CustomerModel>();
            LoadCommand = new Command(LoadExcute);
            CallMeCommand = new Command(CallMeExcute);
            OpenWebCommand =  new Command(async () => await Browser.OpenAsync("https://hecico.com.vn/"));
        }

        




        #endregion

        #region "Method"
        private async void LoadExcute(object obj)
        {
            try
            {
                if (!CheckInternet())
                {
                    return;
                }
                try
                {
                   
                    ShowLoading("Đang load dữ liệu");
                    await Task.Delay(1000);
                    using (HttpClient client = new HttpClient())
                    {
                        KhachHangModel khang = new KhachHangModel();
                        khang.MaKhang = Preferences.Get(Config.MaKhachHang, "");
                        khang.Token  = Preferences.Get(Config.Token , "");
                        client.BaseAddress = new Uri(Config.Url);
                        var ok = client.PostAsJsonAsync("api/getthongtinkhachhang", khang);                       
                        string _json = ok.Result.Content.ReadAsStringAsync().Result;
                        _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                        if (_json.Contains("error") == false && _json.Contains("[]") == false)
                        {
                            Int32 from = _json.IndexOf("[");
                            Int32 to = _json.IndexOf("]");
                            string result = _json.Substring(from, to - from + 1);
                            ListCustomer = JsonConvert.DeserializeObject<ObservableCollection<CustomerModel>>(result);
                            if (ListCustomer.Count > 0)
                            {
                                Customer = ListCustomer[0];
                            }
                            HideLoading();

                        }
                        else
                        {
                            HideLoading();
                        }
                    }



                   

                }
                catch (Exception ex)
                {
                    HideLoading();
                }
            }
            catch (Exception)
            {

                HideLoading();
            }
        }
        private void CallMeExcute(object obj)
        {
            try
            {
                PhoneDialer.Open("0766299989");
            }
            catch (Exception ex)
            {
                ShortAlert(ex.Message);
               
            }
        }

       
        #endregion

        #region "Command"
        public Command LoadCommand { get; }
        public Command CallMeCommand { get; }
        public Command OpenWebCommand { get; }
        #endregion
    }
}
