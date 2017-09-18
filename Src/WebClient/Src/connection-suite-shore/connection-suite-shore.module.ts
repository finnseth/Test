import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { CommonModule } from '@angular/common';
import { DualogCommonModule } from '../dualog-common/dualog-common.module';
import { HttpModule } from '@angular/http';
import { MenuService } from 'connection-suite-shore/services/menu.service';
import { NgModule } from '@angular/core';
import { PermissionService } from 'connection-suite-shore/services/permission.service';
import { SessionService } from 'connection-suite-shore/services/session.service';
import { UserGroupService } from 'connection-suite-shore/services/userGroup.service';

@NgModule({
    imports: [
        CommonModule,
        DualogCommonModule,
        HttpModule,
    ],

    declarations: [],

    providers: [
        PermissionService,
        AuthenticationService,
        MenuService,
        AuthGuard,
        SessionService,
        UserGroupService
    ]

})
export class ConnectionSuiteShoreModule { }
