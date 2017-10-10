import { Http } from '@angular/http';
import { Injectable } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { ConfigurationReader } from '../../infrastructure/services/configuration-reader.service';
import { Availability, AccessRights, Permission } from '../../infrastructure/domain/permission/permission';
import { MainMenuService, MainMenuItem } from './../../infrastructure/services/mainmenu.service';

import { SessionService } from '../../common/services/session.service';
import { AuthenticationService } from '../../common/services/authentication.service';
import { ApiService, PaginationInfo } from '../../common/services/api-base.service';

import { mainMenu } from './../app/app.mainmenu';


@Injectable()
export class PermissionService
       extends ApiService  {

    permissions: Permission[] = null;

    constructor(
         http: Http,
         configurationReader: ConfigurationReader,
         authenticationService: AuthenticationService,
         sessionService: SessionService,
         private menuService: MainMenuService
        ) {
        super(http, authenticationService, sessionService, configurationReader);

    }

    public getPermissions(): Observable<Permission[]> {

        // If the permissions already is loaded, use it
        if ( this.permissions !== null ) {
          return Observable.of( this.permissions );
        }

        // Load the permissions
        return super.Get<any[]>( '/organization/shipping/permission').map( p => {

            this.permissions = p['value'].map( permission => {
                return {
                    name:  <string> permission['name'],
                    allowType:  <AccessRights>AccessRights[ <string> permission['allowType'] ]
                };
            });

            return this.permissions;
        } );
    }

    private permissionMapToArray(object: Object): any[] {
        let output: any[] = [];
        for (const key in object) {
            if (key === 'route') {
                continue;
            }
            if (object.hasOwnProperty('allowType')) {
                output.push(object);
                break;
            } else if (object !== {}) {
                output = output.concat(this.permissionMapToArray(object[key]));
            }
        }
        return output;
    }

    /**
     * Check if the user got access to the menu item
     *
     * @param {any} _permissions
     * @returns {Observable<boolean>}
     *
     * @memberof AuthenticationService
     */
    public GetMenuAccess(_permissions): Observable<boolean> {

        const permissionsToCheck: Permission[] = this.permissionMapToArray(_permissions);
        // const userPermissions: Permission[] = this.permissionService.getPermissions();

        return this.getPermissions().map( userPermissions => {

            for (const permissionToCheck of permissionsToCheck) {
                const userPermissionFound = userPermissions.find(
                    p => p.name.toLocaleLowerCase() === permissionToCheck.name.toLocaleLowerCase()
                );

                if (userPermissionFound === undefined) {
                    continue;
                }

                if (userPermissionFound.allowType >= permissionToCheck.allowType) {
                    return this.CheckAvailability(permissionToCheck.availability);
                }
            }

            return false;
        })
    }

    /**
     * Checking if the availability is acceptable for the logged in user
     *
     * @private
     * @param {Availability} availability
     * @returns {boolean}
     *
     * @memberof AuthenticationService
     */
    public CheckAvailability(availability?: Availability): boolean {

        // Check if accessable for everyone
        if (availability === Availability.All) {
            return true;
        }

        // Check that accessable for shore users
        if (availability === Availability.onlyShore) {
            return true;
        }

        // Check if it is a Dualog feature
        if (availability === Availability.onlyDualog) {
            return this.sessionService.IsDualogAdmin;
        }

        return false;
    }

    FindPermissionByKey(perm: Permission) {
        const permissions = this.permissionMapToArray(PermissionMap);
        for (const permission of permissions) {
            if (permission.name === perm.name) {
                if (permission.route !== undefined) {
                    for (const route of permission.route) {
                        const item: MainMenuItem = this.menuService.GetMenuItemByRoute(route);
                        item.access = perm.allowType;
                        this.setParentPermission(item);
                    }
                }
            }
        }
    }

    private setParentPermission(item: MainMenuItem) {
        if (item.parent !== undefined) {
            if (item.parent.access === AccessRights.None) {
                item.parent.access = item.access;
                this.setParentPermission(item.parent);
            }
        }
    }
}

