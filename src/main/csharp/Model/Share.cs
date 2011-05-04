using System;

namespace Model
{
    public class Share
    {
        private readonly DateTime _date;
        private readonly string _user;

        public Share(string user, DateTime date)
        {
            _user = user;
            _date = date;
        }

        public DateTime Date { get { return _date; } }

        public string User { get { return _user; } }
    }

    public class TextShare : Share
    {
        private readonly string _text;

        public TextShare(string user, string text) : base(user, DateTime.Now)
        {
            _text = text;
        }

        public string Text { get { return _text; } }
    }

    public class AnchorShare : Share
    {
        private readonly Uri _anchor;

        public AnchorShare(string user, Uri anchor) : base(user, DateTime.Now)
        {
            _anchor = anchor;
        }

        public Uri Anchor { get { return _anchor; } }
    }

    public class VideoShare : AnchorShare
    {
        public VideoShare(string user, Uri video) : base(user, video)
        {

        }
    }
}
