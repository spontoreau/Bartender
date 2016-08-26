using System;
using System.Threading;
using Bartender;
using Cirrious.FluentLayouts.Touch;
using StructureMap;
using UIKit;
using XamarinApplication.Domain.Person.Create;
using XamarinApplication.Domain.Person.Read;
using XamarinApplication.Registries;

namespace XamarinApplication
{
    public partial class ViewController : UIViewController
    {
        private UIButton CommandButton { get; set; }
        private UIButton QueryButton { get; set; }
        private ICancellableAsyncDispatcher Dispatcher { get; set; }
        private CancellationTokenSource CancelToken { get; set; }

        protected ViewController(IntPtr handle) : base(handle)
        {
            var registry = new Registry();
            registry.IncludeRegistry<InfrastructureRegistry>();
            var container = new Container(registry);

            Dispatcher = container.GetInstance<ICancellableAsyncDispatcher>();
        }

        public override void ViewDidLoad()
        {
            CommandButton = new UIButton(UIButtonType.RoundedRect);
            CommandButton.SetTitle("Command", UIControlState.Normal);
            CommandButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            CommandButton.TouchUpInside += async (sender, e) => 
            {
                CancelToken = new CancellationTokenSource();
                var alert = UIAlertController.Create("Command", "Please cancel this task !", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (obj) => CancelToken.Cancel()));
                PresentViewController(alert, true, null);
                await Dispatcher.DispatchAsync(new CreatePersonCommand(), CancelToken.Token);
            };
            Add(CommandButton);

            QueryButton = new UIButton(UIButtonType.RoundedRect);
            QueryButton.SetTitle("Query", UIControlState.Normal);
            QueryButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            QueryButton.TouchUpInside += async (sender, e) =>
            {
                CancelToken = new CancellationTokenSource();
                var alert = UIAlertController.Create("Query", "Please cancel this task !", UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (obj) => CancelToken.Cancel()));
                PresentViewController(alert, true, null);
                await Dispatcher.DispatchAsync<GetPersonQuery, GetPersonReadModel>(new GetPersonQuery(), CancelToken.Token);
            };
            Add(QueryButton);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.AddConstraints(
                CommandButton.AtTopOf(View).Plus(50),
                CommandButton.AtRightOf(View).Minus(10),
                CommandButton.AtLeftOf(View).Minus(10),

                QueryButton.AtTopOf(CommandButton).Plus(50),
                QueryButton.AtRightOf(View).Minus(10),
                QueryButton.AtLeftOf(View).Minus(10)
            );

            base.ViewDidLoad();
        }
    }
}

