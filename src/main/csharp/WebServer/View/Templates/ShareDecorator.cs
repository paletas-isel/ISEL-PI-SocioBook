using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Model;

namespace WebServer.View.Templates
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
            return share.Text;
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
            string html = "<a class=\"thumbnail-share\" href=\"http://www.google.pt\"><img src=\"http://open.thumbshots.org/image.pxf?url={0}\"></a>";

            return string.Format(html, new HtmlString(share.Anchor.AbsoluteUri));
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

            return string.Format(html, new HtmlString(share.Anchor.AbsoluteUri));
        }

        #endregion
    }

    public class DecoratorComposite : IHtmlDecorator
    {
        private IHtmlDecorator[] _decorators;

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
        private IDictionary<Type, IHtmlDecorator> _container = 
            new Dictionary<Type, IHtmlDecorator>()
                {
                    {typeof(TextShare), new TextShareDecorator()}
                }

        public DecoratorContainer()
        {
            
        }

        public IHtmlDecorator CreateInstance(Share share)
        {
            Type exactType = share.GetType();

            
        }
    }

    public interface IDecoratorFactory<out T, in TI>
    {
        T CreateInstance(TI inst);
    }

    public class TextShareDecoratorFactory : IDecoratorFactory<TextShareDecorator, TextShare>
    {
        #region Implementation of IDecoratorFactory<out TextShareDecorator,in TextShare>

        public TextShareDecorator CreateInstance(TextShare inst)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class AnchorShareDecoratorFactory : IDecoratorFactory<AnchorShareDecorator, AnchorShare>
    {
        #region Implementation of IDecoratorFactory<out AnchorShareDecorator,in AnchorShare>

        public AnchorShareDecorator CreateInstance(AnchorShare inst)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class VideoShareDecoratorFactory : IDecoratorFactory<VideoShareDecorator, VideoShare>
    {
        #region Implementation of IDecoratorFactory<out VideoShareDecorator,in VideoShare>

        public VideoShareDecorator CreateInstance(VideoShare inst)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}