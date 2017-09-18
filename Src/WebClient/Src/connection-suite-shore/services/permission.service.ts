import { ApiService, ConfigurationReader } from 'dualog-common';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Http } from '@angular/http';
import { Injectable } from '@angular/core';
import { JsonSchema } from 'dualog-common';
import { Observable } from 'rxjs/Rx';
import { SessionService } from 'connection-suite-shore/services/session.service';

@Injectable()
export class PermissionService
       extends ApiService  {

    permissions: Permission[] = null;

    constructor(
         http: Http,
         configurationReader: ConfigurationReader,
         authenticationService: AuthenticationService,
         sessionService: SessionService ) {
        super(http, authenticationService, sessionService, configurationReader);

    }

    public getPermissions(): Observable<Permission[]> {

        // If the permissions already is loaded, use it
        if ( this.permissions !== null ) {
          return Observable.of( this.permissions );
        }

        // Load the permissions
        return super.Get<any[]>( '/permissions').map( p => {

            this.permissions = p.map( permission => {
                return {
                    name:  <string> permission['name'],
                    allowType:  <AccessRights>AccessRights[ <string> permission['allowType'] ]
                };
            });

            return this.permissions;
        } );
    }
}


export interface Permission {
    name: string;
    allowType: AccessRights;
    availability?: Availability;
}

export enum AccessRights {
    None = 0,
    Read = 1,
    Write = 2
}

export enum Availability {
    All = 1000,
    onlyShore = 1,
    // onlyShip = 2,
    onlyDualog = 3
}

export const PermissionMap  = {
    Information: {
        Dashboard: {
            name: 'EmailRestriction',   // @todo: wrong permissions
            allowType: AccessRights.Read,
            availability: Availability.All
        },
    },
    Config: {
        Wizard: {},
        Core: {
            User: {
                name: 'EmailRestriction',   // @todo: wrong permissions
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Communication: {
                name: 'EmailRestriction',   // @todo: wrong permissions
                allowType: AccessRights.Read,
                availability: Availability.All
            }
        },
        Email: {
            Technical: {
                name: 'EmailSetting',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Quarantine: {
                name: 'EmailRestriction',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Distributionlist: {
                name: 'EmailDistributionlist',
                allowType: AccessRights.Read,
                availability: Availability.onlyShore
            },
            Addressbook: {
                name: 'EmailAddressbook',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Operation: {
                name: 'EmailOperation',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
        },
        Filetransfer: {},
        Internet: {
            NetworkControl: {
                name: 'EmailRestriction',   // @todo: wrong permissions
                allowType: AccessRights.Read,
                availability: Availability.All
            }
        },
        Antivirus: {}
    },
    Operations: {
    },
    Dualog: {
        Overview: {
            name: 'EmailRestriction', // @todo: wrong permission
            allowType: AccessRights.Read,
            availability: Availability.onlyDualog
        }
    }
}
