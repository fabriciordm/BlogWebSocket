using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBLog.Notifications
{
  
        public interface INotificationService
        {
            Task NotificationServiceMessage();
            Task NotificationServiceMessage(string message);

    }
   
}
