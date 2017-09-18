import { Injectable } from '@angular/core';
import { ApiService, ConfigurationReader } from 'dualog-common';
import { Permission } from './permission.service';
import { Observable } from 'rxjs/Rx';
import { Http } from '@angular/http';
import { SessionService } from 'connection-suite-shore/services/session.service';
import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';

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

        return super.Get<UserGroupSummary[]>( '/userGroups');
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
