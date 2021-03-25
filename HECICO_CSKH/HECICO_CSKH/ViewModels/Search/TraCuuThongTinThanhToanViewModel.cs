using HECICO_CSKH.Global;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using HECICO_CSKH.Models.Search;
using Newtonsoft.Json;
using HECICO_CSKH.Models;
using System.Net.Http;

namespace HECICO_CSKH.ViewModels.Search
{
   public class TraCuuThongTinThanhToanViewModel : BaseViewModel
    {
        #region "Field"
        DateTime _fromdate;
        DateTime _todate;
        ObservableCollection<TraCuuThongTinThanhToanModel> _listTraCuu;
        ObservableCollection<CustomerByTel> _listCustomer;
        CustomerByTel _selectItem;
        #endregion

        #region "Contructor"
        public DateTime FromDate { get => _fromdate; set => SetProperty(ref _fromdate, value); }
        public DateTime ToDate { get => _todate; set => SetProperty(ref _todate, value); }
        public ObservableCollection<TraCuuThongTinThanhToanModel> ListTraCuu { get => _listTraCuu; set => SetProperty(ref _listTraCuu, value); }
        public ObservableCollection<CustomerByTel> ListCustomer { get => _listCustomer; set => SetProperty(ref _listCustomer, value); }
        public CustomerByTel SelectItem
        {
            get => _selectItem;
            set
            {
                SetProperty(ref _selectItem, value);
                try
                {
                    if (!CheckInternet())
                    {
                        return;
                    }
                    try
                    {
                        if (IsBusy == true) return;
                        IsBusy = true;
                        using (HttpClient client = new HttpClient())
                        {
                            ListTraCuu.Clear();
                            KhachHangModel khang = new KhachHangModel();
                            khang.MaKhang = _selectItem.MA_KHANG;
                            khang.Token = Preferences.Get(Config.Token, "");
                            khang.ThangBatDau = string.Format("{0:yyyyMM}", _fromdate);
                            khang.ThangKetThuc = string.Format("{0:yyyyMM}", _todate);
                            client.BaseAddress = new Uri(Config.Url);
                            var ok = client.PostAsJsonAsync("api/tracuuthongtinhoadon", khang);
                            string _json = ok.Result.Content.ReadAsStringAsync().Result;
                            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                            if (_json.Contains("error") == false && _json.Contains("[]") == false)
                            {
                                Int32 from = _json.IndexOf("[");
                                Int32 to = _json.IndexOf("]");
                                string result = _json.Substring(from, to - from + 1);
                                ListTraCuu = JsonConvert.DeserializeObject<ObservableCollection<TraCuuThongTinThanhToanModel>>(result);
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
                    finally
                    {
                        IsBusy = false;
                    }
                }
                catch (Exception)
                {

                    HideLoading();
                }
            }
        }
        public TraCuuThongTinThanhToanViewModel() 
        {
            DateTime date = DateTime.Now.Date.AddMonths(-3);
            _fromdate = new DateTime(date.Year, date.Month, 1).Date;
            _todate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            ListTraCuu = new ObservableCollection<TraCuuThongTinThanhToanModel>();
            ListCustomer = new ObservableCollection<CustomerByTel>();
            SearchCommand = new Command(OnSearchClicked);
            LoadCommand = new Command(OnLoadExcute);
        }




        #endregion

        #region "Method"
        private void OnSearchClicked(object obj)
        {
            try
            {
                if (!CheckInternet())
                {
                    return;
                }
                try
                {
                    if (IsBusy == true) return;
                    IsBusy = true;
                    using (HttpClient client = new HttpClient())
                    {
                        ListTraCuu.Clear();
                        KhachHangModel khang = new KhachHangModel();
                        khang.MaKhang = _selectItem.MA_KHANG;
                        khang.Token = Preferences.Get(Config.Token, "");
                        khang.ThangBatDau = string.Format("{0:yyyyMM}", _fromdate);
                        khang.ThangKetThuc = string.Format("{0:yyyyMM}", _todate);
                        client.BaseAddress = new Uri(Config.Url);
                        var ok = client.PostAsJsonAsync("api/tracuuthongtinhoadon", khang);
                        string _json = ok.Result.Content.ReadAsStringAsync().Result;
                        _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                        if (_json.Contains("error") == false && _json.Contains("[]") == false)
                        {
                            Int32 from = _json.IndexOf("[");
                            Int32 to = _json.IndexOf("]");
                            string result = _json.Substring(from, to - from + 1);
                            ListTraCuu = JsonConvert.DeserializeObject<ObservableCollection<TraCuuThongTinThanhToanModel>>(result);
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
                finally
                {
                    IsBusy = false;
                }
            }
            catch (Exception)
            {

                HideLoading();
            }
        }
        private async void OnLoadExcute(object obj)
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
                        khang.Token = Preferences.Get(Config.Token, "");
                        khang.Sdt = Preferences.Get(Config.PhoneNumber, "");
                        client.BaseAddress = new Uri(Config.Url);
                        var ok = client.PostAsJsonAsync("api/getlistcustbytel", khang);
                        string _json = ok.Result.Content.ReadAsStringAsync().Result;
                        _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                        if (_json.Contains("error") == false && _json.Contains("[]") == false)
                        {
                            Int32 from = _json.IndexOf("[");
                            Int32 to = _json.IndexOf("]");
                            string result = _json.Substring(from, to - from + 1);
                            ListCustomer = JsonConvert.DeserializeObject<ObservableCollection<CustomerByTel>>(result);
                            if (ListCustomer.Count > 0)
                            {
                                SelectItem = ListCustomer[0];
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
        #endregion

        #region "Command"
        public Command SearchCommand { get; }
        public Command LoadCommand { get; }
        #endregion
    }
}
