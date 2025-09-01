import { AppConsts } from '@shared/AppConsts';
import { UtilsService } from 'abp-ng2-module';

export class SignalRAspNetCoreHelper {
    static initSignalR(callback?: () => void): void {
        const encryptedAuthToken = new UtilsService().getCookieValue(AppConsts.authorization.encryptedAuthTokenName);

        (abp as any).signalr = {
            autoConnect: true,
            connect: undefined,
            hubs: undefined,
            qs: '',
<<<<<<< HEAD
            remoteServiceBaseUrl: AppConsts.remoteServiceBaseUrl,
=======
            remoteServiceBaseUrl: 'https://localhost:44311',
>>>>>>> 0a9bf45a6bf8c9ab792e887be9247ea1c0470777
            startConnection: undefined,
            url: '/signalr'
        };


        const script = document.createElement('script');
        if (callback) {
            script.onload = () => {
                callback();
            };
        }
        script.src = AppConsts.appBaseUrl + '/assets/abp/abp.signalr-client.js';
        document.head.appendChild(script);
    }
}
