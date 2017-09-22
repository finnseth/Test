import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { InfrastructureModule } from './../../infrastructure.module';
import { PopupTopMenuComponent } from './popup-topmenu/popup-topmenu.component';
import { PopupMenuComponent } from './popup-menu/popup-menu.component';
import { TopMenuItemComponent } from './topmenu-item/topmenu-item.component';
import { TopMenuComponent } from './topmenu/topmenu.component';
import { GridMenuComponent } from './gridmenu/gridmenu.component';
import { MenuItemComponent } from './menu-item/menu-item.component';
import { MenuComponent } from './menu/menu.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        InfrastructureModule
    ],

    declarations: [
        GridMenuComponent,
        MenuComponent,
        MenuItemComponent,
        PopupMenuComponent,
        PopupTopMenuComponent,
        TopMenuComponent,
        TopMenuItemComponent
    ],

    exports: [
        GridMenuComponent,
        MenuComponent,
        MenuItemComponent,
        PopupMenuComponent,
        PopupTopMenuComponent,
        TopMenuComponent,
        TopMenuItemComponent
    ]

})
export class MenuModule {}
