using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.AspNet.Core;
using Twilio.Clients;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace MyEnquiry.Controllers
{
    public class VoiceController : TwilioController
    {


        private const string myphone = "+15558675310";
        private const string accid = "+15558675310";
        private const string token = "+15558675310";

        //[HttpPost]
        //public TwiMLResult Call()
        //{
        //    return TwiML();
        //}
    }
}
