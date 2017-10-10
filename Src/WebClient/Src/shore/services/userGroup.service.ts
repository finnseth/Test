import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';
import { Permission } from './../../infrastructure/domain/permission/permission';

import { SessionService } from '../../common/services/session.service';
import { AuthenticationService } from '../../common/services/authentication.service';
import { ApiService } from '../../common/services/api-base.service';


@Injectable()
export class UserGroupService extends ApiService {

    constructor(
        http: Http,
        configurationReader: ConfigurationReader,
        authenticationService: AuthenticationService,
        sessionService: SessionService ) {

            super(http, authenticationService, sessionService, configurationReader);
        }


    public getAll(): Observable<UserGroupSummary[]> {

        return super.Get<UserGroupSummary[]>( '/organization/shipping/usergroup');
    }
}

export interface UserGroupSummary {
    id: number;
    name: string;
    description: string;
}

export interface UserGroupDetials extends UserGroupSummary {

    needApproval: boolean;
    allowFax: boolean;
    faxDeliveryReport: boolean;
    faxNotDeliveredReport: boolean;
    allowTelex: boolean;
    telexDeliveryReport: boolean;
    telexNotDeliveredReport: boolean;
    allowSms: boolean;
    smsDeliveryReport: boolean;
    smsNotDeliveredReport: boolean;
    useImap: boolean;
    usePop: boolean;
    attachmentRule: number;
    daysAutoSignOff: number;
    deleteOldMessages: boolean;
    minutesLoginPerDay: number;
    concurrentDevices: number;
    signinApproval: boolean;
    daysToKeepMessages: number;
    ipTimeout: number;
    smtpRelay: boolean;
    permissions: Permission[];
}
