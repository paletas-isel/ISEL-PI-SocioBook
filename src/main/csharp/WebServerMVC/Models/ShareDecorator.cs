using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Model;

namespace WebServerMVC.Models
{
    public interface IHtmlDecorator
    {
        string ToHtmlView();
    }

    public abstract class ShareDecorator<T> : IHtmlDecorator where T : Share
    {
        private T _share;

        public ShareDecorator(T share)
        {
            _share = share;
        }

        public string ToHtmlView()
        {
            string html = "<div id=\"{0}\" class=\"share\"><div class=\"delete-share\">X</div><span class=\"user-share\">{1}</span><p>{2}</p></div>";

            return string.Format(html, _share.Stamp, _share.User, DoGetContent(_share));
        }

        protected abstract string DoGetContent(T share);
    }

    public class TextShareDecorator : ShareDecorator<TextShare>
    {
        public TextShareDecorator(TextShare share) : base(share)
        {
        }

        #region Overrides of ShareDecorator

        protected override string DoGetContent(TextShare share)
        {
            return share.Content;
        }

        #endregion
    }

    public class AnchorShareDecorator : ShareDecorator<AnchorShare>
    {
        public AnchorShareDecorator(AnchorShare share) : base(share)
        {
        }

        #region Overrides of ShareDecorator<AnchorShare>

        protected override string DoGetContent(AnchorShare share)
        {
            string html = "<a class=\"thumbnail-share\" href=\"{0}\"><img src=\"http://open.thumbshots.org/image.pxf?url={1}\"></a>";

            return string.Format(html, share.Content.AbsoluteUri, new HtmlString(share.Content.AbsoluteUri));
        }

        #endregion
    }

    public class VideoShareDecorator : ShareDecorator<VideoShare>
    {
        public VideoShareDecorator(VideoShare share) : base(share)
        {
        }

        #region Overrides of ShareDecorator<VideoShare>

        protected override string DoGetContent(VideoShare share)
        {
            string html = "<iframe class=\"video-share\" src=\"{0}\">";

            return string.Format(html, new HtmlString(share.Content.AbsoluteUri));
        }

        #endregion
    }

    public class DecoratorComposite : IHtmlDecorator
    {
        private readonly IHtmlDecorator[] _decorators;

        public DecoratorComposite(params IHtmlDecorator[] decorators)
        {
            _decorators = decorators;
        }

        #region Implementation of IHtmlDecorator

        public string ToHtmlView()
        {
            StringBuilder sb = new StringBuilder();
            foreach(IHtmlDecorator decorator in _decorators)
            {
                sb.Append(decorator.ToHtmlView());
            }
            return sb.ToString();
        }

        #endregion
    }

    public class DecoratorContainer
    {
        private readonly IDictionary<Type, IDecoratorFactory<IHtmlDecorator>> _container =
            new Dictionary<Type, IDecoratorFactory<IHtmlDecorator>>()
                {
                    {typeof (TextShare), new TextShareDecoratorFactory()},
                    {typeof (AnchorShare), new AnchorShareDecoratorFactory()},
                    {typeof (VideoShare), new VideoShareDecoratorFactory()}
                };
        
        public IHtmlDecorator CreateInstance(Share share)
        {
            Type exactType = share.GetType();

            return _container[exactType].CreateInstance(share);
        }
    }

    public interface IDecoratorFactory<out T> where T : IHtmlDecorator
    {
        T CreateInstance(Share inst);
    }

    public class TextShareDecoratorFactory : IDecoratorFactory<TextShareDecorator>
    {
        #region Implementation of IDecoratorFactory<out TextShareDecorator,in TextShare>

        public TextShareDecorator CreateInstance(Share inst)
        {
            return new TextShareDecorator(inst as TextShare);
        }

        #endregion
    }

    public class AnchorShareDecoratorFactory : IDecoratorFactory<AnchorShareDecorator>
    {
        #region Implementation of IDecoratorFactory<out AnchorShareDecorator,in AnchorShare>

        public AnchorShareDecorator CreateInstance(Share inst)
        {
            return new AnchorShareDecorator(inst as AnchorShare);
        }

        #endregion
    }

    public class VideoShareDecoratorFactory : IDecoratorFactory<VideoShareDecorator>
    {
        #region Implementation of IDecoratorFactory<out VideoShareDecorator,in VideoShare>

        public VideoShareDecorator CreateInstance(Share inst)
        {
            return new VideoShareDecorator(inst as VideoShare);
        }

        #endregion
    }
}