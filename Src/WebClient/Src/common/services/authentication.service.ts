import { BehaviorSubject, Observable } from 'rxjs/Rx';
import { EventEmitter, Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';

import { Log, User, UserManager } from 'oidc-client';

import { ConfigurationReader } from './../../infrastructure/services/configuration-reader.service';

import { SessionService } from './session.service';

@Injectable()
export class AuthenticationService {
    mgr: UserManager;

    userLoadededEvent: EventEmitter<User> = new EventEmitter<User>();
    currentUser: User;
    loggedInSubject = new BehaviorSubject(false);
    loggedIn = false;

    constructor(
        private http: Http,
        configReader: ConfigurationReader,
        private sessionService: SessionService) {

            Log.logger = console;

            this.setReturnUrl();

            const settings: any = {
                authority: configReader.config.authentication.server,
                client_id: configReader.config.authentication.clientId,
                redirect_uri: `${window.location.origin}/login`,
                post_logout_redirect_uri: window.location.href, // configReader.config.authentication.server + '/Account/Logout'
                response_type: 'token',
                scope: 'api',

                silent_redirect_uri: `${window.location.origin}/silent-renew.html`,
                automaticSilentRenew: true,
                accessTokenExpiringNotificationTime: 4,
                // silentRequestTimeout:10000,

                filterProtocolClaims: true,
                loadUserInfo: true
            };
            this.mgr = new UserManager(settings)

            this.loggedInSubject.asObservable().subscribe((loggedIn) => {
                this.loggedIn = loggedIn;
            });

            this.mgr.getUser().then((user) => {
                    console.log(user);
                    if (user) {
                        this.addUserInformation(user);
                        this.userLoadededEvent.emit(user);
                    } else {
                        this.removeUserInformation();
                    }
                })
                .catch((err) => {
                    this.removeUserInformation();
                });

            this.mgr.events.addUserLoaded((user) => {
                this.addUserInformation(user);
            });

            this.mgr.events.addUserUnloaded((e) => {
                this.removeUserInformation();
            });
    }

    isLoggedInObs(): Observable<boolean> {
        return this.loggedInSubject.asObservable();
    }

    GetLoggedIn(): boolean {
        return this.loggedIn;
    }

    clearState() {
        this.mgr.clearStaleState().then(function () {
            console.log('clearStateState success');
        }).catch(function (e) {
            console.log('clearStateState error', e.message);
        });
    }

    getUser() {
        this.mgr.getUser().then((user) => {
            this.currentUser = user;
            this.userLoadededEvent.emit(user);
        }).catch(function (err) {
            console.log(err);
        });
    }

    removeUser() {
        this.mgr.removeUser().then(() => {
            this.userLoadededEvent.emit(null);
        }).catch(function (err) {
            console.log(err);
        });
    }

    startSigninMainWindow() {
        this.mgr.signinRedirect({ data: 'some data' }).then(function () {
            console.log('signinRedirect done');
        }).catch(function (err) {
            console.log(err);
        });
    }

    endSigninMainWindow() {
        this.mgr.signinRedirectCallback().then( (user) => {
            console.log('signed in');
            console.log(user);
            this.checkIfDualogAdmin(user);
        }).catch(function (err) {
            console.log(err);
        });
    }

    startSignoutMainWindow() {
        this.removeUserInformation();
        this.mgr.getUser().then(user => {
            return this.mgr.signoutRedirect({ id_token_hint: user.id_token }).then(resp => {
                    console.log('signed out', resp);
                }).catch(function (err) {
                    console.log(err);
                });
        });
    };

    endSignoutMainWindow() {
        this.mgr.signoutRedirectCallback().then(function (resp) {
            console.log('signed out', resp);
        }).catch(function (err) {
            console.log(err);
        });
    };

    private addUserInformation(user) {
        this.currentUser = user;
        this.checkIfDualogAdmin(user);
        this.loggedInSubject.next(!(user === undefined));
    }

    private removeUserInformation() {

        this.sessionService.SetSelectedCompany(undefined);
        this.sessionService.SetSelectedShip(undefined);

        // unset the is Dualog Admin User property
        this.sessionService.IsDualogAdmin = false;

        this.loggedInSubject.next(false);
    }

    private setReturnUrl(): void {
        if (this.sessionService.GetReturnUrl() === undefined) {
            this.sessionService.SetReturnUrl(window.location.pathname);
        } else {
            if (window.location.href.indexOf(`${window.location.origin}/login`) === -1) {
                this.sessionService.SetReturnUrl(window.location.pathname);
            }
        }
    }

    /**
     * Check if the logged in user is a Dualog Admin based on the received token
     *
     * If the user is a Dualog Admin the DualogAdmin flag is set in the session
     *
     * @private
     * @param {any} user
     * @memberof AuthenticationService
     */
    private checkIfDualogAdmin(user): void {

        const data: any = this.getDataFromToken(user.access_token);

        // Check if the user is a Dualog Admin user
        if (data.xdadmin !== undefined) {
            this.sessionService.IsDualogAdmin = (data.xdadmin === 'true');
        } else {
            this.sessionService.IsDualogAdmin = false;
        }
    }

    private getDataFromToken(token: any) {
        let data = {};
        if (typeof token !== 'undefined') {
            const encoded = token.split('.')[1];
            data = JSON.parse(this.urlBase64Decode(encoded));
        }
        return data;
    }

    private urlBase64Decode(str: string) {
        let output = str.replace('-', '+').replace('_', '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                throw new Error('Illegal base64url string!');
        }
        return window.atob(output);
    }
}
