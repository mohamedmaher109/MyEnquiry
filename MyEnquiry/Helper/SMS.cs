using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry.Helper
{
    public static class SMS
    {
        public static async Task<string> SendMessage(string PhoneCountryCode, string PhoneNumber, string Message)
        {
            if (PhoneNumber.StartsWith("+"))
            {
                PhoneNumber = PhoneNumber.Remove(0, 1);
            }
            if (PhoneNumber.StartsWith("2"))
            {
                PhoneNumber = PhoneNumber.Remove(0, 1);
            }
            if (!string.IsNullOrEmpty(PhoneCountryCode) && !string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrEmpty(Message))
            {
                string apiUrl = $"https://send.whysms.com/sms/api?action=send-sms&api_key=YWRtaW46YWRtaW4ucGFzc3dvcmQ=&to=+2{PhoneNumber}&from=SoluSpot&sms={Message}";
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var response = await client.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            return "تم";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            return null;
        }
       
    }
}
