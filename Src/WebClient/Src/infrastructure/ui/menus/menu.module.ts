import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SidebarModule } from 'primeng/components/sidebar/sidebar';

import { InfrastructureModule } from './../../infrastructure.module';
import { PopupTopMenuComponent } from './popup-topmenu/popup-topmenu.component';
import { PopupMenuComponent } from './popup-menu/popup-menu.component';
import { TopMenuItemComponent } from './topmenu-item/topmenu-item.component';
import { TopMenuComponent } from './topmenu/topmenu.component';
import { GridMenuComponent } from './gridmenu/gridmenu.component';
import { MenuItemComponent } from './menu-item/menu-item.component';
import { MenuComponent } from './menu/menu.component';
import { LeftSubMenuItemComponent } from './leftsubmenu-item/leftsubmenu-item.component';
import { LeftSubMenuComponent } from './leftsubmenu/leftsubmenu.component';
import { LeftMenuComponent } from './leftmenu/leftmenu.component';
import { SlidingMenuComponent } from './slidingmenu/slidingmenu.component';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        InfrastructureModule,
        SidebarModule
    ],

    declarations: [
        GridMenuComponent,
        MenuComponent,
        MenuItemComponent,
        PopupMenuComponent,
        PopupTopMenuComponent,
        TopMenuComponent,
        TopMenuItemComponent,
        LeftMenuComponent,
        LeftSubMenuComponent,
        LeftSubMenuItemComponent,
        SlidingMenuComponent
    ],

    exports: [
        GridMenuComponent,
        MenuComponent,
        MenuItemComponent,
        PopupMenuComponent,
        PopupTopMenuComponent,
        TopMenuComponent,
        TopMenuItemComponent,
        LeftMenuComponent,
        LeftSubMenuComponent,
        LeftSubMenuItemComponent,
        SlidingMenuComponent,
    ]

})
export class MenuModule {}
