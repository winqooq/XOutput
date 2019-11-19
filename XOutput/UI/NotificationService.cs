using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XOutput.Tools;

namespace XOutput.UI
{
    public class NotificationData
    {
        public string Text { get; set; }
        public string[] TextArguments { get; set; }
        public bool Important { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class NotificationService
    {
        public event Action<NotificationData> NotificationAdded;

        [ResolverMethod]
        public NotificationService()
        {

        }

        public void Add(string text, bool important = false)
        {
            Add(new NotificationData
            {
                Text = text,
                Duration = TimeSpan.MaxValue,
                Important = important,
                TextArguments = new string[0],
            });
        }

        public void Add(string text, string[] textArguments, bool important = false)
        {
            Add(new NotificationData
            {
                Text = text,
                Duration = TimeSpan.MaxValue,
                Important = important,
                TextArguments = textArguments,
            });
        }

        public void Add(string text, TimeSpan duration, bool important = false)
        {
            Add(new NotificationData
            {
                Text = text,
                Duration = duration,
                Important = important,
                TextArguments = new string[0],
            });
        }

        public void Add(string text, string[] textArguments, TimeSpan duration, bool important = false)
        {
            Add(new NotificationData
            {
                Text = text,
                Duration = duration,
                Important = important,
                TextArguments = textArguments,
            });
        }

        private void Add(NotificationData data)
        {
            NotificationAdded?.Invoke(data);
        }
    }
}
