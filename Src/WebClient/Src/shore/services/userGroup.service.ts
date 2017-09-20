import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { Permission } from './permission.service';
import { SessionService } from '../../shore/services/session.service';
import { ApiService } from '../../shore/services/api-base.service';
import { AuthenticationService } from '../../shore/services/authentication.service';
import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';

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
