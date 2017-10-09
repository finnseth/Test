import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { InfrastructureModule } from './../../../infrastructure/infrastructure.module';
import { MenuModule } from './../../../infrastructure/ui/menus/menu.module';
import { DuaVerticalBarModule } from './../../../infrastructure/ui/dua-vertical-bar/dua-vertical-bar.module';
// import { HamburgerMenuModule } from './../../../infrastructure/ui/menus/hamburgermenu/hamburgermenu.module';
// import { SlidingMenuModule } from './../../../infrastructure/ui/menus/slidingmenu/slidingmenu.module';

import { LogoModule } from './../logo/logo.module';
import { SearchboxModule } from './../searchbox/searchbox.module';
import { HeaderComponent } from './header.component';
import { UserboxModule } from './../userbox/userbox.module';
import { SearchModule } from '../../../common/services/search/search.module';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DuaVerticalBarModule,
        MenuModule,
        SearchboxModule,
        UserboxModule,
        LogoModule,
        InfrastructureModule,
        SearchModule,
        // HamburgerMenuModule
        // SlidingMenuModule
    ],

    providers: [
    ],

    declarations: [
        HeaderComponent
    ],

    exports: [
        HeaderComponent
    ]

})
export class HeaderModule {}
