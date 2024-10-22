
public static class Tools
{

    public static string TimeAgo(DateTime dateTime)
    {
        string result = string.Empty;
        var timeSpan = DateTime.Now.Subtract(dateTime);
       
        if (timeSpan <= TimeSpan.FromSeconds(60))
        {
            result = string.Format("{0} seconds ago..", timeSpan.Seconds);
        }
        else if (timeSpan <= TimeSpan.FromMinutes(60))
        {
            result = timeSpan.Minutes > 1 ?
                String.Format("about {0} minutes ago..", timeSpan.Minutes) :
                "about a minute ago..";
        }
        else if (timeSpan <= TimeSpan.FromHours(24))
        {
            result = timeSpan.Hours > 1 ?
                String.Format("about {0} hours ago..", timeSpan.Hours) :
                "about an hour ago..";
        }
        else if (timeSpan <= TimeSpan.FromDays(30))
        {
            result = timeSpan.Days > 1 ?
                String.Format("about {0} days ago..", timeSpan.Days) :
                "yesterday..";
        }
        else if (timeSpan <= TimeSpan.FromDays(365))
        {
            result = timeSpan.Days > 30 ?
                String.Format("about {0} months ago..", timeSpan.Days / 30) :
                "about a month ago..";
        }
        else
        {
            result = timeSpan.Days > 365 ?
                String.Format("about {0} years ago..", timeSpan.Days / 365) :
                "about a year ago..";
        }

        return result;
    }

    //print spec
        public static string GetNameFromEmail(string email, string stopAt = "@")
        {
            if (!String.IsNullOrWhiteSpace(email))
            {
                int charLocation = email.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0)
                {
                    return email.Substring(0, charLocation);
                }
            }

            return String.Empty;
        }
    
}

