import { ApiService, ConfigurationReader, JsonSchema } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Permission } from 'connection-suite-shore/services/permission.service';
import { SessionService } from 'connection-suite-shore/services/session.service';

@Injectable()
export class UserService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public GetUsers(): Observable<User[]> {

        return super.Get<User[]>( '/users');
    }

    public GetUser( id: number): Observable<UserDetail> {

        return super.Get<UserDetail>( `/users/${id}`);
    }

    public GetUserSchema(): Observable<JsonSchema> {
         return super.GetSchema(`/users/0`);
    }

    public PatchUserById(id: number, payload: any ): Observable<User> {
        return super.Patch<User>( `/users/${id}`, payload );
    }

    public GetUserGroups(): Observable<UserGroup[]> {
        return super.Get('/userGroups');
    }
}

export interface User {
    id: number;
    name: string;
    email: string;
    vesselName: string;
    isVesselUser: boolean;
}

export interface UserDetail {
  id: number;
  email: string,
  name: string,
  isVesselUser: boolean;
  phoneNumber: string;
  address: string;
  addrCpttoaddrBook: boolean;
  forwardCopy: boolean;
  forwardTo: string
  hideInAddressBook: boolean;
  messageFormat: string;

  userGroups: UserGroup[];
  permissions: Permission[];
}

export interface UserGroup {
    id: number;
    name: string;
    description: string;
}

