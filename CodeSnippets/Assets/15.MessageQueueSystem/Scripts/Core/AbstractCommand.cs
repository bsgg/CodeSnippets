
namespace MessageQueue
{
    public abstract class AbstractCommandBase
    {
        protected MessengerService m_messageService { get; set; }

        protected abstract void OnStart(DependencyContainer a_dependancyContainer);

        internal virtual void Initialise(string a_targetMessage, DependencyContainer a_dependancyContainer)
        {
            m_messageService = ServiceLocator.GetService<MessengerService>();
            OnStart(a_dependancyContainer);
        }
    }

    public abstract class AbstractCommand<T> : AbstractCommandBase
    {
        protected abstract void OnExecute(T a_data);

        internal override void Initialise(string a_targetMessage, DependencyContainer a_dependancyContainer)
        {
            base.Initialise(a_targetMessage, a_dependancyContainer);
            m_messageService.Subscribe<T>(a_targetMessage, OnExecute);
        }
    }


    public abstract class AbstractCommand : AbstractCommandBase
    {
        protected abstract void OnExecute();

        internal override void Initialise(string a_targetMessage, DependencyContainer a_dependancyContainer)
        {
            base.Initialise(a_targetMessage, a_dependancyContainer);
            m_messageService.Subscribe(a_targetMessage, OnExecute);
        }
    }
}