export const PermissionMap  = {
    Information: {
        route: ['/information'],
        Dashboard: {
            name: 'EmailRestriction',   // @todo: wrong permissions
            allowType: AccessRights.Read,
            availability: Availability.All,
            route: ['/information/dashboard']
        }
    },
    Config: {
        route: ['/configuration'],
        Organization: {
            route: ['/configuration/organization'],
            Company: {
                name: 'OrganizationCompany',
                allowType: AccessRights.Read,
                availability: Availability.All,
                route: ['/configuration/organization/company']
            },
            Ship: {
                name: 'OrganizationShip',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            UserGroup: {
                name: 'OrganizationUserGroup',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            User: {
                name: 'OrganizationUser',
                allowType: AccessRights.Read,
                availability: Availability.All,
                route: ['/configuration/organization/users']
            },
            Contact: {
                CompanyAddressBook: {
                    name: 'OrganizationContactCompanyAddressBook',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                ShipAddressBook: {
                    name: 'OrganizationContactShipAddressBook',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                UserAddressBook: {
                    name: 'OrganizationContactUserAddressBook',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                }
            }
        },
        Email: {
            route: ['/configuration/email'],
            TechnicalSetup: {
                name: 'EmailSetupTechnical',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Restriction: {
                name: 'EmailRestriction',
                allowType: AccessRights.Read,
                availability: Availability.All,
                route: [
                    '/configuration/email/restriction',
                    '/configuration/email/restriction/quarantine'
                ],
            },
            Address: {
                name: 'EmailAddressing',
                allowType: AccessRights.Read,
                availability: Availability.onlyShore
            },
            MessageCopying: {
                name: 'EmailMessageCopying',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Backup: {
                name: 'EmailMessageBackup',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            MessageTracking: {
                name: 'EmailMessageTracking',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
           /*  SetupDelivery: {
                name: 'EmailSetupDelivery',
                allowType: AccessRights.Read,
                availability: Availability.All
            } */
        },
        Filetransfer: {
            name: 'FileTransferSetup',
            allowType: AccessRights.Read,
            availability: Availability.All
        },
        Antivirus: {
            name: 'AntiVirusManagement',
            allowType: AccessRights.Read,
            availability: Availability.All
        },
        Network: {
            Architecture: {
                /* Carrier: {
                    name: 'NetworkCarrierSetup',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                }, */
                InternetGateway: {
                    name: 'NetworkInternetGateway',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                InternalSubnet: {
                    name: 'NetworkInternalSubnet',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                GPS: {
                    name: 'NetworkGPSSetup',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
            },
            FailoverLCR: {
                name: 'NetworkFailoverLCR',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            Restriction: {
                Bandwidth: {
                    name: 'NetworkBandwidth',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                Quota: {
                    name: 'NetworkQuota',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                Proxy: {
                    name: 'NetworkProxy',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                InternetRules: {
                    name: 'NetworkInternetRules',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                HTTPRules: {
                    name: 'NetworkHTTPRules',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                ContentFilter: {
                    name: 'NetworkPortNetworkContentFilterForward',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                DNS: {
                    name: 'NetworkDNS',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                MACFilter: {
                    name: 'NetworkMACFilter',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
            },
            Services: {
                NetworkControl: {
                    name: 'NetworkNetworkControl',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                Web4Sea: {
                    name: 'NetworkWeb4Sea',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                },
                Quota: {
                    name: 'NetworkQuota',
                    allowType: AccessRights.Read,
                    availability: Availability.All
                }
            },
            PortForward: {
                name: 'NetworkPortForward',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            DestinationPort: {
                name: 'NetworkDestinationPort',
                allowType: AccessRights.Read,
                availability: Availability.All
            },
            DHCP: {
                name: 'NetworkDHCP',
                allowType: AccessRights.Read,
                availability: Availability.All
            }
        }
    },
    Operations: {
    }
}
