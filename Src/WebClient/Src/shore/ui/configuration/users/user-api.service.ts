import { Http, URLSearchParams } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { ConfigurationReader } from './../../../../infrastructure/services/configuration-reader.service';
import { JsonSchema } from './../../../../infrastructure/services/schema/';
import { Permission } from './../../../../infrastructure/domain/permission/permission';

import { SessionService } from './../../../../common/services/session.service';
import { AuthenticationService } from './../../../../common/services/authentication.service';
import { ApiService, PaginationInfo } from './../../../../common/services/api-base.service';



@Injectable()
export class UserApiService extends ApiService {

    constructor(
         http: Http,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         configurationReader: ConfigurationReader ) {

        super(http, authenticationService, sessionService, configurationReader);
    }

    public GetUsers(  paginationInfo?: PaginationInfo ): Observable<UserListDetails> {

        return super.Get<UserListDetails>( '/organization/shipping/user', paginationInfo.toURLSearchParams() );
    }

    public GetUser( id: number): Observable<UserDetail> {

        return super.Get<UserDetail>( `/organization/shipping/user/${id}`);
    }

    public GetUserSchema(): Observable<JsonSchema> {
         return super.GetSchema(`/organization/shipping/user/0`);
    }

    public PatchUserById(id: number, payload: any ): Observable<UserDetail> {
        return super.Patch<UserDetail>( `/organization/shipping/user/${id}`, payload );
    }

    public GetUserGroups(): Observable<UserGroup[]> {
        return super.Get('/organization/shipping/usergroup');
    }
}

export interface UserListDetails {
    value: User[];
    totalCount: number;
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

export interface UserPermission {
    name: string;
    allowType: string;
    origin: string;
}

