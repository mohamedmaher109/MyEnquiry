
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using MyEnquiry_DataLayer.Models;
using Newtonsoft.Json;


namespace MyEnquiry_BussniessLayer.Helper
{

    public class NotificationManager
    {
        private  MyAppContext context;
        public NotificationManager(MyAppContext context)
        {
            this.context = context; 
        }
        public NotificationManager()
        {

        }

        private readonly static string Key = "";
        private readonly static string Id = "";
        private readonly static string webAddress = "https://fcm.googleapis.com/fcm/send";

       
        public void SetNotification(string title, string body, List<string> ToIds, string FromId, int orderID,bool Status)
        {
       
            foreach (var userid in ToIds)
            {
                var notification = new Notifications
                {
                    Title = title,
                    Message = body,
                    FromId = FromId,
                    ToId = userid,
                   
                    CreatedAt = DateTime.Now,
                    OrderId = orderID,
                    Status = Status 
                }; 
                context.Notifications.Add(notification); 
            }

            context.SaveChanges();
            
        }
        public void ReadNotification(string userId)
        {


            context.Notifications.Where(s=>s.ToId== userId).ToList().ForEach(not => not.Status = false);

            context.SaveChanges();
            
        }

    }
}
