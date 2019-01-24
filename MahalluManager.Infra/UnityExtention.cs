using Microsoft.Practices.Unity;
namespace MahalluManager.Infra {
    public static class UnityExtention {
        public static void RegisterForNavigation<T>(this IUnityContainer uc) {
            uc.RegisterType(typeof(object), typeof(T), typeof(T).FullName);
        }
    }
}
