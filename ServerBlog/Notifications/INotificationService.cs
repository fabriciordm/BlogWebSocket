using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerBlog.Notifications
{
  
        public interface INotificationService
        {
            Task NotificationServiceMessage(string message);
            
        }
   
}
