import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { OverlayPanelModule } from 'primeng/primeng';

import { BodyComponent } from './ui/body/body.component';
import { BreadcrumbComponent } from './ui/breadcrumb/breadcrumb.component';
import { ContentComponent } from './ui/content/content.component';
import { GlogoComponent } from './ui/glogo/glogo.component';
import { HeaderComponent } from './ui/header/header.component';
import { LogoComponent } from './ui/logo/logo.component';
import { SearchService } from './services/search/search.service';
import { ShipSearchProvider } from './domain/ship/shipsearchprovider';
import { CompanySearchProvider } from './domain/company/companysearchprovider';
import { UserSearchProvider } from './domain/user/usersearchprovider';
import { SearchboxComponent } from './ui/searchbox/searchbox.component';
import { UserboxComponent } from './ui/userbox/userbox.component';
import { InfrastructureModule } from '../infrastructure/infrastructure.module';
import { DuaStatusBarModule } from '../infrastructure/ui/dua-status-bar/dua-status-bar.module';
import { DuaVerticalBarModule } from '../infrastructure/ui/dua-vertical-bar/dua-vertical-bar.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        OverlayPanelModule,
        InfrastructureModule,
        DuaStatusBarModule,
        DuaVerticalBarModule,
    ],

    providers: [SearchService, ShipSearchProvider, CompanySearchProvider, UserSearchProvider],

    declarations: [
        BodyComponent,
        BreadcrumbComponent,
        HeaderComponent,
        ContentComponent,
        LogoComponent,
        GlogoComponent,
        UserboxComponent,
        SearchboxComponent
    ],

    exports: [BodyComponent]
})
export class CommonsModule {}
