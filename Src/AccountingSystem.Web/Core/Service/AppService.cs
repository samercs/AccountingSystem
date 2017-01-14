using AccountingSystem.Data;

namespace AccountingSystem.Core.Service
{
    public class AppService: IAppService
    {
        public IDataContextFactory DataContextFactory { get; set; }
        public IAuthService AuthService { get; set; }
        public ICookieService CookieService { get; set; }

        public AppService(IDataContextFactory dataContextFactory,
                          IAuthService authService,
                          ICookieService cookieService)
        {
            DataContextFactory = dataContextFactory;
            AuthService = authService;
            CookieService = cookieService;
        }
    }
}
