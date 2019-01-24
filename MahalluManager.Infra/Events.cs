using Prism.Events;

namespace MahalluManager.Infra {
    public class MyEventAggregator {
        private static object _lock = new object();
        private static IEventAggregator eventAggregator = null;
        private MyEventAggregator() {

        }
        public static IEventAggregator GetEventAggregator() {
            lock(_lock) {
                if(eventAggregator == null) {
                    eventAggregator = new EventAggregator();
                }
                return eventAggregator;
            }
        }
    }
}
